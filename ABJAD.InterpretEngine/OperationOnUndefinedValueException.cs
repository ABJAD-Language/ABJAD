namespace ABJAD.InterpretEngine;

public class OperationOnUndefinedValueException : InterpretationException
{
    public OperationOnUndefinedValueException(string reference) : base(FormulateArabicMessage(reference), FormulateEnglishMessage(reference))
    {
    }
    
    public OperationOnUndefinedValueException() : base(FormulateArabicMessage(), FormulateEnglishMessage())
    {
    }

    private static string FormulateArabicMessage(string reference)
    {
        return $"لا يمكن إجراء هذه العملية على المؤشر {reference} حيث أنه لا يحمل قيمة بعد.";
    }

    private static string FormulateEnglishMessage(string reference)
    {
        return $"Cannot operate on reference {reference} since it has not been assigned a value yet.";
    }

    private static string FormulateArabicMessage()
    {
        return "لا يمكن إجراء هذه العملية على مؤشر لا يحمل قيمة بعد.";
    }

    private static string FormulateEnglishMessage()
    {
        return "Cannot operate on a reference that has not been assigned a value yet.";
    }
}