using System.Linq;
using System.Text.RegularExpressions;

enum PathType
{
    DirectChild, Descendant
}

public class CxPathQuery
{
    PathType pathInfo;
    string typeName;
    string member;
    string conditions;
 
    public CxPathQuery(params string[] components)
    {
        pathInfo = components[0] == "/" ? PathType.DirectChild : PathType.Descendant;
        typeName = components[1];
        member = components[2].Length > 0 ? components[2].Substring(1) : "";
        conditions = components[3].Length > 0 ? components[3].Substring(1, components[3].Length - 2) : "";
    }

    public override string ToString()
    {
        string ans = pathInfo == PathType.DirectChild ? " / " : " // ";
        ans += $"<{typeName}>";
        ans += member.Length > 0 ? $".{member}" : "";
        ans += conditions.Length > 0 ? $"[ {conditions} ]" : "";
        return ans;
    }

    public List<string> ToCxQL(Dictionary<string, Dictionary<string, string>> dt)
    {
        List<string> ans = new List<string>();
        // TODO|FIXME - For now, ignore parent info
        if (typeName != "*")
        {
            ans.Add($"result = result.FindByType<{typeName}>();");
        }
        if (member.Length> 0)
        {
            ans.Add($"result = result.{dt[typeName][member]};");
        }
        return ans;
    }

}

public class CxPathParser
{
    Dictionary<string, Dictionary<string, string>> paramDispatchTable;

	public CxPathParser()
    { 
        // Hardcoded, can then be read from a JSON or something
        paramDispatchTable = new Dictionary<string, Dictionary<string, string>>();
        foreach (string key in "IfStmt AssignExpr *".Split(' '))
        {
            paramDispatchTable[key] = new Dictionary<string, string>();
        }

        paramDispatchTable["IfStmt"]["condition"] = "CXSelectDomProperty<IfStmt>(x => x.Condition)";
        paramDispatchTable["IfStmt"]["then"] = "CXSelectDomProperty<IfStmt>(x => x.TrueStatements)";
        paramDispatchTable["IfStmt"]["else"] = "CXSelectDomProperty<IfStmt>(x => x.FalseStatements)";
        
        paramDispatchTable["AssignExpr"]["left"] = "CXSelectDomProperty<AssignExpr>(x => x.left)";
        paramDispatchTable["AssignExpr"]["right"] = "CXSelectDomProperty<AssignExpr>(x => x.right)";

        paramDispatchTable["*"]["assigner"] = "GetAssigner()";
        paramDispatchTable["*"]["assignee"] = "GetAssignee()";

    }

    public List<CxPathQuery> ParseQuery(string str)
    {
        Console.WriteLine($"Original Query: {str}");
        Regex component = new Regex(@"^(//?)([a-zA-Z*]+)((?:\.[a-z]+)?)((?:\[[^\]]+\])?)");

        List<CxPathQuery> queryList = new List<CxPathQuery>();
        while (component.Matches(str).Count > 0)
        {
            str = component.Replace(str, match =>
            {
                queryList.Add(new CxPathQuery(match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray()));
                return "";
            });
        }
        return queryList;
    }

    public string GenerateQuery(List<CxPathQuery> pathQueries)
    {
        List<string> result = new List<String>();
        result.Add("result = All;");
        foreach (CxPathQuery query in pathQueries) {
            result.AddRange(query.ToCxQL(paramDispatchTable));
        }
        return string.Join("\n", result);
    }
}
