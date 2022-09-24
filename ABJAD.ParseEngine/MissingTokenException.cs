namespace ABJAD.ParseEngine;

public class MissingTokenException : ParsingException
{
    public MissingTokenException(int line, int index, string tokenType) : base(
        FormulateEnglishMessage(line, index, tokenType))
    {
        Line = line;
        Index = index;
        ArabicMessage = FormulateArabicMessage(line, index, tokenType);
        EnglishMessage = FormulateEnglishMessage(line, index, tokenType);
    }

    private static string FormulateArabicMessage(int line, int index, string tokenType)
    {
        return $"كلمة متوقعة من نوع {tokenType} لم توجد على السطر {line}:{index}";
    }

    private static string FormulateEnglishMessage(int line, int index, string tokenType)
    {
        return $"Expected token of type {tokenType} was not found at line {line}:{index}";
    }
}