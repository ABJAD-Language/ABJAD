using ABJAD.Parser.Domain;
using ABJAD.Parser.Domain.Expressions;
using ABJAD.Parser.Domain.Shared;
using ABJAD.Parser.Domain.Statements;
using Moq;

namespace ABJAD.Test.Parser.Domain.Statements;

public class ParseWhileStatementStrategyTest
{
    private readonly Mock<ITokenConsumer> tokenConsumerMock = new();
    private readonly Mock<ExpressionParser> expressionParserMock = new();
    private readonly Mock<ParseStatementStrategy> parseStatementStrategyMock = new();
    private readonly Mock<IStatementStrategyFactory> statementStrategyFactoryMock = new();

    public ParseWhileStatementStrategyTest()
    {
        statementStrategyFactoryMock.Setup(f => f.Get()).Returns(parseStatementStrategyMock.Object);
    }

    [Fact]
    private void ThrowsExceptionTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseWhileStatementStrategy(null, expressionParserMock.Object, statementStrategyFactoryMock.Object));
    }

    [Fact]
    private void ThrowsExceptionIfExpressionParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseWhileStatementStrategy(tokenConsumerMock.Object, null, statementStrategyFactoryMock.Object));
    }

    [Fact]
    private void ThrowsExceptionIfParseStatementStrategyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseWhileStatementStrategy(tokenConsumerMock.Object, expressionParserMock.Object, null));
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeWhile()
    {
        tokenConsumerMock.Setup(c => c.Consume(TokenType.WHILE)).Throws<Exception>();

        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ConsumesWhileOnHappyPath()
    {
        GetStrategy().Parse();
        tokenConsumerMock.Verify(c => c.Consume(TokenType.WHILE));
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumerOpenParenthesis()
    {
        tokenConsumerMock.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Throws<Exception>();

        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ConsumesOpenParenthesisInOrderOnHappyPath()
    {
        var order = 1;
        tokenConsumerMock.Setup(c => c.Consume(TokenType.WHILE)).Callback(() => Assert.Equal(1, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));

        GetStrategy().Parse();
    }

    [Fact]
    private void ThrowsExceptionIfFailsToParseExpression()
    {
        expressionParserMock.Setup(p => p.Parse()).Throws<Exception>();
        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ParsesExpressionInOrderOnHappyPath()
    {
        var order = 1;
        tokenConsumerMock.Setup(c => c.Consume(TokenType.WHILE)).Callback(() => Assert.Equal(1, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        expressionParserMock.Setup(p => p.Parse()).Callback(() => Assert.Equal(3, order++));

        GetStrategy().Parse();
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeCloseParenthesis()
    {
        tokenConsumerMock.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Throws<Exception>();

        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ConsumesCloseParenthesisInOrderOnHappyPath()
    {
        var order = 1;
        tokenConsumerMock.Setup(c => c.Consume(TokenType.WHILE)).Callback(() => Assert.Equal(1, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        expressionParserMock.Setup(p => p.Parse()).Callback(() => Assert.Equal(3, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Callback(() => Assert.Equal(4, order++));

        GetStrategy().Parse();
    }

    [Fact]
    private void ThrowsExceptionIfFailsToParseStatement()
    {
        parseStatementStrategyMock.Setup(s => s.Parse()).Throws<Exception>();

        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ParsesStatementInOrderOnHappyPath()
    {
        var order = 1;
        tokenConsumerMock.Setup(c => c.Consume(TokenType.WHILE)).Callback(() => Assert.Equal(1, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        expressionParserMock.Setup(p => p.Parse()).Callback(() => Assert.Equal(3, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Callback(() => Assert.Equal(4, order++));
        parseStatementStrategyMock.Setup(c => c.Parse()).Callback(() => Assert.Equal(5, order++));

        GetStrategy().Parse();
    }

    [Fact]
    private void ReturnsWhileStatementOnHappyPath()
    {
        var expressionMock = new Mock<Expression>();
        var statementMock = new Mock<Statement>();

        expressionParserMock.Setup(p => p.Parse()).Returns(expressionMock.Object);
        parseStatementStrategyMock.Setup(s => s.Parse()).Returns(statementMock.Object);

        var statement = GetStrategy().Parse();
        Assert.True(statement is WhileStatement);

        var whileStatement = statement as WhileStatement;
        Assert.Equal(expressionMock.Object, whileStatement.Condition);
        Assert.Equal(statementMock.Object, whileStatement.Body);
    }

    private ParseWhileStatementStrategy GetStrategy()
    {
        return new ParseWhileStatementStrategy(tokenConsumerMock.Object, expressionParserMock.Object,
            statementStrategyFactoryMock.Object);
    }
}