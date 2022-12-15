namespace ABJAD.InterpretEngine.Types;

public class InvalidTypeException : InterpretationException
{
    public InvalidTypeException(DataType requiredType, DataType actualType) : 
        base(FormulateArabicMessage(requiredType, actualType), FormulateEnglishMessage(requiredType, actualType))
    {
    }

    private static string FormulateArabicMessage(DataType requiredType, DataType actualType)
    {
        return $"مطلوب قيمة من نوع {requiredType.GetValue()} لكن الموجود هو قيمة من نوع {actualType.GetValue()}.";
    }

    private static string FormulateEnglishMessage(DataType requiredType, DataType actualType)
    {
        return $"Required value of type {requiredType.GetValue()} but found value of type {actualType.GetValue()} instead.";
    }
}