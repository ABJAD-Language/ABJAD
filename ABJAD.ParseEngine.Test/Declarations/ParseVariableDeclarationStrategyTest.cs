using System;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Declarations;

public class ParseVariableDeclarationStrategyTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();
    private readonly Mock<ExpressionParser> expressionParser = new();
    private readonly Mock<Token> token = new();

    public ParseVariableDeclarationStrategyTest()
    {
        tokenConsumer.Setup(c => c.Consume(It.IsAny<TokenType>())).Returns(token.Object);
    }

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseVariableDeclarationStrategy(null, expressionParser.Object));
    }

    [Fact]
    private void ThrowsExceptionIfExpressionParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseVariableDeclarationStrategy(tokenConsumer.Object, null));
    }

    [Fact]
    private void ConsumesVarToken()
    {
        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.VAR));
    }

    [Fact]
    private void ConsumesStringTokenWhenTypeIsString()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.STRING)).Returns(true);

        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.STRING));
    }

    [Fact]
    private void ConsumesNumberWhenTypeIsNumber()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.NUMBER)).Returns(true);

        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.NUMBER));
    }

    [Fact]
    private void ConsumesBoolWhenTypeIsBool()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.BOOL)).Returns(true);

        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.BOOL));
    }

    [Fact]
    private void ConsumesIdWhenTypeIsCustom()
    {
        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.ID));
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeIdWhenTypeIsId()
    {
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Throws<Exception>();
        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ShouldConsumeVariableNameId()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.STRING)).Returns(true);

        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.STRING));
        tokenConsumer.Verify(c => c.Consume(TokenType.ID));
    }

    [Fact]
    private void ReturnsVariableDeclarationWithoutValueIfNotProvided()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.STRING)).Returns(true);
        tokenConsumer.Setup(c => c.CanConsume(TokenType.EQUAL)).Returns(false);
        tokenConsumer.Setup(c => c.Consume(TokenType.STRING)).Returns(new Token { Type = TokenType.STRING });
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "id" });

        var declaration = GetStrategy().Parse();
        Assert.True(declaration is VariableDeclaration);

        var variableDeclaration = declaration as VariableDeclaration;
        Assert.Equal(TokenType.STRING.ToString(), variableDeclaration.Type);
        Assert.Equal("id", variableDeclaration.Name);
        Assert.Null(variableDeclaration.Value);
    }

    [Fact]
    private void ConsumesEqualWhenFound()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.EQUAL)).Returns(true);
        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.EQUAL));
    }

    [Fact]
    private void ThrowsExceptionIfFailedToParseExpressionWhenFound()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.EQUAL)).Returns(true);
        expressionParser.Setup(p => p.Parse()).Throws<Exception>();

        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ConsumesSemicolonAtTheEnd()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.EQUAL)).Returns(true);

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
        tokenConsumer.Setup(c => c.CanConsume(TokenType.STRING)).Returns(true);
        tokenConsumer.Setup(c => c.CanConsume(TokenType.EQUAL)).Returns(true);
        tokenConsumer.Setup(c => c.Consume(TokenType.STRING)).Returns(new Token { Type = TokenType.STRING });
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "id" });
        var valueExpression = new Mock<Expression>();
        expressionParser.Setup(p => p.Parse()).Returns(valueExpression.Object);

        var declaration = GetStrategy().Parse();
        Assert.True(declaration is VariableDeclaration);

        var variableDeclaration = declaration as VariableDeclaration;
        Assert.Equal(TokenType.STRING.ToString(), variableDeclaration.Type);
        Assert.Equal("id", variableDeclaration.Name);
        Assert.Equal(valueExpression.Object, variableDeclaration.Value);
    }

    private ParseVariableDeclarationStrategy GetStrategy()
    {
        return new ParseVariableDeclarationStrategy(tokenConsumer.Object, expressionParser.Object);
    }
}