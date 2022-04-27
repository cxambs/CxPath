
CxPathParser parser = new CxPathParser();

string[] tests =
{
    "//IfStmt.condition//MethodInvokeExpr[shortName='foo']",
    "//AssignExpr.left",
    "//*.assigner",
    "//*.assignee"
};

foreach (string test in tests) {
    List<CxPathQuery> qList = parser.ParseQuery(test);
    Console.WriteLine("Output:\n" + parser.GenerateQuery(qList));
    Console.WriteLine();
}
