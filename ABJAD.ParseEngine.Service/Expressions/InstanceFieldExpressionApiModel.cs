namespace ABJAD.ParseEngine.Service.Expressions;

public class InstanceFieldExpressionApiModel : ExpressionApiModel
{
    public PrimitiveExpressionApiModel Instance { get; }
    public List<PrimitiveExpressionApiModel> Fields { get; }

    public InstanceFieldExpressionApiModel(PrimitiveExpressionApiModel instance, List<PrimitiveExpressionApiModel> fields)
    {
        Instance = instance;
        Fields = fields;
        Type = "expression.instanceField";
    }
}