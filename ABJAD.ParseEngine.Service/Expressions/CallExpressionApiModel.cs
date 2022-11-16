namespace ABJAD.ParseEngine.Service.Expressions;

public class CallExpressionApiModel : ExpressionApiModel
{
    public PrimitiveExpressionApiModel Method { get; }
    public List<ExpressionApiModel> Arguments { get; }

    public CallExpressionApiModel(PrimitiveExpressionApiModel method, List<ExpressionApiModel> arguments)
    {
        Method = method;
        Arguments = arguments;
        Type = "expression.call";
    }
}