using System;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Statements;

public class ParseAssignmentStatementStrategyTest
{
    private Mock<ITokenConsumer> consumerMock = new();
    private Mock<ExpressionParser> expressionParserMock = new();

    public ParseAssignmentStatementStrategyTest()
    {
        consumerMock.Setup(c => c.Consume(TokenType.ID)).Returns(new Token() { Content = "id" });
    }

    [Fact]
    private void FailsIfConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseAssignmentStatementStrategy(null, expressionParserMock.Object));
    }

    [Fact]
    private void FailsIfExpressionParserFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseAssignmentStatementStrategy(consumerMock.Object, null));
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeAnId()
    {
        consumerMock.Setup(c => c.Consume(TokenType.ID)).Throws<Exception>();

        var strategy = GetStrategy();
        Assert.Throws<Exception>(() => strategy.Parse());
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeEqual()
    {
        consumerMock.Setup(c => c.Consume(TokenType.EQUAL)).Throws<Exception>();

        var strategy = GetStrategy();
        Assert.Throws<Exception>(() => strategy.Parse());
    }

    [Fact]
    private void ThrowsExceptionIfExpressionParserFailsToParse()
    {
        expressionParserMock.Setup(p => p.Parse()).Throws<Exception>();

        var strategy = GetStrategy();
        Assert.Throws<Exception>(() => strategy.Parse());
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeSemicolon()
    {
        consumerMock.Setup(c => c.Consume(TokenType.SEMICOLON)).Throws<Exception>();

        var strategy = GetStrategy();
        Assert.Throws<Exception>(() => strategy.Parse());
    }

    [Fact]
    private void ReturnsAssignmentStatementOnHappyPath()
    {
        var expressionMock = new Mock<Expression>();
        expressionParserMock.Setup(p => p.Parse()).Returns(expressionMock.Object);

        var statement = GetStrategy().Parse();
        Assert.True(statement is AssignmentStatement);
        var assignmentStatement = statement as AssignmentStatement;
        Assert.Equal("id", assignmentStatement.Target);
        Assert.Equal(expressionMock.Object, assignmentStatement.Value);
    }

    private ParseAssignmentStatementStrategy GetStrategy()
    {
        return new ParseAssignmentStatementStrategy(consumerMock.Object, expressionParserMock.Object);
    }
}