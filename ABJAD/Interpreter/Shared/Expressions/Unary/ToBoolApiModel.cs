namespace ABJAD.Interpreter.Shared.Expressions.Unary;

public class ToBoolApiModel
{
    public object Target { get; }

    public ToBoolApiModel(object target)
    {
        Target = target;
    }
}