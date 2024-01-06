namespace ABJAD.Parser.Expressions.Unary;

public class PostfixAdditionExpressionApiModel : UnaryExpressionApiModel
{
    public PostfixAdditionExpressionApiModel(ExpressionApiModel target) : base("postfix.addition", target)
    {
        // TODO switch to string target when PostfixAdditionExpression switches to IdentifierPrimitive target
    }
}