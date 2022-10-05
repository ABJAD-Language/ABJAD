using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Statements;

public class ParseWhileStatementStrategy : ParseStatementStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParser expressionParser;
    private readonly IStatementStrategyFactory statementStrategyFactory;

    public ParseWhileStatementStrategy(ITokenConsumer tokenConsumer, ExpressionParser expressionParser,
        IStatementStrategyFactory statementStrategyFactory)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(expressionParser);
        Guard.Against.Null(statementStrategyFactory);
        this.tokenConsumer = tokenConsumer;
        this.expressionParser = expressionParser;
        this.statementStrategyFactory = statementStrategyFactory;
    }

    public Statement Parse()
    {
        tokenConsumer.Consume(TokenType.WHILE);

        tokenConsumer.Consume(TokenType.OPEN_PAREN);
        var condition = expressionParser.Parse();
        tokenConsumer.Consume(TokenType.CLOSE_PAREN);

        var body = statementStrategyFactory.Get().Parse();

        return new WhileStatement(condition, body);
    }
}