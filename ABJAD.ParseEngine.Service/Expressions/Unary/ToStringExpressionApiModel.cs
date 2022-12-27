namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public class ToStringExpressionApiModel : UnaryExpressionApiModel
{
    public ToStringExpressionApiModel(ExpressionApiModel target) : base("toString", target)
    {
    }
}