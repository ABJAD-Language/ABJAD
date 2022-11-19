namespace ABJAD.ParseEngine.Service.Statements;

public class IfElseStatementApiModel : StatementApiModel
{
    public IfStatementApiModel MainIfStatement { get; }
    public List<IfStatementApiModel> OtherIfStatements { get; }
    public StatementApiModel ElseBody { get; }

    public IfElseStatementApiModel(IfStatementApiModel mainIfStatement, List<IfStatementApiModel> otherIfStatements, StatementApiModel elseBody)
    {
        MainIfStatement = mainIfStatement;
        OtherIfStatements = otherIfStatements;
        ElseBody = elseBody;
        Type = "statement.ifElse";
    }
}