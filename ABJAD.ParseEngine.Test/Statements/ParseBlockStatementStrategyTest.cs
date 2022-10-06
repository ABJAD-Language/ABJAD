using System;
using System.Collections.Generic;
using ABJAD.ParseEngine.Bindings;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using FluentAssertions;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Statements;

public class ParseBlockStatementStrategyTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();
    private readonly Mock<IBindingFactory> bindingFactory = new();

    public ParseBlockStatementStrategyTest()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.CLOSE_BRACE)).Returns(true);
    }

    [Fact]
    private void InstantiationFailsIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseBlockStatementStrategy(null, bindingFactory.Object));
    }

    [Fact]
    private void InstantiationFailsIfBindingFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseBlockStatementStrategy(tokenConsumer.Object, null));
    }

    [Fact]
    private void ConsumesOpenBrace()
    {
        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.OPEN_BRACE));
    }

    [Fact]
    private void ParsesBindingUntilReachesCloseBraceToken()
    {
        tokenConsumer.SetupSequence(c => c.CanConsume(TokenType.CLOSE_BRACE)).Returns(false).Returns(false)
            .Returns(true);
        GetStrategy().Parse();
        bindingFactory.Verify(f => f.Get(), Times.Exactly(2));
    }

    [Fact]
    private void ConsumesCloseBraceToken()
    {
        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.CLOSE_BRACE));
    }

    [Fact]
    private void ReturnsBlockStatementOnHappyPath()
    {
        var binding = new Mock<Binding>();
        tokenConsumer.SetupSequence(c => c.CanConsume(TokenType.CLOSE_BRACE)).Returns(false).Returns(true);
        bindingFactory.Setup(f => f.Get()).Returns(binding.Object);
        var expectedStatement = new BlockStatement(new List<Binding> { binding.Object });
        var statement = GetStrategy().Parse();
        statement.Should().BeEquivalentTo(expectedStatement, options => options.RespectingRuntimeTypes());
    }

    private ParseBlockStatementStrategy GetStrategy()
    {
        return new ParseBlockStatementStrategy(tokenConsumer.Object, bindingFactory.Object);
    }
}