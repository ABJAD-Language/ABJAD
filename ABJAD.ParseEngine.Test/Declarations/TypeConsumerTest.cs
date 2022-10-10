using System;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Types;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Declarations;

public class TypeConsumerTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new TypeConsumer(null));
    }

    [Fact]
    private void ConsumesStringTokenWhenTypeIsString()
    {
        tokenConsumer.Setup(c => c.Consume(TokenType.STRING)).Returns(new Token { Type = TokenType.STRING });
        tokenConsumer.Setup(c => c.CanConsume(TokenType.STRING)).Returns(true);

        var type = GetConsumer().Consume();

        tokenConsumer.Verify(c => c.Consume(TokenType.STRING));
        Assert.Equal("STRING", type.GetValue());
    }

    [Fact]
    private void ConsumesNumberWhenTypeIsNumber()
    {
        tokenConsumer.Setup(c => c.Consume(TokenType.NUMBER)).Returns(new Token { Type = TokenType.NUMBER });
        tokenConsumer.Setup(c => c.CanConsume(TokenType.NUMBER)).Returns(true);

        var type = GetConsumer().Consume();

        tokenConsumer.Verify(c => c.Consume(TokenType.NUMBER));
        Assert.Equal("NUMBER", type.GetValue());
    }

    [Fact]
    private void ConsumesBoolWhenTypeIsBool()
    {
        tokenConsumer.Setup(c => c.Consume(TokenType.BOOL)).Returns(new Token { Type = TokenType.BOOL });
        tokenConsumer.Setup(c => c.CanConsume(TokenType.BOOL)).Returns(true);

        var type = GetConsumer().Consume();

        tokenConsumer.Verify(c => c.Consume(TokenType.BOOL));
        Assert.Equal("BOOL", type.GetValue());
    }

    [Fact]
    private void ConsumesIdWhenTypeIsCustom()
    {
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "customType" });

        var type = GetConsumer().Consume();

        tokenConsumer.Verify(c => c.Consume(TokenType.ID));
        Assert.Equal("customType", type.GetValue());
    }

    [Fact]
    private void ConsumeTypeOrVoidConsumesStringTokenWhenTypeIsString()
    {
        tokenConsumer.Setup(c => c.Consume(TokenType.STRING)).Returns(new Token { Type = TokenType.STRING });
        tokenConsumer.Setup(c => c.CanConsume(TokenType.STRING)).Returns(true);

        var type = GetConsumer().ConsumeTypeOrVoid();

        tokenConsumer.Verify(c => c.Consume(TokenType.STRING));
        Assert.Equal("STRING", type.GetValue());
    }

    [Fact]
    private void ConsumeTypeOrVoidConsumesNumberWhenTypeIsNumber()
    {
        tokenConsumer.Setup(c => c.Consume(TokenType.NUMBER)).Returns(new Token { Type = TokenType.NUMBER });
        tokenConsumer.Setup(c => c.CanConsume(TokenType.NUMBER)).Returns(true);

        var type = GetConsumer().ConsumeTypeOrVoid();

        tokenConsumer.Verify(c => c.Consume(TokenType.NUMBER));
        Assert.Equal("NUMBER", type.GetValue());
    }

    [Fact]
    private void ConsumeTypeOrVoidConsumesBoolWhenTypeIsBool()
    {
        tokenConsumer.Setup(c => c.Consume(TokenType.BOOL)).Returns(new Token { Type = TokenType.BOOL });
        tokenConsumer.Setup(c => c.CanConsume(TokenType.BOOL)).Returns(true);

        var type = GetConsumer().ConsumeTypeOrVoid();

        tokenConsumer.Verify(c => c.Consume(TokenType.BOOL));
        Assert.Equal("BOOL", type.GetValue());
    }

    [Fact]
    private void ConsumeTypeOrVoidConsumesVoidWhenTypeIsVoid()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.VOID)).Returns(true);
        tokenConsumer.Setup(c => c.Consume(TokenType.VOID)).Returns(new Token { Type = TokenType.VOID });

        var type = GetConsumer().ConsumeTypeOrVoid();

        tokenConsumer.Verify(c => c.Consume(TokenType.VOID));
        Assert.Equal("VOID", type.GetValue());
    }

    [Fact]
    private void ConsumeTypeOrVoidConsumesIdWhenTypeIsCustom()
    {
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "customType" });

        var type = GetConsumer().ConsumeTypeOrVoid();

        tokenConsumer.Verify(c => c.Consume(TokenType.ID));
        Assert.Equal("customType", type.GetValue());
    }

    private TypeConsumer GetConsumer()
    {
        return new TypeConsumer(tokenConsumer.Object);
    }
}