// See https://aka.ms/new-console-template for more information

CxPathParser parser = new CxPathParser();
List<CxPathQuery> qList = parser.ParseQuery("//IfElseStmt.then//InvokeMethodExpr[shortName='foo']");
foreach (CxPathQuery q in qList)
{
    Console.WriteLine(q);
}