namespace ABJAD.ParseEngine.Service.Expressions;

public class InstanceFieldExpressionApiModel : ExpressionApiModel
{
    public PrimitiveExpressionApiModel Instance { get; } // TODO move to string instance when core class moves to IdentifierPrimitive
    public List<PrimitiveExpressionApiModel> Fields { get; } // TODO move to string fields when core class moves to IdentifierPrimitive

    public InstanceFieldExpressionApiModel(PrimitiveExpressionApiModel instance, List<PrimitiveExpressionApiModel> fields)
    {
        Instance = instance;
        Fields = fields;
        Type = "expression.instanceField";
    }
}