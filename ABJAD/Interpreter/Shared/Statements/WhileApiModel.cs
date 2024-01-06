namespace ABJAD.Interpreter.Shared.Statements;

public class WhileApiModel
{
    public object Condition { get; }
    public object Body { get; }

    public WhileApiModel(object condition, object body)
    {
        Condition = condition;
        Body = body;
    }
}