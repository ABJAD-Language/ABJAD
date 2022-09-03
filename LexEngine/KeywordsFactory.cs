namespace LexEngine;

public class KeywordsFactory
{
    private readonly Dictionary<string, TokenType> keywords = new()
    {
        { "منطق", TokenType.BOOL },
        { "صنف", TokenType.CLASS },
        { "ثابت", TokenType.CONST },
        { "والا", TokenType.ELSE },
        { "خطا", TokenType.FALSE },
        { "كرر", TokenType.FOR },
        { "دالة", TokenType.FUNC },
        { "اذا", TokenType.IF },
        { "انشئ", TokenType.NEW },
        { "عدم", TokenType.NULL },
        { "رقم", TokenType.NUMBER },
        { "اكتب", TokenType.PRINT },
        { "ارجع", TokenType.RETURN },
        { "مقطع", TokenType.STRING },
        { "صحيح", TokenType.TRUE },
        { "نوع", TokenType.TYPEOF },
        { "طالما", TokenType.WHILE },
        { "متغير", TokenType.VAR },
    };

    public TokenType GetToken(string label)
    {
        var tokenExists = keywords.TryGetValue(label, out var tokenType);
        if (tokenExists)
        {
            return tokenType;
        }

        throw new InvalidTokenException();
    }
}