namespace ABJAD.LexEngine.Tokens;

public class KeywordsFactory
{
    private static readonly Dictionary<string, TokenType> Keywords = new()
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
        { "لاشيء", TokenType.VOID },
        { "منشئ", TokenType.CONSTRUCTOR },
    };

    public static bool IsKeyword(string label)
    {
        return Keywords.ContainsKey(label);
    }

    public static TokenType GetToken(string label)
    {
        return Keywords[label];
    }
}