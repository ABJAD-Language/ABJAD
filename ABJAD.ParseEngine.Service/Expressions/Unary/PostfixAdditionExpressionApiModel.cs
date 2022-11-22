namespace ABJAD.ParseEngine.Service.Expressions.Unary;

public class PostfixAdditionExpressionApiModel : UnaryExpressionApiModel
{
    public PostfixAdditionExpressionApiModel(ExpressionApiModel target) : base("expression.postfix.addition", target)
    {
        // TODO switch to string target when PostfixAdditionExpression switches to IdentifierPrimitive target
    }
}