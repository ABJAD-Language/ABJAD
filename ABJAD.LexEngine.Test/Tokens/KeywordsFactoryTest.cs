using ABJAD.LexEngine.Tokens;
using Xunit;

namespace ABJAD.LexEngine.Test.Tokens;

public class KeywordsFactoryTest
{

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
    [InlineData("لاشيء", TokenType.VOID)]
    [InlineData("منشئ", TokenType.CONSTRUCTOR)]
    private void TokensCheck(string label, TokenType type)
    {
        Assert.Equal(type, KeywordsFactory.GetToken(label));
    }

    [Theory]
    [InlineData("منطق", true)]
    [InlineData("ولد", false)]
    [InlineData("متغير", true)]
    [InlineData("متغي", false)]
    private void KeywordCheck(string label, bool isKeyword)
    {
        Assert.Equal(isKeyword, KeywordsFactory.IsKeyword(label));
    }
}