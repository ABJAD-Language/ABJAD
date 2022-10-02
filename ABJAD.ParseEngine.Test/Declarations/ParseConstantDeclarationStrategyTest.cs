using System;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Declarations;

public class ParseConstantDeclarationStrategyTest
{
    private readonly Mock<ExpressionParser> expressionParser = new();
    private readonly Mock<ITokenConsumer> tokenConsumer = new();
    private readonly Mock<ITypeConsumer> typeConsumer = new();
    private readonly Mock<Token> token = new();

    public ParseConstantDeclarationStrategyTest()
    {
        tokenConsumer.Setup(c => c.Consume(It.IsAny<TokenType>())).Returns(token.Object);
    }

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseConstantDeclarationStrategy(null, expressionParser.Object, typeConsumer.Object));
    }

    [Fact]
    private void ThrowsExceptionIfExpressionParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseConstantDeclarationStrategy(tokenConsumer.Object, null, typeConsumer.Object));
    }

    [Fact]
    private void ThrowsExceptionIfTypeConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseConstantDeclarationStrategy(tokenConsumer.Object, expressionParser.Object, null));
    }

    [Fact]
    private void ConsumesConstToken()
    {
        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.CONST));
    }

    [Fact]
    private void ConsumesConstantType()
    {
        GetStrategy().Parse();
        typeConsumer.Verify(c => c.Consume());
    }

    [Fact]
    private void ConsumesIdWhenTypeIsCustom()
    {
        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.ID));
    }

    [Fact]
    private void ShouldConsumeConstantNameId()
    {
        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.ID));
    }

    [Fact]
    private void ConsumesEqualToken()
    {
        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.EQUAL));
    }

    [Fact]
    private void ThrowsExceptionIfFailedToParseExpression()
    {
        expressionParser.Setup(p => p.Parse()).Throws<Exception>();

        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ConsumesSemicolonAtTheEnd()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.EQUAL)).Callback(() => Assert.Equal(1, order++));
        expressionParser.Setup(c => c.Parse()).Callback(() => Assert.Equal(2, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.SEMICOLON)).Callback(() => Assert.Equal(3, order++));

        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.SEMICOLON));
    }

    [Fact]
    private void ReturnsVariableDeclarationWithValueExpressionWhenFound()
    {
        typeConsumer.Setup(c => c.Consume()).Returns("type");
        tokenConsumer.Setup(c => c.Consume(TokenType.STRING)).Returns(new Token { Type = TokenType.STRING });
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "id" });
        var valueExpression = new Mock<Expression>();
        expressionParser.Setup(p => p.Parse()).Returns(valueExpression.Object);

        var declaration = GetStrategy().Parse();
        Assert.True(declaration is ConstantDeclaration);

        var constantDeclaration = declaration as ConstantDeclaration;
        Assert.Equal("type", constantDeclaration.Type);
        Assert.Equal("id", constantDeclaration.Name);
        Assert.Equal(valueExpression.Object, constantDeclaration.Value);
    }

    private ParseConstantDeclarationStrategy GetStrategy()
    {
        return new ParseConstantDeclarationStrategy(tokenConsumer.Object, expressionParser.Object, typeConsumer.Object);
    }
}