using ABJAD.InterpretEngine.ScopeManagement;

namespace ABJAD.InterpretEngine.Test.ScopeManagement;

public class ClassNotFoundExceptionTest
{
    [Fact(DisplayName = "generates the expected messages")]
    public void generates_the_expected_messages()
    {
        var exception = new ClassNotFoundException("انسان");
        var englishMessage = "Class انسان does not exist in the scope.";
        var arabicMessage = "الصنف انسان غير موجود في المجال.";
        Assert.Equal(englishMessage, exception.Message);
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}