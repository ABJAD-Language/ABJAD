using System;
using System.Collections.Generic;
using ABJAD.ParseEngine.Shared;
using Xunit;

namespace ABJAD.ParseEngine.Test;

public class TokenConsumerTest
{
    [Fact]
    private void ThrowsExceptionWhenProvidedEmptyListOfTokens()
    {
        Assert.Throws<ArgumentException>(() => new TokenConsumer(new List<Token>(), 0));
    }

    [Fact]
    private void ThrowsExceptionWhenProvidedNegativeIndex()
    {
        var token = new Token {Type = TokenType.ID, Line = 10, Index = 13, Content = "ب"};
        Assert.Throws<ArgumentException>(() => new TokenConsumer(new List<Token> {token}, -1));
    }

    public class ConsumeTokenWithTargetTest
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
            var consumer = new TokenConsumer(new List<Token> {token}, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => consumer.Consume(TokenType.OPEN_PAREN));
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

    public class ConsumeTokenWithoutTargetTest
    {
        [Fact]
        private void ConsumesTokenCorrectly()
        {
            var token = new Token {Type = TokenType.ID, Line = 10, Index = 13, Content = "ب"};
            var tokens = new List<Token> {token};
            var consumer = new TokenConsumer(tokens, 0);
            var consumedToken = consumer.Consume();
            Assert.Equal(token, consumedToken);
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
            consumer.Consume();
            var consumedToken = consumer.Consume();
            Assert.Equal(3, consumedToken.Line);
            Assert.Equal(5, consumedToken.Index);
            Assert.Equal(TokenType.OPEN_PAREN, consumedToken.Type);
            Assert.Equal("(", consumedToken.Content);
        }

        [Fact]
        private void ThrowsExceptionWhenIndexOutOfRange()
        {
            var token = new Token {Type = TokenType.ID, Line = 10, Index = 13, Content = "ب"};
            var consumer = new TokenConsumer(new List<Token> {token}, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => consumer.Consume());
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
            var consumedToken = consumer.Consume();
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
            consumer.Consume();
            var consumedToken = consumer.Consume();
            Assert.Equal(3, consumedToken.Line);
            Assert.Equal(6, consumedToken.Index);
            Assert.Equal(TokenType.CLOSE_PAREN, consumedToken.Type);
            Assert.Equal(")", consumedToken.Content);
        }
    }

    public class PeekTest
    {
        [Fact]
        private void ShouldReturnTheFirstTokenInListIfNoTokensHaveBeenConsumed()
        {
            var token = new Token {Type = TokenType.ID, Line = 10, Index = 13, Content = "ب"};
            var tokenConsumer = new TokenConsumer(new List<Token> {token}, 0);
            Assert.Equal(token, tokenConsumer.Peek());
        }

        [Fact]
        private void ShouldThrowAnExceptionWhenIndexIsOutOfRange()
        {
            var token = new Token {Type = TokenType.ID, Line = 10, Index = 13, Content = "ب"};
            var tokenConsumer = new TokenConsumer(new List<Token> {token}, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => tokenConsumer.Peek());
        }
    }

    public class CanConsumeTest
    {
        [Fact]
        private void ShouldReturnTrueIfIndexIsLessThanListCount()
        {
            var token = new Token {Type = TokenType.ID, Line = 10, Index = 13, Content = "ب"};
            var tokenConsumer = new TokenConsumer(new List<Token> {token}, 0);
            Assert.True(tokenConsumer.CanConsume());
        }

        [Fact]
        private void ShouldReturnFalseIfIndexIsEqualToListCount()
        {
            var token = new Token {Type = TokenType.ID, Line = 10, Index = 13, Content = "ب"};
            var tokenConsumer = new TokenConsumer(new List<Token> {token}, 1);
            Assert.False(tokenConsumer.CanConsume());
        }

        [Fact]
        private void ShouldReturnFalseIfIndexIsGreaterThanToListCount()
        {
            var token = new Token {Type = TokenType.ID, Line = 10, Index = 13, Content = "ب"};
            var tokenConsumer = new TokenConsumer(new List<Token> {token}, 2);
            Assert.False(tokenConsumer.CanConsume());
        }
    }
}