namespace ABJAD.InterpretEngine.Expressions;

public class InvalidVoidReturnStatementException : InterpretationException
{
    public InvalidVoidReturnStatementException(string methodName) 
        : base(FormulateArabicMessage(methodName), FormulateEnglishMessage(methodName))
    {
    }

    private static string FormulateArabicMessage(string methodName)
    {
        return $"الدالة {methodName} لا يمكن أن ترجع قيمة.";
    }

    private static string FormulateEnglishMessage(string methodName)
    {
        return $"Method {methodName} should not return a value.";
    }
}