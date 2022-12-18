namespace ABJAD.InterpretEngine.Types;

public class InvalidTypeCastException : InterpretationException
{
    public InvalidTypeCastException(DataType targetType, object value) : base(FormulateArabicMessage(targetType, value), FormulateEnglishMessage(targetType, value))
    {
    }

    private static string FormulateArabicMessage(DataType targetType, object value)
    {
        return $"لا يمكن تحويل القيمة {value} إلى نوع {targetType.GetValue()}.";
    }

    private static string FormulateEnglishMessage(DataType targetType, object value)
    {
        return $"Cannot cast the value {value} to type {targetType.GetValue()}.";
    }
}