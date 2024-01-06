namespace ABJAD.Interpreter.Domain.Types;

public class IllegalNullAssignmentException : InterpretationException
{
    public IllegalNullAssignmentException(DataType type)
        : base(FormulateArabicMessage(type), FormulateEnglishMessage(type))
    {
    }

    private static string FormulateArabicMessage(DataType type)
    {
        return $"القيمة عدم لا يمكن تعيينها لمؤشر من نوع {type.GetValue()}.";
    }

    private static string FormulateEnglishMessage(DataType type)
    {
        return $"The value null cannot be assigned to reference of type {type.GetValue()}.";
    }
}