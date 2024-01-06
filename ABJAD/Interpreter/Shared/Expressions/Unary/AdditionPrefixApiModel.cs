namespace ABJAD.Interpreter.Shared.Expressions.Unary;

public class AdditionPrefixApiModel
{
    public object Target { get; }

    public AdditionPrefixApiModel(object target)
    {
        Target = target;
    }
}