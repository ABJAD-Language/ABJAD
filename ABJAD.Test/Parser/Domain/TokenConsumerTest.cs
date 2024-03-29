using ABJAD.Parser.Domain;
using ABJAD.Parser.Domain.Shared;

namespace ABJAD.Test.Parser.Domain;

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
        var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
        Assert.Throws<ArgumentException>(() => new TokenConsumer(new List<Token> { token }, -1));
    }

    public class ConsumeTokenWithTargetTest
    {
        [Fact]
        private void ConsumesTokenCorrectlyWhenMatches()
        {
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var tokens = new List<Token> { token };
            var consumer = new TokenConsumer(tokens, 0);
            var consumedToken = consumer.Consume(TokenType.ID);
            Assert.Equal(token, consumedToken);
        }

        [Fact]
        private void UpdatesIndexCorrectlyAfterConsumingToken()
        {
            var tokens = new List<Token>
            {
                new() { Type = TokenType.ID },
                new() { Type = TokenType.OPEN_PAREN, Line = 3, Index = 5, Content = "(" }
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
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var consumer = new TokenConsumer(new List<Token> { token }, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => consumer.Consume(TokenType.OPEN_PAREN));
        }

        [Fact]
        private void SkipsWhiteSpaces()
        {
            var tokens = new List<Token>
            {
                new() { Type = TokenType.ID },
                new() { Type = TokenType.WHITE_SPACE },
                new() { Type = TokenType.WHITE_SPACE },
                new() { Type = TokenType.OPEN_PAREN, Line = 3, Index = 5, Content = "(" }
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
                new() { Type = TokenType.ID },
                new() { Type = TokenType.WHITE_SPACE },
                new() { Type = TokenType.WHITE_SPACE },
                new() { Type = TokenType.OPEN_PAREN, Line = 3, Index = 5, Content = "(" },
                new() { Type = TokenType.CLOSE_PAREN, Line = 3, Index = 6, Content = ")" }
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
                new() { Type = TokenType.CLOSE_PAREN, Line = 3, Index = 6, Content = ")" }
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
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var tokens = new List<Token> { token };
            var consumer = new TokenConsumer(tokens, 0);
            var consumedToken = consumer.Consume();
            Assert.Equal(token, consumedToken);
        }

        [Fact]
        private void UpdatesIndexCorrectlyAfterConsumingToken()
        {
            var tokens = new List<Token>
            {
                new() { Type = TokenType.ID },
                new() { Type = TokenType.OPEN_PAREN, Line = 3, Index = 5, Content = "(" }
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
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var consumer = new TokenConsumer(new List<Token> { token }, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => consumer.Consume());
        }

        [Fact]
        private void SkipsWhiteSpaces()
        {
            var tokens = new List<Token>
            {
                new() { Type = TokenType.ID },
                new() { Type = TokenType.WHITE_SPACE },
                new() { Type = TokenType.WHITE_SPACE },
                new() { Type = TokenType.OPEN_PAREN, Line = 3, Index = 5, Content = "(" }
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
                new() { Type = TokenType.ID },
                new() { Type = TokenType.WHITE_SPACE },
                new() { Type = TokenType.WHITE_SPACE },
                new() { Type = TokenType.OPEN_PAREN, Line = 3, Index = 5, Content = "(" },
                new() { Type = TokenType.CLOSE_PAREN, Line = 3, Index = 6, Content = ")" }
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
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token }, 0);
            Assert.Equal(token, tokenConsumer.Peek());
        }

        [Fact]
        private void ShouldThrowAnExceptionWhenIndexIsOutOfRange()
        {
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token }, 1);
            Assert.Throws<ArgumentOutOfRangeException>(() => tokenConsumer.Peek());
        }

        [Fact(DisplayName = "should not return the comment token if it is the head")]
        public void should_not_return_the_comment_token_if_it_is_the_head()
        {
            var tokens = new List<Token>
            {
                new() { Type = TokenType.COMMENT },
                new() { Type = TokenType.IF }
            };

            var tokenConsumer = new TokenConsumer(tokens, 0);
            Assert.Equal(TokenType.IF, tokenConsumer.Peek().Type);
        }
    }

    public class LookAheadTest
    {
        [Fact]
        private void ShouldReturnTheHeadTokenWhenOffsetIsZero()
        {
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token }, 0);
            Assert.Equal(token, tokenConsumer.LookAhead(0));
        }

        [Fact]
        private void ShouldReturnTokenAfterHeadWhenOffsetIsOne()
        {
            var token1 = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var token2 = new Token { Type = TokenType.NUMBER, Line = 4, Index = 2, Content = "3" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token1, token2 }, 0);
            Assert.Equal(token2, tokenConsumer.LookAhead(1));
        }

        [Fact]
        private void ShouldThrowExceptionIfOffsetIsNegative()
        {
            var token1 = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var token2 = new Token { Type = TokenType.NUMBER, Line = 4, Index = 2, Content = "3" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token1, token2 }, 1);
            Assert.Throws<ArgumentException>(() => tokenConsumer.LookAhead(-1));
        }

        [Fact(DisplayName = "should skip comment token")]
        public void should_skip_comment_token()
        {
            var token1 = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var token2 = new Token { Type = TokenType.COMMENT, Line = 12, Index = 2, Content = "ignore" };
            var token3 = new Token { Type = TokenType.NUMBER, Line = 13, Index = 2, Content = "3" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token1, token2, token3 }, 0);
            Assert.Equal(token3, tokenConsumer.LookAhead(1));
        }
    }

    public class GetCurrentLineTest
    {
        [Fact]
        private void ReturnsTheLineOfTheHeadToken()
        {
            var token = new Token { Line = 10 };
            var tokenConsumer = new TokenConsumer(new List<Token> { token }, 0);
            Assert.Equal(10, tokenConsumer.GetCurrentLine());
        }
    }

    public class GetCurrentIndexTest
    {
        [Fact]
        private void ReturnsTheIndexOfTheHeadToken()
        {
            var token = new Token { Index = 3 };
            var tokenConsumer = new TokenConsumer(new List<Token> { token }, 0);
            Assert.Equal(3, tokenConsumer.GetCurrentIndex());
        }
    }

    public class CanConsumeTest
    {
        [Fact]
        private void ShouldReturnTrueIfIndexIsLessThanListCount()
        {
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token }, 0);
            Assert.True(tokenConsumer.CanConsume());
        }

        [Fact]
        private void ShouldReturnFalseIfIndexIsEqualToListCount()
        {
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token }, 1);
            Assert.False(tokenConsumer.CanConsume());
        }

        [Fact]
        private void ShouldReturnFalseIfIndexIsGreaterThanToListCount()
        {
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token }, 2);
            Assert.False(tokenConsumer.CanConsume());
        }
    }

    public class CanConsumeTypeTest
    {
        [Fact]
        private void ShouldReturnFalseWhenNoMoreTokensToConsume()
        {
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token }, 1);
            Assert.False(tokenConsumer.CanConsume(TokenType.ID));
        }

        [Fact]
        private void ShouldReturnFalseWhenIndexIsGreaterThanListCount()
        {
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token }, 2);
            Assert.False(tokenConsumer.CanConsume(TokenType.ID));
        }

        [Fact]
        private void ShouldReturnFalseWhenHeadTokenTypeDoesNotMatchTarget()
        {
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token }, 0);
            Assert.False(tokenConsumer.CanConsume(TokenType.NUMBER));
        }

        [Fact]
        private void ShouldReturnTrueWhenHeadTokenTypeMatchesTarget()
        {
            var token = new Token { Type = TokenType.ID, Line = 10, Index = 13, Content = "ب" };
            var tokenConsumer = new TokenConsumer(new List<Token> { token }, 0);
            Assert.True(tokenConsumer.CanConsume(TokenType.ID));
        }
    }
}