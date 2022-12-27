namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public class ToNumberExpressionApiModel : UnaryExpressionApiModel
{
    public ToNumberExpressionApiModel(ExpressionApiModel target) : base("toNumber", target)
    {
    }
}