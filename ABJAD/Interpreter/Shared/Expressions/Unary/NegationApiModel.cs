namespace ABJAD.Interpreter.Shared.Expressions.Unary;

public class NegationApiModel
{
    public object Target { get; }

    public NegationApiModel(object target)
    {
        Target = target;
    }
}