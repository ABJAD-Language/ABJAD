namespace ABJAD.Interpreter.Domain.ScopeManagement;

public class MatchingClassDeclarationAlreadyExistsException : InterpretationException
{
    public MatchingClassDeclarationAlreadyExistsException(string className)
        : base(FormulateArabicMessage(className), FormulateEnglishMessage(className))
    {
    }

    private static string FormulateArabicMessage(string className)
    {
        return $"صنف باسم {className} موجود فعلا في المجال.";
    }

    private static string FormulateEnglishMessage(string className)
    {
        return $"Class with name {className} already exists in scope.";
    }
}