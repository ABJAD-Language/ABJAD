namespace ABJAD.ParseEngine.Statements;

public class IfElseStatement : Statement
{
    public IfElseStatement(IfStatement mainIfStatement, IEnumerable<IfStatement> otherIfStatements, Statement elseBody)
    {
        MainIfStatement = mainIfStatement;
        OtherIfStatements = otherIfStatements;
        ElseBody = elseBody;
    }

    public IfStatement MainIfStatement { get; }
    public IEnumerable<IfStatement> OtherIfStatements { get; }
    public Statement ElseBody { get; }
}