namespace ABJAD.Interpreter.Domain.Statements;

public class IllegalUseOfReturnStatementException : InterpretationException
{
    public IllegalUseOfReturnStatementException()
        : base(FormulateArabicMessage(), FormulateEnglishMessage())
    {
    }

    private static string FormulateArabicMessage()
    {
        return "لا يمكن الاعتماد على عبارة الإرجاع خارج سياق الدالة.";
    }

    private static string FormulateEnglishMessage()
    {
        return "Return statements can only be used inside function bodies.";
    }
}