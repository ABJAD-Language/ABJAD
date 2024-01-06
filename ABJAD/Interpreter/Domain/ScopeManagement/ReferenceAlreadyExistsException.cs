namespace ABJAD.Interpreter.Domain.ScopeManagement;

public class ReferenceAlreadyExistsException : InterpretationException
{
    public ReferenceAlreadyExistsException(string reference) : base(FormulateArabicMessage(reference), FormulateEnglishMessage(reference))
    {
    }

    private static string FormulateArabicMessage(string reference)
    {
        return $"المؤشر {reference} موجود فعلا في المجال الحالي.";
    }

    private static string FormulateEnglishMessage(string reference)
    {
        return $"Reference {reference} already exists in the current scope.";
    }
}