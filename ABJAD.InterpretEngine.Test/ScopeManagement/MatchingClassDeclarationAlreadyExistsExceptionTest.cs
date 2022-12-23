using ABJAD.InterpretEngine.ScopeManagement;

namespace ABJAD.InterpretEngine.Test.ScopeManagement;

public class MatchingClassDeclarationAlreadyExistsExceptionTest
{
    [Fact(DisplayName = "generates the expected messages")]
    public void generates_the_expected_messages()
    {
        var exception = new MatchingClassDeclarationAlreadyExistsException("انسان");
        var englishMessage = "Class with name انسان already exists in scope.";
        var arabicMessage = "صنف باسم انسان موجود فعلا في المجال.";
        Assert.Equal(englishMessage, exception.Message);
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}