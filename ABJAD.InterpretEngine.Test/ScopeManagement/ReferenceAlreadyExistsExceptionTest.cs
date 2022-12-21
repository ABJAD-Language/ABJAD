using ABJAD.InterpretEngine.ScopeManagement;

namespace ABJAD.InterpretEngine.Test.ScopeManagement;

public class ReferenceAlreadyExistsExceptionTest
{
    [Fact(DisplayName = "generates the expected messages")]
    public void generates_the_expected_messages()
    {
        var exception = new ReferenceAlreadyExistsException("اسم");
        var englishMessage = "Reference اسم already exists in the current scope.";
        var arabicMessage = "المؤشر اسم موجود فعلا في المجال الحالي.";
        Assert.Equal(englishMessage, exception.Message);
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}