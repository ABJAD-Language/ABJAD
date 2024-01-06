namespace ABJAD.Interpreter.Domain.Expressions;

public class ReferenceNameDoesNotExistException : InterpretationException
{
    public ReferenceNameDoesNotExistException(string name) : base(FormulateArabicMessage(name), FormulateEnglishMessage(name))
    {
    }

    private static string FormulateArabicMessage(string name)
    {
        return $"لا توجد قيمة معرفة تحت اسم {name}.";
    }

    private static string FormulateEnglishMessage(string name)
    {
        return $"Value for identifier {name} was not found.";
    }
}