using System;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Statements;

public class ParsePrintStatementStrategyTest
{
    private Mock<ITokenConsumer> consumerMock = new();
    private readonly Mock<ExpressionParser> expressionParser = new();

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParsePrintStatementStrategy(null, expressionParser.Object));
    }

    [Fact]
    private void ThrowsExceptionIfExpressionParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParsePrintStatementStrategy(consumerMock.Object, null));
    }

    [Fact]
    private void ThrowsExceptionWhenFailsToConsumePrintToken()
    {
        consumerMock.Setup(c => c.Consume(TokenType.PRINT)).Throws<Exception>();

        var strategy = GetStrategy();
        Assert.Throws<Exception>(() => strategy.Parse());
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeOpenParenthesis()
    {
        consumerMock.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Throws<Exception>();

        var strategy = GetStrategy();
        Assert.Throws<Exception>(() => strategy.Parse());
    }

    [Fact]
    private void ThrowsExceptionIfFailsToParseAnExpression()
    {
        expressionParser.Setup(parser => parser.Parse()).Throws<Exception>();

        var strategy = GetStrategy();
        Assert.Throws<Exception>(() => strategy.Parse());
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeCloseParenthesis()
    {
        consumerMock.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Throws<Exception>();

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
    private void ReturnsPrintStatementOnHappyPath()
    {
        var expression = new Mock<Expression>();
        expressionParser.Setup(p => p.Parse()).Returns(() => expression.Object);

        var strategy = GetStrategy();
        var statement = strategy.Parse();

        Assert.True(statement is PrintStatement);
        Assert.Equal(expression.Object, (statement as PrintStatement).Target);
    }

    private ParsePrintStatementStrategy GetStrategy()
    {
        return new ParsePrintStatementStrategy(consumerMock.Object, expressionParser.Object);
    }
}