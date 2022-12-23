namespace ABJAD.InterpretEngine.ScopeManagement;

public class MatchingFunctionAlreadyExistsException : InterpretationException
{
    public MatchingFunctionAlreadyExistsException(string functionName, int numberOfParameters) 
        : base(FormulateArabicMessage(functionName, numberOfParameters), FormulateEnglishMessage(functionName, numberOfParameters))
    {
    }

    private static string FormulateArabicMessage(string functionName, int numberOfParameters)
    {
        return $"دالة باسم {functionName} و{numberOfParameters} معطيات موجودة فعلا في المجال الحالي.";
    }

    private static string FormulateEnglishMessage(string functionName, int numberOfParameters)
    {
        return $"Function with name {functionName} and {numberOfParameters} parameter(s) already exists in the current scope.";
    }
}