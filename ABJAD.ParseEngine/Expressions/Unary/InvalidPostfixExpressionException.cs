namespace ABJAD.ParseEngine.Expressions.Unary;

public class InvalidPostfixExpressionException : ParsingException
{
    public InvalidPostfixExpressionException(int line, int index) : base(FormulateEnglishMessage(line, index))
    {
        Line = line;
        Index = index;
        EnglishMessage = FormulateEnglishMessage(line, index);
        ArabicMessage = FormulateArabicMessage(line, index);
    }

    private static string FormulateArabicMessage(int line, int index)
    {
        return $"العمليات الحسابية المؤخرة يجب أن تستعمل مع المتغيرات فقط ({line}:{index})";
    }

    private static string FormulateEnglishMessage(int line, int index)
    {
        return $"Postfix can only be used with variables ({line}:{index})";
    }
}