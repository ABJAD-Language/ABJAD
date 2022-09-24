using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Shared;

namespace ABJAD.ParseEngine.Expressions;

public class ParsingExpressionStrategy : ParsingStrategy<Expression>
{
    private List<Token> tokens;
    private int index;

    public Expression Parse(List<Token> tokens, int index)
    {
        this.index = index;
        this.tokens = tokens.Where(t => t.Type != TokenType.WHITE_SPACE).ToList();
        return ParseAbstractSyntaxTree();
    }

    private Expression ParseAbstractSyntaxTree()
    {
        return ParseOrOperationExpression();
    }

    private Expression ParseExpression(Func<Expression> higherPrecedenceExpressionParser, params TokenType[] target)
    {
        var expression = higherPrecedenceExpressionParser.Invoke();

        while (Match(target))
        {
            var @operator = tokens[index++];
            var secondOperand = higherPrecedenceExpressionParser.Invoke();
            expression = BuildBinaryExpression(@operator, expression, secondOperand);
        }

        return expression;
    }

    private static Expression BuildBinaryExpression(Token @operator, Expression expression, Expression secondOperand)
    {
        return @operator.Type switch
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
            _                       => expression
        };
    }

    private Expression ParseOrOperationExpression()
    {
        return ParseExpression(ParseAndOperationExpression, TokenType.OR);
    }

    private Expression ParseAndOperationExpression()
    {
        return ParseExpression(ParseEqualityCheckExpression, TokenType.AND);
    }

    private Expression ParseEqualityCheckExpression()
    {
        return ParseExpression(ParseComparisonExpression, TokenType.EQUAL_EQUAL, TokenType.BANG_EQUAL);
    }

    private Expression ParseComparisonExpression()
    {
        return ParseExpression(ParseAdditionExpression, TokenType.LESS_THAN, TokenType.LESS_EQUAL,
            TokenType.GREATER_THAN, TokenType.GREATER_EQUAL);
    }

    private Expression ParseAdditionExpression()
    {
        return ParseExpression(ParseMultiplicationExpression, TokenType.PLUS, TokenType.DASH);
    }

    private Expression ParseMultiplicationExpression()
    {
        return ParseExpression(ParseUnaryOrHigherPrecedenceExpression, TokenType.STAR, TokenType.SLASH,
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
            Consume(TokenType.OPEN_PAREN);
        }

        var expression = ParsePostfixOrHigherPrecedenceExpression();

        if (UnaryExpressionRequiresGrouping(@operator))
        {
            Consume(TokenType.CLOSE_PAREN);
        }

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
            _                   => throw new FailedToParseExpressionException(GetCurrentLine(), GetCurrentIndex())
        };
    }

    private PrefixSubtractionExpression BuildPrefixSubtractionExpressionIfEligible(Expression expression)
    {
        if (expression is PrimitiveExpression {Primitive: IdentifierPrimitive})
        {
            return new PrefixSubtractionExpression(expression);
        }

        index--;
        throw new InvalidPrefixExpressionException(GetCurrentLine(), GetCurrentIndex());
    }

    private Expression BuildPrefixAdditionExpressionIfEligible(Expression expression)
    {
        if (expression is PrimitiveExpression {Primitive: IdentifierPrimitive})
        {
            return new PrefixAdditionExpression(expression);
        }

        index--;
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
        if (expression is not PrimitiveExpression {Primitive: IdentifierPrimitive})
        {
            index--;
            throw new InvalidPostfixExpressionException(GetCurrentLine(), GetCurrentIndex());
        }

        var @operator = tokens[index++];

        if (@operator.Type == TokenType.PLUS_PLUS)
        {
            return new PostfixAdditionExpression(expression);
        }

        return new PostfixSubtractionExpression(expression);
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
            if (GetPrimitive() is IdentifierPrimitive childIdentifier)
            {
                return ParseInstanceStateExpression(parentIdentifier, childIdentifier);
            }

            throw new MissingExpressionException(GetCurrentLine(), GetCurrentIndex(), TokenType.ID.ToString());
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
        return tokens.Count > index && target.Contains(tokens[index].Type);
    }

    private InstantiationExpression ParseInstantiationExpression()
    {
        Consume(TokenType.NEW);
        if (GetPrimitive() is IdentifierPrimitive @class)
        {
            Consume(TokenType.OPEN_PAREN);
            var arguments = ParseMethodCallArguments();
            Consume(TokenType.CLOSE_PAREN);
            return new InstantiationExpression(new PrimitiveExpression(@class), arguments);
        }

        throw new MissingExpressionException(GetCurrentLine(), GetCurrentIndex(), "ID");
    }

    private Expression ParseInstanceStateExpression(IdentifierPrimitive parentIdentifier,
        IdentifierPrimitive childIdentifier)
    {
        var parentIdentifierExpression = new PrimitiveExpression(parentIdentifier);
        var childIdentifierExpression = new PrimitiveExpression(childIdentifier);
        if (Match(TokenType.OPEN_PAREN))
        {
            return ParseInstanceMethodCallExpression(parentIdentifierExpression, childIdentifierExpression);
        }

        return new InstanceFieldExpression(parentIdentifierExpression, childIdentifierExpression);
    }

    private InstanceMethodCallExpression ParseInstanceMethodCallExpression(
        PrimitiveExpression parentIdentifierExpression, PrimitiveExpression childIdentifierExpression)
    {
        Consume(TokenType.OPEN_PAREN);
        var arguments = ParseMethodCallArguments();

        Consume(TokenType.CLOSE_PAREN);
        return new InstanceMethodCallExpression(parentIdentifierExpression, childIdentifierExpression, arguments);
    }

    private List<Expression> ParseMethodCallArguments()
    {
        var arguments = new List<Expression>();

        try
        {
            while (!Match(TokenType.CLOSE_PAREN))
            {
                arguments.Add(ParseAbstractSyntaxTree());

                if (!MoreArgumentsExist())
                {
                    break;
                }
            }
        }
        catch (Exception e)
        {
            if (e is ArgumentOutOfRangeException)
            {
                throw new MissingTokenException(GetCurrentLine(), GetCurrentIndex() + 1,
                    TokenType.CLOSE_PAREN.ToString());
            }

            if (e is ParsingException cause)
            {
                throw new MissingTokenException(cause.Line, cause.Index, TokenType.CLOSE_PAREN.ToString());
            }

            throw;
        }

        return arguments;
    }

    private bool MoreArgumentsExist()
    {
        if (TryConsume(TokenType.COMMA))
        {
            return true;
        }

        if (Match(TokenType.CLOSE_PAREN))
        {
            return false;
        }

        throw new MissingTokenException(GetCurrentLine(), GetCurrentIndex(), TokenType.COMMA.ToString());
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
        if (tokens.Count <= index)
        {
            throw new MissingExpressionException(GetCurrentLine(), GetCurrentIndex() + 1);
        }

        var token = tokens[index++];
        return BuildPrimitive(token);
    }

    private static Primitive BuildPrimitive(Token token)
    {
        return token.Type switch
        {
            TokenType.NUMBER_CONST => NumberPrimitive.From(token.Content),
            TokenType.STRING_CONST => StringPrimitive.From(token.Content),
            TokenType.TRUE         => BoolPrimitive.True(),
            TokenType.FALSE        => BoolPrimitive.False(),
            TokenType.NULL         => NullPrimitive.Instance(),
            TokenType.ID           => IdentifierPrimitive.From(token.Content),
            _                      => throw new FailedToParseExpressionException(token.Line, token.Index)
        };
    }

    private void Consume(TokenType targetType)
    {
        if (tokens.Count <= index || tokens[index++].Type != targetType)
        {
            throw new MissingTokenException(GetCurrentLine(), GetCurrentIndex(), targetType.ToString());
        }
    }

    private bool TryConsume(TokenType targetType)
    {
        if (tokens.Count <= index || tokens[index].Type != targetType)
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
        if (tokens.Count <= index)
        {
            return tokens.Last();
        }

        return tokens[index];
    }
}