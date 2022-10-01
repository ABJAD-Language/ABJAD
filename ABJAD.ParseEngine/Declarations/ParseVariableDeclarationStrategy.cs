using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Declarations;

public class ParseVariableDeclarationStrategy : ParseDeclarationStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ExpressionParser expressionParser;

    public ParseVariableDeclarationStrategy(ITokenConsumer tokenConsumer, ExpressionParser expressionParser)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(expressionParser);
        this.tokenConsumer = tokenConsumer;
        this.expressionParser = expressionParser;
    }

    public Declaration Parse()
    {
        tokenConsumer.Consume(TokenType.VAR);
        var type = ConsumeVariableType();
        var name = tokenConsumer.Consume(TokenType.ID);

        Expression? value = null;
        if (tokenConsumer.CanConsume(TokenType.EQUAL))
        {
            tokenConsumer.Consume(TokenType.EQUAL);
            value = expressionParser.Parse();
            tokenConsumer.Consume(TokenType.SEMICOLON);
        }

        return new VariableDeclaration(type, name.Content, value);
    }

    private string ConsumeVariableType()
    {
        if (IsTypeString())
        {
            return tokenConsumer.Consume(TokenType.STRING).Type.ToString();
        }

        if (IsTypeNumber())
        {
            return tokenConsumer.Consume(TokenType.NUMBER).Type.ToString();
        }

        if (IsTypeBool())
        {
            return tokenConsumer.Consume(TokenType.BOOL).Type.ToString();
        }

        return tokenConsumer.Consume(TokenType.ID).Content;
    }

    private bool IsTypeBool()
    {
        return tokenConsumer.CanConsume(TokenType.BOOL);
    }

    private bool IsTypeNumber()
    {
        return tokenConsumer.CanConsume(TokenType.NUMBER);
    }

    private bool IsTypeString()
    {
        return tokenConsumer.CanConsume(TokenType.STRING);
    }
}