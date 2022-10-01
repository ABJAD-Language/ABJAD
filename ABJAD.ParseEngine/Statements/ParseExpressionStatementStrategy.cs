using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Statements;

public class ParseExpressionStatementStrategy : ParseStatementStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParser expressionParser;

    public ParseExpressionStatementStrategy(ITokenConsumer tokenConsumer, ExpressionParser expressionParser)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(expressionParser);
        this.tokenConsumer = tokenConsumer;
        this.expressionParser = expressionParser;
    }

    public Statement Parse()
    {
        var expression = expressionParser.Parse();
        tokenConsumer.Consume(TokenType.SEMICOLON);
        return new ExpressionStatement(expression);
    }
}