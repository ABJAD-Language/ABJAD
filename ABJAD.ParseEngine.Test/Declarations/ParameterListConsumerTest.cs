using System;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Shared;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Declarations;

public class ParameterListConsumerTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();
    private readonly Mock<ITypeConsumer> typeConsumer = new();

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParameterListConsumer(null, typeConsumer.Object));
    }

    [Fact]
    private void ThrowsExceptionIfTypeConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParameterListConsumer(tokenConsumer.Object, null));
    }

    [Fact]
    private void ReturnsEmptyListWhenNoParametersFound()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.CLOSE_PAREN)).Returns(true);
        var functionParameters = GetConsumer().Consume();
        Assert.Empty(functionParameters);
    }

    [Fact]
    private void ReturnsListOfOneParameterWhenOnlyOneExists()
    {
        tokenConsumer.SetupSequence(c => c.CanConsume(TokenType.CLOSE_PAREN)).Returns(false).Returns(true);

        typeConsumer.Setup(c => c.Consume()).Returns("type");
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "name" });

        var functionParameters = GetConsumer().Consume();
        Assert.Equal(1, functionParameters.Count);

        Assert.Equal("type", functionParameters[0].Type);
        Assert.Equal("name", functionParameters[0].Name);
    }

    [Fact]
    private void ReturnsListOfTwoParameterWhenExist()
    {
        tokenConsumer.SetupSequence(c => c.CanConsume(TokenType.CLOSE_PAREN)).Returns(false).Returns(false)
            .Returns(true);
        tokenConsumer.SetupSequence(c => c.CanConsume(TokenType.COMMA)).Returns(true).Returns(false);


        typeConsumer.SetupSequence(c => c.Consume()).Returns("type1").Returns("type2");
        tokenConsumer.SetupSequence(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "name1" })
            .Returns(new Token { Content = "name2" });

        var functionParameters = GetConsumer().Consume();
        Assert.Equal(2, functionParameters.Count);

        Assert.Equal("type1", functionParameters[0].Type);
        Assert.Equal("name1", functionParameters[0].Name);

        Assert.Equal("type2", functionParameters[1].Type);
        Assert.Equal("name2", functionParameters[1].Name);
    }

    private ParameterListConsumer GetConsumer()
    {
        return new ParameterListConsumer(tokenConsumer.Object, typeConsumer.Object);
    }
}