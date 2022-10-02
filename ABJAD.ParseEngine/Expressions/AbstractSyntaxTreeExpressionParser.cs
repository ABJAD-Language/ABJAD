using ABJAD.ParseEngine.Expressions.Binary;
using ABJAD.ParseEngine.Expressions.Unary;
using ABJAD.ParseEngine.Expressions.Unary.Postfix;
using ABJAD.ParseEngine.Expressions.Unary.Prefix;
using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Expressions;

public class AbstractSyntaxTreeExpressionParser : ExpressionParser
{
    private readonly ITokenConsumer consumer;

    public AbstractSyntaxTreeExpressionParser(ITokenConsumer consumer)
    {
        Guard.Against.Null(consumer);
        this.consumer = consumer;
    }

    public Expression Parse()
    {
        return ParseOrOperationExpression();
    }

    private Expression ParseBinaryExpression(Func<Expression> higherPrecedenceExpressionParser,
        params TokenType[] target)
    {
        var expression = higherPrecedenceExpressionParser.Invoke();

        while (Match(target))
        {
            var @operator = consumer.Consume();
            var secondOperand = higherPrecedenceExpressionParser.Invoke();
            expression = BinaryOperationExpressionFactory.Get(@operator.Type, expression, secondOperand);
        }

        return expression;
    }

    private Expression ParseOrOperationExpression()
    {
        return ParseBinaryExpression(ParseAndOperationExpression, TokenType.OR);
    }

    private Expression ParseAndOperationExpression()
    {
        return ParseBinaryExpression(ParseEqualityCheckExpression, TokenType.AND);
    }

    private Expression ParseEqualityCheckExpression()
    {
        return ParseBinaryExpression(ParseComparisonExpression, TokenType.EQUAL_EQUAL, TokenType.BANG_EQUAL);
    }

    private Expression ParseComparisonExpression()
    {
        return ParseBinaryExpression(ParseAdditionExpression, TokenType.LESS_THAN, TokenType.LESS_EQUAL,
            TokenType.GREATER_THAN, TokenType.GREATER_EQUAL);
    }

    private Expression ParseAdditionExpression()
    {
        return ParseBinaryExpression(ParseMultiplicationExpression, TokenType.PLUS, TokenType.DASH);
    }

    private Expression ParseMultiplicationExpression()
    {
        return ParseBinaryExpression(ParseUnaryOrHigherPrecedenceExpression, TokenType.STAR, TokenType.SLASH,
            TokenType.MODULO);
    }

    private Expression ParseUnaryOrHigherPrecedenceExpression()
    {
        if (Match(TokenType.DASH, TokenType.BANG, TokenType.PLUS_PLUS, TokenType.DASH_DASH, TokenType.NUMBER,
                TokenType.BOOL, TokenType.STRING, TokenType.TYPEOF))
        {
            return ParseUnaryExpression();
        }

        return ParsePostfixOrHigherPrecedenceExpression();
    }

    private Expression ParseUnaryExpression()
    {
        var @operator = consumer.Consume();

        if (UnaryExpressionRequiresGrouping(@operator))
        {
            return ParseUnaryExpressionWithGrouping(@operator);
        }

        var expression = ParsePostfixOrHigherPrecedenceExpression();

        return BuildUnaryExpression(@operator, expression);
    }

    private Expression ParseUnaryExpressionWithGrouping(Token @operator)
    {
        consumer.Consume(TokenType.OPEN_PAREN);
        var expression = ParsePostfixOrHigherPrecedenceExpression();
        consumer.Consume(TokenType.CLOSE_PAREN);

        return BuildUnaryExpression(@operator, expression);
    }

    private static bool UnaryExpressionRequiresGrouping(Token @operator)
    {
        return @operator.Type is TokenType.NUMBER or TokenType.BOOL or TokenType.STRING or TokenType.TYPEOF;
    }

    private Expression BuildUnaryExpression(Token @operator, Expression expression)
    {
        return @operator.Type switch
        {
            TokenType.DASH      => new NegativeExpression(expression),
            TokenType.BANG      => new NegationExpression(expression),
            TokenType.PLUS_PLUS => BuildPrefixAdditionExpressionIfEligible(expression),
            TokenType.DASH_DASH => BuildPrefixSubtractionExpressionIfEligible(expression),
            TokenType.NUMBER    => new ToNumberExpression(expression),
            TokenType.BOOL      => new ToBoolExpression(expression),
            TokenType.STRING    => new ToStringExpression(expression),
            TokenType.TYPEOF    => new TypeOfExpression(expression),
        };
    }

    private PrefixSubtractionExpression BuildPrefixSubtractionExpressionIfEligible(Expression expression)
    {
        if (expression is PrimitiveExpression { Primitive: IdentifierPrimitive })
        {
            return new PrefixSubtractionExpression(expression);
        }

        throw new InvalidPrefixExpressionException(GetCurrentLine(), GetCurrentIndex());
    }

    private Expression BuildPrefixAdditionExpressionIfEligible(Expression expression)
    {
        if (expression is PrimitiveExpression { Primitive: IdentifierPrimitive })
        {
            return new PrefixAdditionExpression(expression);
        }

        throw new InvalidPrefixExpressionException(GetCurrentLine(), GetCurrentIndex());
    }

    private Expression ParsePostfixOrHigherPrecedenceExpression()
    {
        var expression = ParsePrimitiveOrHigherPrecedenceExpression();
        if (Match(TokenType.PLUS_PLUS, TokenType.DASH_DASH))
        {
            return ParsePostfixExpression(expression);
        }

        return expression;
    }

    private Expression ParsePostfixExpression(Expression expression)
    {
        var @operator = consumer.Consume();

        return BuildPostfixExpression(expression, @operator.Type);
    }

    private Expression BuildPostfixExpression(Expression expression, TokenType operatorType)
    {
        if (expression is not PrimitiveExpression { Primitive: IdentifierPrimitive })
        {
            throw new InvalidPostfixExpressionException(GetCurrentLine(), GetCurrentIndex());
        }

        return operatorType switch
        {
            TokenType.PLUS_PLUS => new PostfixAdditionExpression(expression),
            TokenType.DASH_DASH => new PostfixSubtractionExpression(expression),
        };
    }

    private Expression ParsePrimitiveOrHigherPrecedenceExpression()
    {
        if (Match(TokenType.OPEN_PAREN))
        {
            return ParseGroupExpression();
        }

        if (Match(TokenType.NEW))
        {
            return ParseInstantiationExpression();
        }

        var primitive = GetPrimitive();
        if (primitive is not IdentifierPrimitive parentIdentifier)
        {
            return new PrimitiveExpression(primitive);
        }

        if (consumer.CanConsume(TokenType.DOT))
        {
            consumer.Consume(TokenType.DOT);
            var identifiers = new List<IdentifierPrimitive> { parentIdentifier };
            var firstLevelChild = GetPrimitive();
            if (firstLevelChild is not IdentifierPrimitive firstLevelChildIdentifier)
            {
                throw new FailedToParseExpressionException(GetCurrentLine(), GetCurrentIndex());
            }

            identifiers.Add(firstLevelChildIdentifier);
            while (consumer.CanConsume(TokenType.DOT))
            {
                consumer.Consume(TokenType.DOT);
                var child = GetPrimitive();
                if (child is not IdentifierPrimitive childIdentifier)
                {
                    throw new FailedToParseExpressionException(GetCurrentLine(), GetCurrentIndex());
                }

                identifiers.Add(childIdentifier);
            }

            return ParseInstanceStateExpression(identifiers);
        }

        if (consumer.CanConsume(TokenType.OPEN_PAREN))
        {
            consumer.Consume(TokenType.OPEN_PAREN);
            var arguments = ParseMethodCallArguments();
            consumer.Consume(TokenType.CLOSE_PAREN);
            return new CallExpression(new PrimitiveExpression(parentIdentifier), arguments);
        }

        return new PrimitiveExpression(primitive);
    }

    private bool Match(params TokenType[] target)
    {
        return consumer.CanConsume() && target.Contains(GetCurrentToken().Type);
    }

    private InstantiationExpression ParseInstantiationExpression()
    {
        consumer.Consume(TokenType.NEW);
        if (GetPrimitive() is not IdentifierPrimitive @class)
        {
            throw new FailedToParseExpressionException(GetCurrentLine(), GetCurrentIndex());
        }

        consumer.Consume(TokenType.OPEN_PAREN);
        var arguments = ParseMethodCallArguments();
        consumer.Consume(TokenType.CLOSE_PAREN);
        return new InstantiationExpression(new PrimitiveExpression(@class), arguments);
    }

    private Expression ParseInstanceStateExpression(List<IdentifierPrimitive> identifiers)
    {
        var identifierExpressions = identifiers.Select(child => new PrimitiveExpression(child)).ToList();
        if (Match(TokenType.OPEN_PAREN))
        {
            return ParseInstanceMethodCallExpression(identifierExpressions);
        }

        return new InstanceFieldExpression(identifierExpressions.First(), identifierExpressions.Skip(1));
    }

    private InstanceMethodCallExpression ParseInstanceMethodCallExpression(List<PrimitiveExpression> identifiers)
    {
        consumer.Consume(TokenType.OPEN_PAREN);
        var arguments = ParseMethodCallArguments();

        consumer.Consume(TokenType.CLOSE_PAREN);
        return new InstanceMethodCallExpression(identifiers.SkipLast(1), identifiers.Last(), arguments);
    }

    private List<Expression> ParseMethodCallArguments()
    {
        return new MethodCallArgumentsParser(consumer, this).Parse();
    }

    private Expression ParseGroupExpression()
    {
        consumer.Consume(TokenType.OPEN_PAREN);
        var expression = Parse();
        consumer.Consume(TokenType.CLOSE_PAREN);

        return new GroupExpression(expression);
    }

    private Primitive GetPrimitive()
    {
        if (!consumer.CanConsume())
        {
            throw new FailedToParseExpressionException(GetCurrentLine(), GetCurrentIndex());
        }

        var token = consumer.Consume();
        return PrimitiveFactory.Get(token);
    }

    private int GetCurrentIndex()
    {
        return GetCurrentToken().Index;
    }

    private int GetCurrentLine()
    {
        return GetCurrentToken().Line;
    }

    private Token GetCurrentToken()
    {
        return consumer.Peek();
    }
}