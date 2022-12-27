namespace ABJAD.InterpretEngine.Service.Shared.Expressions.Unary;

public class ToNumberApiModel
{
    public object Target { get; }

    public ToNumberApiModel(object target)
    {
        Target = target;
    }
}