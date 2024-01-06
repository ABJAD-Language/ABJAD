namespace ABJAD.Interpreter.Domain.ScopeManagement;

public class IllegalConstantValueChangeException : InterpretationException
{
    public IllegalConstantValueChangeException(string reference)
        : base($"تغيير قيمة المؤشر الثابت {reference} غير مسموح.", $"Changing the value of the constant {reference} is not allowed.")
    {
    }
}