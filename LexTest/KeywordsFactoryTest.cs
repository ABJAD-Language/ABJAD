using LexEngine;
using Xunit;

namespace LexTest;

public class KeywordsFactoryTest
{
    private readonly KeywordsFactory keywordsFactory;
    
    public KeywordsFactoryTest()
    {
        keywordsFactory = new KeywordsFactory();
    }

    [Theory]
    [InlineData("منطق", TokenType.BOOL)]
    [InlineData("صنف", TokenType.CLASS)]
    [InlineData("ثابت", TokenType.CONST)]
    [InlineData("والا", TokenType.ELSE)]
    [InlineData("خطا", TokenType.FALSE)]
    [InlineData("كرر", TokenType.FOR)]
    [InlineData("دالة", TokenType.FUNC)]
    [InlineData("اذا", TokenType.IF)]
    [InlineData("انشئ", TokenType.NEW)]
    [InlineData("عدم", TokenType.NULL)]
    [InlineData("رقم", TokenType.NUMBER)]
    [InlineData("اكتب", TokenType.PRINT)]
    [InlineData("ارجع", TokenType.RETURN)]
    [InlineData("مقطع", TokenType.STRING)]
    [InlineData("صحيح", TokenType.TRUE)]
    [InlineData("نوع", TokenType.TYPEOF)]
    [InlineData("طالما", TokenType.WHILE)]
    [InlineData("متغير", TokenType.VAR)]
    private void TokensCheck(string label, TokenType type)
    {
        Assert.Equal(type, keywordsFactory.GetToken(label));
    }

    [Fact]
    private void ThrowsExceptionWhenTokenDoesNotExist()
    {
        Assert.Throws<InvalidTokenException>(() => keywordsFactory.GetToken("غير_موجود"));
    }
}