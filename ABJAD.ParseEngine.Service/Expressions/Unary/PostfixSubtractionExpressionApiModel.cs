namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public class PostfixSubtractionExpressionApiModel : UnaryExpressionApiModel
{
    public PostfixSubtractionExpressionApiModel(ExpressionApiModel target) : base("postfix.subtraction", target)
    {
    }
}