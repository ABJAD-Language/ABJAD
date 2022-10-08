using System;
using ABJAD.ParseEngine.Bindings;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using FluentAssertions;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Statements;

public class ParseForStatementStrategyTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();
    private readonly Mock<ExpressionParser> expressionParser = new();
    private readonly Mock<IBindingFactory> bindingFactory = new();
    private readonly Mock<BlockStatementParser> blockStatementParser = new();
    private readonly Mock<IStatementStrategyFactory> statementStrategyFactory = new();
    private readonly Mock<ParseStatementStrategy> statementParser = new();

    private readonly StatementBinding targetAssignmentBinding =
        new(new AssignmentStatement("target", new Mock<Expression>().Object));

    public ParseForStatementStrategyTest()
    {
        statementStrategyFactory.Setup(f => f.Get()).Returns(statementParser.Object);
        bindingFactory.Setup(f => f.Get()).Returns(targetAssignmentBinding);
    }

    [Fact]
    private void InstantiationFailsIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseForStatementStrategy(null, expressionParser.Object,
            bindingFactory.Object, statementStrategyFactory.Object));
    }

    [Fact]
    private void InstantiationFailsIfExpressionParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseForStatementStrategy(tokenConsumer.Object, null, bindingFactory.Object,
                statementStrategyFactory.Object));
    }

    [Fact]
    private void InstantiationFailsIfStatementParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseForStatementStrategy(tokenConsumer.Object,
            expressionParser.Object, null, statementStrategyFactory.Object));
    }

    [Fact]
    private void InstantiationFailsIfStatementStrategyFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseForStatementStrategy(tokenConsumer.Object, expressionParser.Object, bindingFactory.Object, null));
    }

    [Fact]
    private void ParsesFor()
    {
        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.FOR));
    }

    [Fact]
    private void ParsesOpenParenthesisAfterFor()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.FOR)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));

        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.OPEN_PAREN));
    }

    [Fact]
    private void ParsesStatementAfterOpenParenthesis()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.FOR)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        bindingFactory.Setup(f => f.Get()).Returns(targetAssignmentBinding).Callback(() => Assert.Equal(3, order++));

        GetStrategy().Parse();

        bindingFactory.Verify(f => f.Get());
    }

    [Fact]
    private void DoesNotFailIfParsedBindingIsAssignmentStatement()
    {
        bindingFactory.Setup(f => f.Get()).Returns(targetAssignmentBinding);

        var exception = Record.Exception(() => GetStrategy().Parse());
        Assert.Null(exception);
    }

    [Fact]
    private void FailsIfParsedBindingIsVariableDeclarationWithoutValue()
    {
        bindingFactory.Setup(f => f.Get())
            .Returns(new DeclarationBinding(new VariableDeclaration("type", "name", null)));

        Assert.Throws<ArgumentNullException>(() => GetStrategy().Parse());
    }

    [Fact]
    private void DoeNotFailIfParsedBindingIsVariableDeclarationWithValue()
    {
        bindingFactory.Setup(f => f.Get())
            .Returns(new DeclarationBinding(new VariableDeclaration("type", "name", new Mock<Expression>().Object)));

        var exception = Record.Exception(() => GetStrategy().Parse());
        Assert.Null(exception);
    }

    [Fact]
    private void FailsIfParsedBindingIsNeitherAssignmentNorVariableDeclarationWithValue()
    {
        bindingFactory.Setup(f => f.Get()).Returns(new Mock<Binding>().Object);

        Assert.Throws<ArgumentException>(() => GetStrategy().Parse());
    }

    [Fact]
    private void ParsesConditionExpressionStatementAfterTargetBinding()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.FOR)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        bindingFactory.Setup(f => f.Get()).Returns(targetAssignmentBinding).Callback(() => Assert.Equal(3, order++));
        statementParser.Setup(e => e.Parse()).Callback(() => Assert.True(order++ is 4 or 5));

        GetStrategy().Parse();

        statementParser.Verify(f => f.Parse());
    }

    [Fact]
    private void ParsesTargetCallbackExpressionAfterCondition()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.FOR)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        bindingFactory.Setup(f => f.Get()).Returns(targetAssignmentBinding).Callback(() => Assert.Equal(3, order++));
        statementParser.Setup(e => e.Parse()).Callback(() => Assert.True(order++ is 4 or 6));
        expressionParser.Setup(e => e.Parse()).Callback(() => Assert.Equal(5, order++));

        GetStrategy().Parse();

        expressionParser.Verify(f => f.Parse());
    }

    [Fact]
    private void ParsesCloseParenthesisAfterTargetCallbackExpression()
    {
        var order = 1;
        expressionParser.Setup(e => e.Parse()).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Callback(() => Assert.Equal(2, order++));

        GetStrategy().Parse();

        tokenConsumer.Verify(c => c.Consume(TokenType.CLOSE_PAREN));
    }

    [Fact]
    private void ParsesStatementAfterCloseParenthesis()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Callback(() => Assert.Equal(2, order++));
        statementParser.Setup(e => e.Parse()).Callback(() => Assert.True(order++ is 1 or 3));

        GetStrategy().Parse();

        statementParser.Verify(p => p.Parse());
    }

    [Fact]
    private void ReturnsForStatementOnHappyPath()
    {
        var conditionStatement = new Mock<Statement>();
        var bodyStatement = new Mock<Statement>();
        statementParser.SetupSequence(p => p.Parse())
            .Returns(conditionStatement.Object)
            .Returns(bodyStatement.Object);
        var expressionCallback = new Mock<Expression>();
        expressionParser.Setup(p => p.Parse()).Returns(expressionCallback.Object);

        var expectedStatement = new ForStatement(targetAssignmentBinding, conditionStatement.Object,
            expressionCallback.Object, bodyStatement.Object);
        var statement = GetStrategy().Parse();
        statement.Should().BeEquivalentTo(expectedStatement, options => options.RespectingRuntimeTypes());
    }

    private ParseForStatementStrategy GetStrategy()
    {
        return new ParseForStatementStrategy(tokenConsumer.Object, expressionParser.Object,
            bindingFactory.Object, statementStrategyFactory.Object);
    }
}