using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Statements;

public class ParseAssignmentStatementStrategy : ParseStatementStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParserFactory expressionParserFactory;

    public ParseAssignmentStatementStrategy(ITokenConsumer tokenConsumer,
        ExpressionParserFactory expressionParserFactory)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(expressionParserFactory);
        this.tokenConsumer = tokenConsumer;
        this.expressionParserFactory = expressionParserFactory;
    }

    public Statement Parse()
    {
        var targetToken = tokenConsumer.Consume(TokenType.ID);
        tokenConsumer.Consume(TokenType.EQUAL);
        var expressionParser = expressionParserFactory.CreateInstance(tokenConsumer);
        var value = expressionParser.Parse();
        tokenConsumer.Consume(TokenType.SEMICOLON);
        return new AssignmentStatement(IdentifierPrimitive.From(targetToken.Content), value);
    }
}