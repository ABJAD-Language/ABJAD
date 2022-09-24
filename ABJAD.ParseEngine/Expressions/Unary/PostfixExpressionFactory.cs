using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Expressions.Unary;

public static class PostfixExpressionFactory
{
    public static Expression Get(Expression expression, TokenType operatorType)
    {
        // TODO check if expression is an identifier primitive when we have line and index in expressions
        return operatorType switch
        {
            TokenType.PLUS_PLUS => new PostfixAdditionExpression(expression),
            TokenType.DASH_DASH => new PostfixSubtractionExpression(expression),
            _                   => throw new InvalidPostfixSyntaxExpressionException()
        };
    }
}