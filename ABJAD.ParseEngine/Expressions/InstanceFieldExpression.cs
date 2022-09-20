namespace ABJAD.ParseEngine.Expressions;

public class InstanceFieldExpression : Expression
{
    public InstanceFieldExpression(PrimitiveExpression instance, PrimitiveExpression field)
    {
        Instance = instance;
        Field = field;
    }

    public PrimitiveExpression Instance { get; }
    public PrimitiveExpression Field { get; }
}