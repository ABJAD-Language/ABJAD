using ABJAD.Parser.Domain.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Expressions;

public class MethodCallArgumentsParser : IMethodCallArgumentsParser
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParser expressionParser;

    public MethodCallArgumentsParser(ITokenConsumer tokenConsumer, ExpressionParser expressionParser)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(expressionParser);
        this.tokenConsumer = tokenConsumer;
        this.expressionParser = expressionParser;
    }

    public List<Expression> Parse()
    {
        var arguments = new List<Expression>();

        while (!tokenConsumer.CanConsume(TokenType.CLOSE_PAREN))
        {
            arguments.Add(expressionParser.Parse());

            if (NoMoreArgumentsExist())
            {
                break;
            }

            tokenConsumer.Consume(TokenType.COMMA);
        }

        return arguments;
    }

    private bool NoMoreArgumentsExist()
    {
        if (tokenConsumer.CanConsume(TokenType.COMMA))
        {
            return false;
        }

        if (tokenConsumer.CanConsume(TokenType.CLOSE_PAREN))
        {
            return true;
        }

        throw new FailedToParseExpressionException(tokenConsumer.GetCurrentLine(), tokenConsumer.GetCurrentIndex());
    }
}