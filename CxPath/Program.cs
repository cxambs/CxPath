// See https://aka.ms/new-console-template for more information

CxPathParser parser = new CxPathParser();
List<CxPathQuery> qList = parser.ParseQuery("//IfStmt.condition//InvokeMethodExpr[shortName='foo']");
foreach (CxPathQuery q in qList)
{
    Console.WriteLine(q);
}
Console.WriteLine(parser.GenerateQuery(qList));