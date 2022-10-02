using System;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Expressions;

public class MethodCallArgumentsParserTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();
    private readonly Mock<ExpressionParser> expressionParser = new();

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new MethodCallArgumentsParser(null, expressionParser.Object));
    }

    [Fact]
    private void ThrowsExceptionIfExpressionParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new MethodCallArgumentsParser(tokenConsumer.Object, null));
    }

    [Fact]
    private void ReturnsEmptyListWhenNoArgumentsFound()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.CLOSE_PAREN)).Returns(true);
        Assert.Empty(GetParser().Parse());
    }

    [Fact]
    private void ReturnsListOfOneArgumentWhenExists()
    {
        tokenConsumer.SetupSequence(c => c.CanConsume(TokenType.CLOSE_PAREN)).Returns(false).Returns(true);
        var expression = new Mock<Expression>();
        expressionParser.Setup(p => p.Parse()).Returns(expression.Object);

        var arguments = GetParser().Parse();
        Assert.Equal(1, arguments.Count);
        Assert.Equal(expression.Object, arguments[0]);
    }

    [Fact]
    private void ReturnsListOfTwoArgumentWhenExist()
    {
        tokenConsumer.SetupSequence(c => c.CanConsume(TokenType.CLOSE_PAREN)).Returns(false).Returns(false)
            .Returns(true);
        tokenConsumer.SetupSequence(c => c.CanConsume(TokenType.COMMA)).Returns(true).Returns(false);
        var expression1 = new Mock<Expression>();
        var expression2 = new Mock<Expression>();
        expressionParser.SetupSequence(p => p.Parse()).Returns(expression1.Object).Returns(expression2.Object);

        var arguments = GetParser().Parse();
        Assert.Equal(2, arguments.Count);
        Assert.Equal(expression1.Object, arguments[0]);
        Assert.Equal(expression2.Object, arguments[1]);
    }

    private MethodCallArgumentsParser GetParser()
    {
        return new MethodCallArgumentsParser(tokenConsumer.Object, expressionParser.Object);
    }
}