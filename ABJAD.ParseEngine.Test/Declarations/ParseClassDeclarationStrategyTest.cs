using System;
using System.Collections.Generic;
using ABJAD.ParseEngine.Bindings;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Declarations;

public class ParseClassDeclarationStrategyTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();
    private readonly Mock<ParseDeclarationStrategy> blockDeclarationParser = new();

    public ParseClassDeclarationStrategyTest()
    {
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "className" });
        blockDeclarationParser.Setup(p => p.Parse()).Returns(new BlockDeclaration(new List<DeclarationBinding>()));
    }

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseClassDeclarationStrategy(null, blockDeclarationParser.Object));
    }

    [Fact]
    private void ThrowsExceptionIfParseDeclarationStrategyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseClassDeclarationStrategy(tokenConsumer.Object, null));
    }

    [Fact]
    private void ConsumesClassToken()
    {
        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.CLASS));
    }

    [Fact]
    private void ConsumesNameIdInOrder()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.CLASS)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "className" })
            .Callback(() => Assert.Equal(2, order++));

        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.CLASS));
        tokenConsumer.Verify(c => c.Consume(TokenType.ID));
    }

    [Fact]
    private void ParsesBlockStatementInOrder()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.CLASS)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "className" })
            .Callback(() => Assert.Equal(2, order++));
        blockDeclarationParser.Setup(c => c.Parse()).Returns(new BlockDeclaration(new List<DeclarationBinding>()))
            .Callback(() => Assert.Equal(3, order++));

        GetStrategy().Parse();
        blockDeclarationParser.Verify(p => p.Parse());
    }

    [Fact]
    private void ReturnsClassDeclarationOnHappyPath()
    {
        var blockDeclaration = new BlockDeclaration(new List<DeclarationBinding>());
        blockDeclarationParser.Setup(p => p.Parse()).Returns(blockDeclaration);

        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "className" });

        var declaration = GetStrategy().Parse();
        Assert.True(declaration is ClassDeclaration);

        var classDeclaration = declaration as ClassDeclaration;
        Assert.Equal("className", classDeclaration.Name);
        Assert.Equal(blockDeclaration, classDeclaration.Body);
    }

    private ParseClassDeclarationStrategy GetStrategy()
    {
        return new ParseClassDeclarationStrategy(tokenConsumer.Object, blockDeclarationParser.Object);
    }
}