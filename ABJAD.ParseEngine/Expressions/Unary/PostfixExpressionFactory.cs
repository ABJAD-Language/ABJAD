using ABJAD.ParseEngine.Expressions.Unary.Postfix;
using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Expressions.Unary;

public static class PostfixExpressionFactory
{
    public static Expression Get(Expression expression, TokenType operatorType)
    {
        if (expression is not PrimitiveExpression {Primitive: IdentifierPrimitive})
        {
            throw new PostfixIllegalArgumentException();
        }

        return operatorType switch
        {
            TokenType.PLUS_PLUS => new PostfixAdditionExpression(expression),
            TokenType.DASH_DASH => new PostfixSubtractionExpression(expression),
            _                   => throw new InvalidPostfixSyntaxExpressionException()
        };
    }
}