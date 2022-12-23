using ABJAD.InterpretEngine.ScopeManagement;

namespace ABJAD.InterpretEngine.Test.ScopeManagement;

public class MatchingFunctionAlreadyExistsExceptionTest
{
    [Fact(DisplayName = "generates the expected messages")]
    public void generates_the_expected_messages()
    {
        var exception = new MatchingFunctionAlreadyExistsException("احسب", 3);
        var englishMessage = "Function with name احسب and 3 parameter(s) already exists in the current scope.";
        var arabicMessage = "دالة باسم احسب و3 معطيات موجودة فعلا في المجال الحالي.";
        Assert.Equal(englishMessage, exception.Message);
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}