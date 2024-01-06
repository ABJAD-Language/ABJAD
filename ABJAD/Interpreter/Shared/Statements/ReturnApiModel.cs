namespace ABJAD.Interpreter.Shared.Statements;

public class ReturnApiModel
{
    public object Target { get; }

    public ReturnApiModel(object target)
    {
        Target = target;
    }
}