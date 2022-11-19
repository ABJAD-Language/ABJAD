namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public class PrefixSubtractionExpressionApiModel : UnaryExpressionApiModel
{
    public PrefixSubtractionExpressionApiModel(ExpressionApiModel target) : base("expression.prefix.subtraction", target)
    {
    }
}