using System;
using System.Collections.Generic;
using ABJAD.ParseEngine.Shared;
using Xunit;

namespace ABJAD.ParseEngine.Test;

public class TokenConsumerTest
{
    [Fact]
    private void ConsumesTokenCorrectlyWhenMatches()
    {
        var token = new Token {Type = TokenType.ID, Line = 10, Index = 13, Content = "ب"};
        var tokens = new List<Token> {token};
        var consumer = new TokenConsumer(tokens, 0);
        var consumedToken = consumer.Consume(TokenType.ID);
        Assert.Equal(token, consumedToken);
    }

    [Fact]
    private void ThrowsExceptionWhenProvidedEmptyListOfTokens()
    {
        Assert.Throws<ArgumentException>(() => new TokenConsumer(new List<Token>(), 0));
    }

    [Fact]
    private void ThrowsExceptionWhenProvidedNegativeIndex()
    {
        var token = new Token {Type = TokenType.ID, Line = 10, Index = 13, Content = "ب"};
        Assert.Throws<ArgumentException>(() => new TokenConsumer(new List<Token>{token}, -1));
    }

    [Fact]
    private void UpdatesIndexCorrectlyAfterConsumingToken()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID},
            new() {Type = TokenType.OPEN_PAREN, Line = 3, Index = 5, Content = "("}
        };
        var consumer = new TokenConsumer(tokens, 0);
        consumer.Consume(TokenType.ID);
        var consumedToken = consumer.Consume(TokenType.OPEN_PAREN);
        Assert.Equal(3, consumedToken.Line);
        Assert.Equal(5, consumedToken.Index);
        Assert.Equal(TokenType.OPEN_PAREN, consumedToken.Type);
        Assert.Equal("(", consumedToken.Content);
    }

    [Fact]
    private void ThrowsExceptionWhenIndexOutOfRange()
    {
        var token = new Token {Type = TokenType.ID, Line = 10, Index = 13, Content = "ب"};
        var consumer = new TokenConsumer(new List<Token>{token}, 1);
        var exception = Assert.Throws<ExpectedTokenNotFoundException>(() => consumer.Consume(TokenType.OPEN_PAREN));
        Assert.Equal(10, exception.Line);
        Assert.Equal(14, exception.Index);
        Assert.Equal("Expected token of type 'OPEN_PAREN' was not found at line 10:14", exception.EnglishMessage);
        Assert.Equal("رمز متوقع من نوع 'OPEN_PAREN' لم يوجد على السطر 10:14", exception.ArabicMessage);
    }

    [Fact]
    private void SkipsWhiteSpaces()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.OPEN_PAREN, Line = 3, Index = 5, Content = "("}
        };
        var consumer = new TokenConsumer(tokens, 1);
        var consumedToken = consumer.Consume(TokenType.OPEN_PAREN);
        Assert.Equal(3, consumedToken.Line);
        Assert.Equal(5, consumedToken.Index);
        Assert.Equal(TokenType.OPEN_PAREN, consumedToken.Type);
        Assert.Equal("(", consumedToken.Content);
    }

    [Fact]
    private void UpdatesIndexCorrectlyAfterSkippingWhiteSpaces()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.ID},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.WHITE_SPACE},
            new() {Type = TokenType.OPEN_PAREN, Line = 3, Index = 5, Content = "("},
            new() {Type = TokenType.CLOSE_PAREN, Line = 3, Index = 6, Content = ")"}
        };
        var consumer = new TokenConsumer(tokens, 1);
        consumer.Consume(TokenType.OPEN_PAREN);
        var consumedToken = consumer.Consume(TokenType.CLOSE_PAREN);
        Assert.Equal(3, consumedToken.Line);
        Assert.Equal(6, consumedToken.Index);
        Assert.Equal(TokenType.CLOSE_PAREN, consumedToken.Type);
        Assert.Equal(")", consumedToken.Content);
    }

    [Fact]
    private void ThrowsExceptionIfHeadTokenDidntMatch()
    {
        var tokens = new List<Token>
        {
            new() {Type = TokenType.CLOSE_PAREN, Line = 3, Index = 6, Content = ")"}
        };
        var consumer = new TokenConsumer(tokens, 0);
        var exception = Assert.Throws<ExpectedTokenNotFoundException>(() => consumer.Consume(TokenType.ID));
        Assert.Equal(3, exception.Line);
        Assert.Equal(6, exception.Index);
        Assert.Equal("Expected token of type 'ID' was not found at line 3:6", exception.EnglishMessage);
        Assert.Equal("رمز متوقع من نوع 'ID' لم يوجد على السطر 3:6", exception.ArabicMessage);
    }
}