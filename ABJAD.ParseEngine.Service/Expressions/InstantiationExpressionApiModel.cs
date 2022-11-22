namespace ABJAD.ParseEngine.Service.Expressions;

public class InstantiationExpressionApiModel : ExpressionApiModel
{
    public PrimitiveExpressionApiModel Class { get; } // TODO move to string class when core class moves to IdentifierPrimitive
    public List<ExpressionApiModel> Arguments { get; }

    public InstantiationExpressionApiModel(PrimitiveExpressionApiModel @class, List<ExpressionApiModel> arguments)
    {
        Class = @class;
        Arguments = arguments;
        Type = "expression.instantiation";
    }
}