namespace ABJAD.InterpretEngine.Service.Shared.Expressions.Unary;

public class SubtractionPostfixApiModel
{
    public object Target { get; }

    public SubtractionPostfixApiModel(object target)
    {
        Target = target;
    }
}