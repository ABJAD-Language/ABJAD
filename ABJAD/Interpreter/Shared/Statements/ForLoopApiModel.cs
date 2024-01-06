namespace ABJAD.Interpreter.Shared.Statements;

public class ForLoopApiModel
{
    public object Target { get; }
    public object Condition { get; }
    public object TargetCallback { get; }
    public object Body { get; }

    public ForLoopApiModel(object target, object condition, object targetCallback, object body)
    {
        Target = target;
        Condition = condition;
        TargetCallback = targetCallback;
        Body = body;
    }
}