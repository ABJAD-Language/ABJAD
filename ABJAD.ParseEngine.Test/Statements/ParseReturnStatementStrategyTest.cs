using System;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Statements;

public class ParseReturnStatementStrategyTest
{
    private readonly Mock<ITokenConsumer> tokenConsumerMock = new();
    private readonly Mock<ExpressionParser> expressionParser = new();

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseReturnStatementStrategy(null, expressionParser.Object));
    }

    [Fact]
    private void ThrowsExceptionIfExpressionParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseReturnStatementStrategy(tokenConsumerMock.Object, null));
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeReturn()
    {
        tokenConsumerMock.Setup(c => c.Consume(TokenType.RETURN)).Throws<Exception>();

        var strategy = GetStrategy();
        Assert.Throws<Exception>(() => strategy.Parse());
    }

    [Fact]
    private void ThrowsExceptionIfFailsToParseExpression()
    {
        expressionParser.Setup(p => p.Parse()).Throws<Exception>();

        var strategy = GetStrategy();
        Assert.Throws<Exception>(() => strategy.Parse());
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeSemicolon()
    {
        tokenConsumerMock.Setup(c => c.Consume(TokenType.SEMICOLON)).Throws<Exception>();

        var strategy = GetStrategy();
        Assert.Throws<Exception>(() => strategy.Parse());
    }

    [Fact]
    private void ReturnsReturnStatementOnHappyPath()
    {
        var expressionMock = new Mock<Expression>();
        expressionParser.Setup(p => p.Parse()).Returns(expressionMock.Object);

        var statement = GetStrategy().Parse();
        Assert.True(statement is ReturnStatement);
        var returnStatement = statement as ReturnStatement;
        Assert.Equal(expressionMock.Object, returnStatement.Target);
    }

    private ParseReturnStatementStrategy GetStrategy()
    {
        return new ParseReturnStatementStrategy(tokenConsumerMock.Object, expressionParser.Object);
    }
}