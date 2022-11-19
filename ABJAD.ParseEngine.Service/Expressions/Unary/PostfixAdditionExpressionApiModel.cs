namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public class PostfixAdditionExpressionApiModel : UnaryExpressionApiModel
{
    public PostfixAdditionExpressionApiModel(ExpressionApiModel target) : base("expression.postfix.addition", target)
    {
    }
}