
CxPathParser parser = new CxPathParser();

string[] tests =
{
    "//IfStmt.condition//MethodInvokeExpr[@shortName=\"foo\"]",
    "//AssignExpr.left",
    "//*.assigner",
    "//*.assignee",
    "//CastExpr[TargetType.TypeName = \"NSString\"]",
    "//MethodInvokeExpr/parent::IfStmt",
    "//*",
    "//MethodInvokeExpression/parent::*",
    "//MethodInvokeExpression/ancestor::IfStmt",
    "//TypeRef[@linePragma = 3]",
    "//*[@modifier=Private]"
};

foreach (string test in tests) {
    List<CxPathQuery> qList = parser.ParseQuery(test);
    Console.WriteLine("Output:\n" + parser.GenerateQuery(qList));
    Console.WriteLine();
}
