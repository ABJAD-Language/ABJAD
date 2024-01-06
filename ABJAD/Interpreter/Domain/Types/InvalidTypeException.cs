namespace ABJAD.Interpreter.Domain.Types;

public class InvalidTypeException : InterpretationException
{ // TODO test
    public InvalidTypeException(DataType actualType, params DataType[] requiredType) :
        base(FormulateArabicMessage(actualType, requiredType), FormulateEnglishMessage(actualType, requiredType))
    {
    }

    private static string FormulateArabicMessage(DataType actualType, params DataType[] requiredType)
    {
        return $"مطلوب قيمة من نوع [{GetRequiredValuesString(requiredType, "، ")}] لكن الموجود هو قيمة من نوع {actualType.GetValue()}.";
    }

    private static string FormulateEnglishMessage(DataType actualType, params DataType[] requiredType)
    {
        return $"Required value of type [{GetRequiredValuesString(requiredType, ", ")}] but found value of type {actualType.GetValue()} instead.";
    }

    private static object GetRequiredValuesString(DataType[] requiredType, string separator)
    {
        return string.Join(separator, requiredType.Select(type => type.GetValue()));
    }
}