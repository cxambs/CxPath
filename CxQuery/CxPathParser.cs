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

}

public class CxPathParser
{
	public CxPathParser()
	{
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
}
