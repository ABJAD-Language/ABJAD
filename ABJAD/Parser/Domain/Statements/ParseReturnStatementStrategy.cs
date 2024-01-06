using ABJAD.Parser.Domain.Expressions;
using ABJAD.Parser.Domain.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Statements;

public class ParseReturnStatementStrategy : ParseStatementStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParser expressionParser;

    public ParseReturnStatementStrategy(ITokenConsumer tokenConsumer, ExpressionParser expressionParser)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(expressionParser);
        this.tokenConsumer = tokenConsumer;
        this.expressionParser = expressionParser;
    }

    public Statement Parse()
    {
        tokenConsumer.Consume(TokenType.RETURN);
        var target = expressionParser.Parse();
        tokenConsumer.Consume(TokenType.SEMICOLON);
        return new ReturnStatement(target);
    }
}