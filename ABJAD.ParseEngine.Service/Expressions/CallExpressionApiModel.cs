using ABJAD.ParseEngine.Service.Primitives;

namespace ABJAD.ParseEngine.Service.Expressions;

public class CallExpressionApiModel : ExpressionApiModel
{
    public PrimitiveExpressionApiModel Method { get; } // TODO switch to string method when core class moves to IdentifierPrimitive
    public List<ExpressionApiModel> Arguments { get; }

    public CallExpressionApiModel(PrimitiveExpressionApiModel method, List<ExpressionApiModel> arguments)
    {
        Method = method;
        Arguments = arguments;
        Type = "expression.call";
    }
}