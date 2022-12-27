namespace ABJAD.InterpretEngine.Service.Shared.Expressions.Unary;

public class AdditionPostfixApiModel
{
    public object Target { get; }

    public AdditionPostfixApiModel(object target)
    {
        Target = target;
    }
}