using ABJAD.ParseEngine.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Declarations;

public class TypeConsumer : ITypeConsumer
{
    private readonly ITokenConsumer tokenConsumer;

    public TypeConsumer(ITokenConsumer tokenConsumer)
    {
        Guard.Against.Null(tokenConsumer);
        this.tokenConsumer = tokenConsumer;
    }

    public string Consume()
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

    public string ConsumeTypeOrVoid()
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

        if (IsTypeVoid())
        {
            return tokenConsumer.Consume(TokenType.VOID).Type.ToString();
        }

        return tokenConsumer.Consume(TokenType.ID).Content;
    }

    private bool IsTypeVoid()
    {
        return tokenConsumer.CanConsume(TokenType.VOID);
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