using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Statements;

public class ParseWhileStatementStrategy : ParseStatementStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParser expressionParser;
    private readonly ParseStatementStrategy parseStatementStrategy;

    public ParseWhileStatementStrategy(ITokenConsumer tokenConsumer, ExpressionParser expressionParser,
        ParseStatementStrategy parseStatementStrategy)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(expressionParser);
        Guard.Against.Null(parseStatementStrategy);
        this.tokenConsumer = tokenConsumer;
        this.expressionParser = expressionParser;
        this.parseStatementStrategy = parseStatementStrategy;
    }

    public Statement Parse()
    {
        tokenConsumer.Consume(TokenType.WHILE);

        tokenConsumer.Consume(TokenType.OPEN_PAREN);
        var condition = expressionParser.Parse();
        tokenConsumer.Consume(TokenType.CLOSE_PAREN);

        var body = parseStatementStrategy.Parse();

        return new WhileStatement(condition, body);
    }
}