namespace LexEngine.Tokens;

public class InvalidTokenException : Exception
{
    public InvalidTokenException(int line, int index) : base(FormulateEnglishMessage(line, index))
    {
        Line = line;
        Index = index;
        EnglishMessage = FormulateEnglishMessage(line, index);
        ArabicMessage = FormulateArabicMessage(line, index);
    }

    private static string FormulateArabicMessage(int line, int index)
    {
        return $"رمز غير صالح على السطر {line} : {index}";
    }

    private static string FormulateEnglishMessage(int line, int index)
    {
        return $"Invalid token at line {line} : {index}";
    }

    public int Line { get; }
    public int Index { get; }
    public string EnglishMessage { get; }
    public string ArabicMessage { get; }
}