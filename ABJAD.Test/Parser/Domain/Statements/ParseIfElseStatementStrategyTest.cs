using ABJAD.Parser.Domain;
using ABJAD.Parser.Domain.Expressions;
using ABJAD.Parser.Domain.Shared;
using ABJAD.Parser.Domain.Statements;
using FluentAssertions;
using Moq;

namespace ABJAD.Test.Parser.Domain.Statements;

public class ParseIfElseStatementStrategyTest
{
    private readonly Mock<ITokenConsumer> tokenConsumerMock = new();
    private readonly Mock<ExpressionParser> expressionParserMock = new();
    private readonly Mock<ParseStatementStrategy> statementParserMock = new();
    private readonly Mock<IStatementStrategyFactory> statementStrategyFactoryMock = new();

    public ParseIfElseStatementStrategyTest()
    {
        statementStrategyFactoryMock.Setup(f => f.Get()).Returns(statementParserMock.Object);
    }

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseIfElseStatementStrategy(null, expressionParserMock.Object, statementStrategyFactoryMock.Object));
    }

    [Fact]
    private void ThrowsExceptionIfExpressionParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseIfElseStatementStrategy(tokenConsumerMock.Object, null, statementStrategyFactoryMock.Object));
    }

    [Fact]
    private void ThrowsExceptionIfStatementStrategyFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseIfElseStatementStrategy(tokenConsumerMock.Object, expressionParserMock.Object, null));
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeIf()
    {
        tokenConsumerMock.Setup(c => c.Consume(TokenType.IF)).Throws<Exception>();
        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ConsumesIfOnHappyPath()
    {
        GetStrategy().Parse();
        tokenConsumerMock.Verify(c => c.Consume(TokenType.IF));
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeOpenParenthesis()
    {
        tokenConsumerMock.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Throws<Exception>();
        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ConsumesOpenParenthesisInOrderOnHappyPath()
    {
        var order = 1;
        tokenConsumerMock.Setup(c => c.Consume(TokenType.IF)).Callback(() => Assert.Equal(1, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));

        GetStrategy().Parse();
    }

    [Fact]
    private void ThrowsExceptionIfFailsToParseConditionExpression()
    {
        expressionParserMock.Setup(p => p.Parse()).Throws<Exception>();
        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ParsesConditionExpressionInOrderOnHappyPath()
    {
        var order = 1;
        tokenConsumerMock.Setup(c => c.Consume(TokenType.IF)).Callback(() => Assert.Equal(1, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        expressionParserMock.Setup(c => c.Parse()).Callback(() => Assert.Equal(3, order++));

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
        tokenConsumerMock.Setup(c => c.Consume(TokenType.IF)).Callback(() => Assert.Equal(1, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        expressionParserMock.Setup(c => c.Parse()).Callback(() => Assert.Equal(3, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Callback(() => Assert.Equal(4, order++));

        GetStrategy().Parse();
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeBodyStatement()
    {
        statementParserMock.Setup(c => c.Parse()).Throws<Exception>();
        Assert.Throws<Exception>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ParsesBodyStatementInOrderOnHappyPath()
    {
        var order = 1;
        tokenConsumerMock.Setup(c => c.Consume(TokenType.IF)).Callback(() => Assert.Equal(1, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        expressionParserMock.Setup(c => c.Parse()).Callback(() => Assert.Equal(3, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Callback(() => Assert.Equal(4, order++));
        statementParserMock.Setup(c => c.Parse()).Callback(() => Assert.Equal(5, order++));

        GetStrategy().Parse();
    }

    [Fact]
    private void ShouldReturnIfStatementIfNoElseWasFound()
    {
        var expressionMock = new Mock<Expression>();
        var statementMock = new Mock<Statement>();

        expressionParserMock.Setup(p => p.Parse()).Returns(expressionMock.Object);
        statementParserMock.Setup(p => p.Parse()).Returns(statementMock.Object);

        tokenConsumerMock.Setup(c => c.CanConsume(TokenType.ELSE)).Returns(false);

        var statement = GetStrategy().Parse();
        Assert.True(statement is IfStatement);

        var ifStatement = statement as IfStatement;
        Assert.Equal(expressionMock.Object, ifStatement.Condition);
        Assert.Equal(statementMock.Object, ifStatement.Body);
    }

    [Fact]
    private void ShouldParseElseBodyStatementInOrderIfNoElseIfWasFound()
    {
        tokenConsumerMock.Setup(c => c.CanConsume(TokenType.ELSE)).Returns(true);
        tokenConsumerMock.Setup(c => c.CanConsume(TokenType.IF)).Returns(false);

        var order = 1;
        tokenConsumerMock.Setup(c => c.Consume(TokenType.IF)).Callback(() => Assert.Equal(1, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        expressionParserMock.Setup(c => c.Parse()).Callback(() => Assert.Equal(3, order++));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Callback(() => Assert.Equal(4, order++));
        statementParserMock.Setup(c => c.Parse()).Callback(() => Assert.True(order++ is 5 or 7, order.ToString()));
        tokenConsumerMock.Setup(c => c.Consume(TokenType.ELSE)).Callback(() => Assert.Equal(6, order++));

        GetStrategy().Parse();
    }

    [Fact]
    private void ShouldReturnIfElseStatementWithEmptyMiddleIfStatementsIfNoElseIfWasFound()
    {
        var statementIndex = 0;
        var expressionMock = new Mock<Expression>();
        var ifStatementMock = new Mock<Statement>();
        var elseStatementMock = new Mock<Statement>();

        expressionParserMock.Setup(p => p.Parse()).Returns(expressionMock.Object);
        statementParserMock.Setup(p => p.Parse())
            .Returns(() => statementIndex++ == 0 ? ifStatementMock.Object : elseStatementMock.Object);

        tokenConsumerMock.Setup(c => c.CanConsume(TokenType.ELSE)).Returns(true);
        tokenConsumerMock.Setup(c => c.CanConsume(TokenType.IF)).Returns(false);

        var statement = GetStrategy().Parse();

        var ifElseStatement = statement as IfElseStatement;
        Assert.NotNull(ifElseStatement);

        Assert.Equal(expressionMock.Object, ifElseStatement.MainIfStatement.Condition);
        Assert.Equal(ifStatementMock.Object, ifElseStatement.MainIfStatement.Body);
        Assert.Empty(ifElseStatement.OtherIfStatements);
        Assert.Equal(elseStatementMock.Object, ifElseStatement.ElseBody);
    }

    [Fact]
    private void ShouldReturnIfElseStatementWithOneMiddleIfStatementWhenFound()
    {
        var statementIndex = 0;
        var conditionIndex = 0;
        var primaryConditionMock = new Mock<Expression>();
        var secondaryConditionMock = new Mock<Expression>();
        var ifStatementMock = new Mock<Statement>();
        var elseIfStatementMock = new Mock<Statement>();
        var elseStatementMock = new Mock<Statement>();

        expressionParserMock.Setup(p => p.Parse())
            .Returns(() =>
            {
                conditionIndex++;
                return conditionIndex == 1 ? primaryConditionMock.Object : secondaryConditionMock.Object;
            });

        statementParserMock.Setup(p => p.Parse())
            .Returns(() =>
            {
                statementIndex++;
                return statementIndex switch
                {
                    1 => ifStatementMock.Object,
                    2 => elseIfStatementMock.Object,
                    _ => elseStatementMock.Object
                };
            });

        tokenConsumerMock.Setup(c => c.CanConsume(TokenType.ELSE)).Returns(true);

        var numberOfMiddleIfs = 1;
        tokenConsumerMock.Setup(c => c.CanConsume(TokenType.IF)).Returns(() => numberOfMiddleIfs++ == 1);

        var statement = GetStrategy().Parse();

        var expectedStatement =
            new IfElseStatement(new IfStatement(primaryConditionMock.Object, ifStatementMock.Object),
                new[] { new IfStatement(secondaryConditionMock.Object, elseIfStatementMock.Object) },
                elseStatementMock.Object);

        expectedStatement.Should().BeEquivalentTo(statement, options => options.RespectingRuntimeTypes());
    }

    private ParseIfElseStatementStrategy GetStrategy()
    {
        return new ParseIfElseStatementStrategy(tokenConsumerMock.Object, expressionParserMock.Object,
            statementStrategyFactoryMock.Object);
    }
}