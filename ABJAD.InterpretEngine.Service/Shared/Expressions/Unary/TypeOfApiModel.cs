namespace ABJAD.InterpretEngine.Service.Shared.Expressions.Unary;

public class TypeOfApiModel
{
    public object Target { get; }

    public TypeOfApiModel(object target)
    {
        Target = target;
    }
}