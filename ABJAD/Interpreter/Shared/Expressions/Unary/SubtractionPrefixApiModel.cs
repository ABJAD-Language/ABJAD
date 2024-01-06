namespace ABJAD.Interpreter.Shared.Expressions.Unary;

public class SubtractionPrefixApiModel
{
    public object Target { get; }

    public SubtractionPrefixApiModel(object target)
    {
        Target = target;
    }
}