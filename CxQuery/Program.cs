// See https://aka.ms/new-console-template for more information

CxPathParser parser = new CxPathParser();
List<Query> qList = parser.ParseQuery("//IfElseStmt.then//InvokeMethodExpr[shortName='foo']");
foreach (Query q in qList)
{
    Console.WriteLine(q);
}