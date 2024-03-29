using ABJAD.Parser.Domain;
using ABJAD.Parser.Domain.Expressions;
using ABJAD.Parser.Domain.Shared;
using ABJAD.Parser.Domain.Statements;
using Moq;

namespace ABJAD.Test.Parser.Domain.Statements;

public class ParseExpressionStatementStrategyTest
{
    private readonly Mock<ITokenConsumer> tokenConsumerMock = new();
    private readonly Mock<ExpressionParser> expressionParserMock = new();

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseExpressionStatementStrategy(null, expressionParserMock.Object));
    }

    [Fact]
    private void ThrowsExceptionIfExpressionParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseExpressionStatementStrategy(tokenConsumerMock.Object, null));
    }

    [Fact]
    private void ThrowsExceptionIfFailsToParseExpression()
    {
        expressionParserMock.Setup(p => p.Parse()).Throws<Exception>();
        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeSemicolon()
    {
        tokenConsumerMock.Setup(c => c.Consume(TokenType.SEMICOLON)).Throws<Exception>();
        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ReturnsExpressionStatementOnHappyPath()
    {
        var expressionMock = new Mock<Expression>();
        expressionParserMock.Setup(p => p.Parse()).Returns(expressionMock.Object);

        var statement = GetStrategy().Parse();
        Assert.True(statement is ExpressionStatement);
        var expressionStatement = statement as ExpressionStatement;
        Assert.Equal(expressionMock.Object, expressionStatement.Target);
    }

    private ParseExpressionStatementStrategy GetStrategy()
    {
        return new ParseExpressionStatementStrategy(tokenConsumerMock.Object, expressionParserMock.Object);
    }
}