namespace ABJAD.InterpretEngine.ScopeManagement;

public class ClassNotFoundException : InterpretationException
{
    public ClassNotFoundException(string className) 
        : base(FomrulateArabicMessage(className), FormulateEnglishMessage(className))
    {
    }

    private static string FomrulateArabicMessage(string className)
    {
        return $"الصنف {className} غير موجود في المجال.";
    }

    private static string FormulateEnglishMessage(string className)
    {
        return $"Class {className} does not exist in the scope.";
    }
}