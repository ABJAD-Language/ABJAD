namespace ABJAD.ParseEngine.Expressions;

public class MissingExpressionException : ParsingException
{
    public MissingExpressionException(int line, int index, string expressionType) : base(
        FormulateEnglishMessage(line, index, expressionType))
    {
        Line = line;
        Index = index;
        ArabicMessage = FormulateArabicMessage(line, index, expressionType);
        EnglishMessage = FormulateEnglishMessage(line, index, expressionType);
    }

    public MissingExpressionException(int line, int index) : base(FormulateEnglishMessage(line, index))
    {
        Line = line;
        Index = index;
        ArabicMessage = FormulateArabicMessage(line, index);
        EnglishMessage = FormulateEnglishMessage(line, index);
    }

    private static string FormulateArabicMessage(int line, int index, string expressionType)
    {
        return $"عبارة متوقعة من نوع {expressionType} لم توجد على السطر {line}:{index}";
    }

    private static string FormulateEnglishMessage(int line, int index, string expressionType)
    {
        return $"Expected expression of type {expressionType} was not found at line {line}:{index}";
    }

    private static string FormulateArabicMessage(int line, int index)
    {
        return $"عبارة متوقعة لم توجد على السطر {line}:{index}";
    }

    private static string FormulateEnglishMessage(int line, int index)
    {
        return $"Expected expression was not found at line {line}:{index}";
    }
}