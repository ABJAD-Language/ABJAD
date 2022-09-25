using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Expressions.Binary;

public static class BinaryOperationExpressionFactory
{
    public static Expression Get(TokenType operatorType, Expression expression,
        Expression secondOperand)
    {
        return operatorType switch
        {
            TokenType.OR            => new OrOperationExpression(expression, secondOperand),
            TokenType.AND           => new AndOperationExpression(expression, secondOperand),
            TokenType.EQUAL_EQUAL   => new EqualityCheckExpression(expression, secondOperand),
            TokenType.BANG_EQUAL    => new InequalityCheckExpression(expression, secondOperand),
            TokenType.LESS_THAN     => new LessCheckExpression(expression, secondOperand),
            TokenType.LESS_EQUAL    => new LessOrEqualCheckExpression(expression, secondOperand),
            TokenType.GREATER_THAN  => new GreaterCheckExpression(expression, secondOperand),
            TokenType.GREATER_EQUAL => new GreaterOrEqualCheckExpression(expression, secondOperand),
            TokenType.PLUS          => new AdditionExpression(expression, secondOperand),
            TokenType.DASH          => new SubtractionExpression(expression, secondOperand),
            TokenType.STAR          => new MultiplicationExpression(expression, secondOperand),
            TokenType.SLASH         => new DivisionExpression(expression, secondOperand),
            TokenType.MODULO        => new ModuloExpression(expression, secondOperand),
            _                       => throw new ArgumentException(operatorType.ToString())
        };
    }
}