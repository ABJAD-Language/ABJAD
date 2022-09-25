using ABJAD.ParseEngine.Expressions.Binary;
using ABJAD.ParseEngine.Expressions.Unary;
using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Expressions;

public class ParsingExpressionStrategy : ParsingStrategy<Expression>
{
    private List<Token> tokens;
    private int index;

    public Expression Parse(List<Token> tokens, int index)
    {
        Guard.Against.Negative(index);
        Guard.Against.Null(tokens);

        this.index = index;
        this.tokens = tokens.Where(t => t.Type != TokenType.WHITE_SPACE).ToList();
        Guard.Against.NullOrEmpty(this.tokens);
        return ParseAbstractSyntaxTree();
    }

    private Expression ParseAbstractSyntaxTree()
    {
        return ParseOrOperationExpression();
    }

    private Expression ParseBinaryExpression(Func<Expression> higherPrecedenceExpressionParser,
        params TokenType[] target)
    {
        var expression = higherPrecedenceExpressionParser.Invoke();

        while (Match(target))
        {
            var @operator = tokens[index++];
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
        var @operator = tokens[index++];

        if (UnaryExpressionRequiresGrouping(@operator))
        {
            return ParseUnaryExpressionWithGrouping(@operator);
        }

        var expression = ParsePostfixOrHigherPrecedenceExpression();

        return BuildUnaryExpression(@operator, expression);
    }

    private Expression ParseUnaryExpressionWithGrouping(Token @operator)
    {
        Consume(TokenType.OPEN_PAREN);
        var expression = ParsePostfixOrHigherPrecedenceExpression();
        Consume(TokenType.CLOSE_PAREN);

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
            _                   => throw new Exception()
        };
    }

    private PrefixSubtractionExpression BuildPrefixSubtractionExpressionIfEligible(Expression expression)
    {
        if (expression is PrimitiveExpression {Primitive: IdentifierPrimitive})
        {
            return new PrefixSubtractionExpression(expression);
        }

        throw new Exception();
    }

    private Expression BuildPrefixAdditionExpressionIfEligible(Expression expression)
    {
        if (expression is PrimitiveExpression {Primitive: IdentifierPrimitive})
        {
            return new PrefixAdditionExpression(expression);
        }

        index--;
        throw new Exception();
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
        var @operator = tokens[index++];

        return PostfixExpressionFactory.Get(expression, @operator.Type);
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

        if (TryConsume(TokenType.DOT))
        {
            return ParseInstanceStateExpression(parentIdentifier, GetPrimitive());
        }

        if (TryConsume(TokenType.OPEN_PAREN))
        {
            var arguments = ParseMethodCallArguments();
            Consume(TokenType.CLOSE_PAREN);
            return new CallExpression(new PrimitiveExpression(parentIdentifier), arguments);
        }

        return new PrimitiveExpression(primitive);
    }

    private bool Match(params TokenType[] target)
    {
        return tokens.Count > index && target.Contains(GetCurrentToken().Type);
    }

    private InstantiationExpression ParseInstantiationExpression()
    {
        Consume(TokenType.NEW);
        if (GetPrimitive() is not IdentifierPrimitive @class)
        {
            throw new Exception();
        }

        Consume(TokenType.OPEN_PAREN);
        var arguments = ParseMethodCallArguments();
        Consume(TokenType.CLOSE_PAREN);
        return new InstantiationExpression(new PrimitiveExpression(@class), arguments);
    }

    private Expression ParseInstanceStateExpression(IdentifierPrimitive parent, Primitive child)
    {
        if (child is not IdentifierPrimitive)
        {
            throw new Exception();
        }

        var parentIdentifierExpression = new PrimitiveExpression(parent);
        var childIdentifierExpression = new PrimitiveExpression(child);
        if (Match(TokenType.OPEN_PAREN))
        {
            return ParseInstanceMethodCallExpression(parentIdentifierExpression, childIdentifierExpression);
        }

        return new InstanceFieldExpression(parentIdentifierExpression, childIdentifierExpression);
    }

    private InstanceMethodCallExpression ParseInstanceMethodCallExpression(PrimitiveExpression parent,
        PrimitiveExpression child)
    {
        Consume(TokenType.OPEN_PAREN);
        var arguments = ParseMethodCallArguments();

        Consume(TokenType.CLOSE_PAREN);
        return new InstanceMethodCallExpression(parent, child, arguments);
    }

    private List<Expression> ParseMethodCallArguments()
    {
        var arguments = new List<Expression>();

        while (!Match(TokenType.CLOSE_PAREN))
        {
            arguments.Add(ParseAbstractSyntaxTree());

            if (NoMoreArgumentsExist())
            {
                break;
            }
        }

        return arguments;
    }

    private bool NoMoreArgumentsExist()
    {
        if (TryConsume(TokenType.COMMA))
        {
            return false;
        }

        if (Match(TokenType.CLOSE_PAREN))
        {
            return true;
        }

        throw new Exception();
    }

    private Expression ParseGroupExpression()
    {
        Consume(TokenType.OPEN_PAREN);
        var expression = ParseAbstractSyntaxTree();
        Consume(TokenType.CLOSE_PAREN);

        return new GroupExpression(expression);
    }

    private Primitive GetPrimitive()
    {
        if (NoMoreTokensToConsume())
        {
            throw new Exception();
        }

        var token = tokens[index++];
        return PrimitiveFactory.Get(token);
    }

    private bool NoMoreTokensToConsume()
    {
        return tokens.Count <= index;
    }

    private void Consume(TokenType targetType)
    {
        if (NoMoreTokensToConsume() || tokens[index++].Type != targetType)
        {
            throw new Exception();
        }
    }

    private bool TryConsume(TokenType targetType)
    {
        if (NoMoreTokensToConsume() || GetCurrentToken().Type != targetType)
        {
            return false;
        }

        index++;
        return true;
    }

    private int GetCurrentIndex()
    {
        return GetCurrentOrLastToken().Index;
    }

    private int GetCurrentLine()
    {
        return GetCurrentOrLastToken().Line;
    }

    private Token GetCurrentOrLastToken()
    {
        if (NoMoreTokensToConsume())
        {
            return tokens.Last();
        }

        return GetCurrentToken();
    }

    private Token GetCurrentToken()
    {
        return tokens[index];
    }
}