using ABJAD.Parser.Domain;
using ABJAD.Parser.Domain.Shared;
using ABJAD.Parser.Domain.Statements;
using Moq;

namespace ABJAD.Test.Parser.Domain.Statements;

public class StatementStrategyFactoryTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();

    [Fact]
    private void InstantiationFailsIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new StatementStrategyFactory(null));
    }

    [Fact]
    private void ReturnsParsePrintStatementStrategyWhenHeadTokenIsPrint()
    {
        SetHeadTokenType(TokenType.PRINT);
        Assert.IsType<ParsePrintStatementStrategy>(GetFactory().Get());
    }

    [Fact]
    private void ReturnsParseReturnStatementStrategyWhenHeadTokenIsReturn()
    {
        SetHeadTokenType(TokenType.RETURN);
        Assert.IsType<ParseReturnStatementStrategy>(GetFactory().Get());
    }

    [Fact]
    private void ReturnsParseBlockStatementStrategyWhenHeadTokenIsOpenBrace()
    {
        SetHeadTokenType(TokenType.OPEN_BRACE);
        Assert.IsType<ParseBlockStatementStrategy>(GetFactory().Get());
    }

    [Fact]
    private void ReturnsParseForStatementStrategyWhenHeadTokenIsFor()
    {
        SetHeadTokenType(TokenType.FOR);
        Assert.IsType<ParseForStatementStrategy>(GetFactory().Get());
    }

    [Fact]
    private void ReturnsParseWhileStatementStrategyWhenHeadTokenIsWhile()
    {
        SetHeadTokenType(TokenType.WHILE);
        Assert.IsType<ParseWhileStatementStrategy>(GetFactory().Get());
    }

    [Fact]
    private void ReturnsParseIfElseStatementStrategyWhenHeadTokenIsIf()
    {
        SetHeadTokenType(TokenType.IF);
        Assert.IsType<ParseIfElseStatementStrategy>(GetFactory().Get());
    }

    [Fact]
    private void ReturnsParseAssignmentStatementStrategyWhenHeadTokenIsIdFollowedByEqual()
    {
        SetHeadTokenType(TokenType.ID);
        SetLookAheadTokenType(TokenType.EQUAL);
        Assert.IsType<ParseAssignmentStatementStrategy>(GetFactory().Get());
    }

    [Fact]
    private void ReturnsParseExpressionStatementStrategyWhenHeadTokenIsIdAndNotFollowedByEqual()
    {
        SetHeadTokenType(TokenType.ID);
        SetLookAheadTokenType(TokenType.PLUS_PLUS);
        Assert.IsType<ParseExpressionStatementStrategy>(GetFactory().Get());
    }

    [Fact]
    private void ReturnsParseExpressionStatementStrategyWhenHeadTokenIsNotOneOfAbove()
    {
        SetHeadTokenType(TokenType.STRING_CONST);
        Assert.IsType<ParseExpressionStatementStrategy>(GetFactory().Get());
    }

    private void SetHeadTokenType(TokenType tokenType)
    {
        tokenConsumer.Setup(c => c.Peek()).Returns(new Token { Type = tokenType });
    }

    private void SetLookAheadTokenType(TokenType tokenType)
    {
        tokenConsumer.Setup(c => c.LookAhead(1)).Returns(new Token { Type = tokenType });
    }

    private StatementStrategyFactory GetFactory() => new(tokenConsumer.Object);
}