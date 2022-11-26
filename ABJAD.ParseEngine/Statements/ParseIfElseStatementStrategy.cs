using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Statements;

public class ParseIfElseStatementStrategy : ParseStatementStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParser expressionParser;
    private readonly IStatementStrategyFactory statementStrategyFactory;

    public ParseIfElseStatementStrategy(ITokenConsumer tokenConsumer, ExpressionParser expressionParser,
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
        var primaryIfStatement = ParseIfStatement();
        if (!tokenConsumer.CanConsume(TokenType.ELSE))
        {
            return primaryIfStatement;
        }

        tokenConsumer.Consume(TokenType.ELSE);

        var minorIfStatements = new List<IfStatement>();
        while (tokenConsumer.CanConsume(TokenType.IF))
        {
            minorIfStatements.Add(ParseIfStatement());

            if (!tokenConsumer.CanConsume(TokenType.ELSE))
            {
                break;
            }

            tokenConsumer.Consume(TokenType.ELSE);
        }

        var elseBody = GetStatementStrategy().Parse();
        return new IfElseStatement(primaryIfStatement, minorIfStatements, elseBody);
    }

    private IfStatement ParseIfStatement()
    {
        tokenConsumer.Consume(TokenType.IF);
        tokenConsumer.Consume(TokenType.OPEN_PAREN);
        var condition = expressionParser.Parse();
        tokenConsumer.Consume(TokenType.CLOSE_PAREN);
        var body = GetStatementStrategy().Parse();

        return new IfStatement(condition, body);
    }

    private ParseStatementStrategy GetStatementStrategy()
    {
        return statementStrategyFactory.Get();
    }
}