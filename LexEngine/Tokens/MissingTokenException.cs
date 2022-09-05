namespace LexEngine.Tokens;

public class MissingTokenException : LexicalAnalysisException
{
    public MissingTokenException(int index, int line, string token) : base(FormulateEnglishMessage(index, line, token))
    {
        Index = index;
        Line = line;
        Token = token;
        EnglishMessage = FormulateEnglishMessage(index, line, token);
        ArabicMessage = FormulateArabicMessage(index, line, token);
    }

    public int Index { get; }

    public int Line { get; }

    public string Token { get; }

    public override string EnglishMessage { get; }

    public override string ArabicMessage { get; }

    private static string FormulateEnglishMessage(int index, int line, string token)
    {
        return $"Missing Token: Expected '{token}' at line {line} : {index}";
    }

    private static string FormulateArabicMessage(int index, int line, string token)
    {
        return $"رمز مفقود: الرمز '{token}' لم يوجد على السطر {line} : {index}";
    }
}