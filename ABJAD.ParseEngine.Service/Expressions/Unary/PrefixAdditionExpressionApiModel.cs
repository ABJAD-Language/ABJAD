namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public class PrefixAdditionExpressionApiModel : UnaryExpressionApiModel
{
    public PrefixAdditionExpressionApiModel(ExpressionApiModel target) : base("expression.prefix.addition", target)
    {
    }
}