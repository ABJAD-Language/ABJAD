namespace ABJAD.InterpretEngine.Service.Shared.Statements;

public class IfApiModel
{
    public object Condition { get; }
    public object Body { get; }

    public IfApiModel(object condition, object body)
    {
        Condition = condition;
        Body = body;
    }
}