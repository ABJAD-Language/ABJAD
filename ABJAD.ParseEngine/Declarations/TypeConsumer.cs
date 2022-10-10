using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Types;
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

    public DataType Consume()
    {
        if (IsTypeString())
        {
            tokenConsumer.Consume(TokenType.STRING);
            return DataType.String();
        }

        if (IsTypeNumber())
        {
            tokenConsumer.Consume(TokenType.NUMBER);
            return DataType.Number();
        }

        if (IsTypeBool())
        {
            tokenConsumer.Consume(TokenType.BOOL);
            return DataType.Bool();
        }

        var customType = tokenConsumer.Consume(TokenType.ID).Content;
        return DataType.Custom(customType);
    }

    public DataType ConsumeTypeOrVoid()
    {
        if (IsTypeString())
        {
            tokenConsumer.Consume(TokenType.STRING);
            return DataType.String();
        }

        if (IsTypeNumber())
        {
            tokenConsumer.Consume(TokenType.NUMBER);
            return DataType.Number();
        }

        if (IsTypeBool())
        {
            tokenConsumer.Consume(TokenType.BOOL);
            return DataType.Bool();
        }

        if (IsTypeVoid())
        {
            tokenConsumer.Consume(TokenType.VOID);
            return DataType.Void();
        }

        var customType = tokenConsumer.Consume(TokenType.ID).Content;
        return DataType.Custom(customType);
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