using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Statements;

public class ParseIfElseStatementStrategy : ParseStatementStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParser expressionParser;
    private readonly ParseStatementStrategy statementParser;

    public ParseIfElseStatementStrategy(ITokenConsumer tokenConsumer, ExpressionParser expressionParser,
        ParseStatementStrategy statementParser)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(expressionParser);
        Guard.Against.Null(statementParser);
        this.tokenConsumer = tokenConsumer;
        this.expressionParser = expressionParser;
        this.statementParser = statementParser;
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
        }

        var elseBody = statementParser.Parse();
        return new IfElseStatement(primaryIfStatement, minorIfStatements, elseBody);
    }

    private IfStatement ParseIfStatement()
    {
        tokenConsumer.Consume(TokenType.IF);
        tokenConsumer.Consume(TokenType.OPEN_PAREN);
        var condition = expressionParser.Parse();
        tokenConsumer.Consume(TokenType.CLOSE_PAREN);
        var body = statementParser.Parse();

        return new IfStatement(condition, body);
    }
}