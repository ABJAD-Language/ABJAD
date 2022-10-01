using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Statements;

public class ParseAssignmentStatementStrategy : ParseStatementStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParser expressionParser;

    public ParseAssignmentStatementStrategy(ITokenConsumer tokenConsumer, ExpressionParser expressionParser)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(expressionParser);
        this.tokenConsumer = tokenConsumer;
        this.expressionParser = expressionParser;
    }

    public Statement Parse()
    {
        var targetToken = tokenConsumer.Consume(TokenType.ID);
        tokenConsumer.Consume(TokenType.EQUAL);
        var value = expressionParser.Parse();
        tokenConsumer.Consume(TokenType.SEMICOLON);
        return new AssignmentStatement(targetToken.Content, value);
    }
}