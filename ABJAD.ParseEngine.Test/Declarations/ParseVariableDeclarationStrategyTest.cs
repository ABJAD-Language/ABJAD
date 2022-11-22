using System;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Types;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Declarations;

public class ParseVariableDeclarationStrategyTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();
    private readonly Mock<ExpressionParser> expressionParser = new();
    private readonly Mock<ITypeConsumer> typeConsumer = new();
    private readonly Mock<Token> token = new();

    public ParseVariableDeclarationStrategyTest()
    {
        typeConsumer.Setup(c => c.Consume()).Returns(DataType.String());
        tokenConsumer.Setup(c => c.Consume(It.IsAny<TokenType>())).Returns(token.Object);
    }

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseVariableDeclarationStrategy(null, expressionParser.Object, typeConsumer.Object));
    }

    [Fact]
    private void ThrowsExceptionIfExpressionParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseVariableDeclarationStrategy(tokenConsumer.Object, null, typeConsumer.Object));
    }

    [Fact]
    private void ThrowsExceptionIfTypeConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseVariableDeclarationStrategy(tokenConsumer.Object, expressionParser.Object, null));
    }

    [Fact]
    private void ConsumesVarToken()
    {
        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.VAR));
    }

    [Fact]
    private void ConsumesVariableType()
    {
        GetStrategy().Parse();
        typeConsumer.Verify(c => c.Consume());
    }

    [Fact]
    private void ShouldConsumeVariableNameId()
    {
        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.ID));
    }

    [Fact]
    private void ReturnsVariableDeclarationWithoutValueIfNotProvided()
    {
        typeConsumer.Setup(c => c.Consume()).Returns(DataType.Custom("type"));
        tokenConsumer.Setup(c => c.CanConsume(TokenType.EQUAL)).Returns(false);
        tokenConsumer.Setup(c => c.Consume(TokenType.STRING)).Returns(new Token { Type = TokenType.STRING });
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "id" });

        var declaration = GetStrategy().Parse();
        Assert.True(declaration is VariableDeclaration);

        var variableDeclaration = declaration as VariableDeclaration;
        Assert.Equal("type", variableDeclaration.Type);
        Assert.Equal("id", variableDeclaration.Name);
        Assert.Null(variableDeclaration.Value);
        
        tokenConsumer.Verify(c => c.Consume(TokenType.SEMICOLON));
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
        typeConsumer.Setup(c => c.Consume()).Returns(DataType.Custom("type"));
        tokenConsumer.Setup(c => c.CanConsume(TokenType.EQUAL)).Returns(true);
        tokenConsumer.Setup(c => c.Consume(TokenType.STRING)).Returns(new Token { Type = TokenType.STRING });
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "id" });
        var valueExpression = new Mock<Expression>();
        expressionParser.Setup(p => p.Parse()).Returns(valueExpression.Object);

        var declaration = GetStrategy().Parse();
        Assert.True(declaration is VariableDeclaration);

        var variableDeclaration = declaration as VariableDeclaration;
        Assert.Equal("type", variableDeclaration.Type);
        Assert.Equal("id", variableDeclaration.Name);
        Assert.Equal(valueExpression.Object, variableDeclaration.Value);
    }

    private ParseVariableDeclarationStrategy GetStrategy()
    {
        return new ParseVariableDeclarationStrategy(tokenConsumer.Object, expressionParser.Object, typeConsumer.Object);
    }
}