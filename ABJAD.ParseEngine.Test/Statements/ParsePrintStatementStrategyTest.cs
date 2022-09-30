using System;
using System.Collections.Generic;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Statements;

public class ParsePrintStatementStrategyTest
{
    private readonly Mock<ExpressionParser> expressionParser = new();
    private readonly Mock<ExpressionParserFactory> expressionParserFactory = new();

    public ParsePrintStatementStrategyTest()
    {
        expressionParserFactory.Setup(factory => factory.CreateInstance(It.IsAny<ITokenConsumer>()))
            .Returns(expressionParser.Object);
    }

    [Fact]
    private void ThrowsExceptionWhenFailsToConsumePrintToken()
    {
        var tokens = new List<Token>
        {
            new() { Type = TokenType.ID }
        };
        var strategy = new ParsePrintStatementStrategy(new TokenConsumer(tokens, 0), expressionParserFactory.Object);
        Assert.Throws<ExpectedTokenNotFoundException>(() => strategy.Parse());
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeOpenParenthesis()
    {
        var tokens = new List<Token>
        {
            new() { Type = TokenType.PRINT }
        };
        var strategy = new ParsePrintStatementStrategy(new TokenConsumer(tokens, 0), expressionParserFactory.Object);
        Assert.Throws<ArgumentOutOfRangeException>(() => strategy.Parse());
    }

    [Fact]
    private void ThrowsExceptionIfFailsToParseAnExpression()
    {
        expressionParser.Setup(parser => parser.Parse()).Throws(new FailedToParseExpressionException(1, 1));

        var tokens = new List<Token>
        {
            new() { Type = TokenType.PRINT },
            new() { Type = TokenType.OPEN_PAREN },
            new() { Type = TokenType.ID },
        };
        var strategy = new ParsePrintStatementStrategy(new TokenConsumer(tokens, 0), expressionParserFactory.Object);
        Assert.Throws<FailedToParseExpressionException>(() => strategy.Parse());
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeCloseParenthesis()
    {
        var tokens = new List<Token>
        {
            new() { Type = TokenType.PRINT },
            new() { Type = TokenType.OPEN_PAREN },
            new() { Type = TokenType.ID },
        };

        var tokenConsumer = new TokenConsumer(tokens, 0);
        expressionParser.Setup(p => p.Parse()).Returns(() =>
        {
            tokenConsumer.Consume();
            return default;
        });

        var strategy = new ParsePrintStatementStrategy(tokenConsumer, expressionParserFactory.Object);
        Assert.Throws<ArgumentOutOfRangeException>(() => strategy.Parse());
    }

    [Fact]
    private void ThrowsExceptionIfFailsToConsumeSemicolon()
    {
        var tokens = new List<Token>
        {
            new() { Type = TokenType.PRINT },
            new() { Type = TokenType.OPEN_PAREN },
            new() { Type = TokenType.ID },
            new() { Type = TokenType.CLOSE_PAREN },
        };
        var tokenConsumer = new TokenConsumer(tokens, 0);

        expressionParser.Setup(p => p.Parse()).Returns(() =>
        {
            tokenConsumer.Consume();
            return default;
        });

        var strategy = new ParsePrintStatementStrategy(tokenConsumer, expressionParserFactory.Object);
        Assert.Throws<ArgumentOutOfRangeException>(() => strategy.Parse());
    }

    [Fact]
    private void ReturnsPrintStatementOnHappyPath()
    {
        var tokens = new List<Token>
        {
            new() { Type = TokenType.PRINT },
            new() { Type = TokenType.OPEN_PAREN },
            new() { Type = TokenType.ID },
            new() { Type = TokenType.CLOSE_PAREN },
            new() { Type = TokenType.SEMICOLON },
        };
        var tokenConsumer = new TokenConsumer(tokens, 0);

        var expression = new Mock<Expression>();
        expressionParser.Setup(p => p.Parse()).Returns(() =>
        {
            tokenConsumer.Consume();
            return expression.Object;
        });

        var strategy = new ParsePrintStatementStrategy(tokenConsumer, expressionParserFactory.Object);
        var statement = strategy.Parse();

        Assert.True(statement is PrintStatement);
        Assert.Equal(expression.Object, (statement as PrintStatement).Target);
    }
}