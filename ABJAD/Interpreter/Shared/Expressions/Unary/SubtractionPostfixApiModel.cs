namespace ABJAD.Interpreter.Shared.Expressions.Unary;

public class SubtractionPostfixApiModel
{
    public object Target { get; }

    public SubtractionPostfixApiModel(object target)
    {
        Target = target;
    }
}