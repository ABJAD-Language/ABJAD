namespace ABJAD.Parser.Domain.Expressions;

public class FailedToParseExpressionException : ParsingException
{
    public FailedToParseExpressionException(int line, int index) : base(FormulateEnglishMessage(line, index))
    {
        Line = line;
        Index = index;
        EnglishMessage = FormulateEnglishMessage(line, index);
        ArabicMessage = FormulateArabicMessage(line, index);
    }

    private static string FormulateArabicMessage(int line, int index)
    {
        return $"فشل في تحليل العبارة على السطر {line}:{index}";
    }

    private static string FormulateEnglishMessage(int line, int index)
    {
        return $"Failed to parse expression at line {line}:{index}";
    }
}