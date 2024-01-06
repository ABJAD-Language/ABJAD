namespace ABJAD.Interpreter.Shared.Statements;

public class IfElseApiModel
{
    public IfApiModel MainIfStatement { get; }
    public List<IfApiModel> OtherIfStatements { get; }
    public object ElseBody { get; }

    public IfElseApiModel(IfApiModel mainIfStatement, List<IfApiModel> otherIfStatements, object elseBody)
    {
        MainIfStatement = mainIfStatement;
        OtherIfStatements = otherIfStatements;
        ElseBody = elseBody;
    }
}