namespace ABJAD.ParseEngine.Expressions;

public class InstanceFieldExpression : Expression
{
    public InstanceFieldExpression(PrimitiveExpression instance, IEnumerable<PrimitiveExpression> fields)
    {
        Instance = instance;
        Fields = fields;
    }

    public PrimitiveExpression Instance { get; }
    public IEnumerable<PrimitiveExpression> Fields { get; }
}