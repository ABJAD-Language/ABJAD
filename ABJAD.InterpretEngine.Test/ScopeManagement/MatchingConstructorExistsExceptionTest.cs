using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Test.ScopeManagement;

public class MatchingConstructorExistsExceptionTest
{
    [Fact(DisplayName = "generates the expected messages")]
    public void generates_the_expected_messages()
    {
        var exception = new MatchingConstructorExistsException( "انسان", DataType.Bool(), DataType.Number());
        var englishMessage = "A constructor for the class انسان that takes the parameters [منطق, رقم] already exists.";
        var arabicMessage = "منشئ للصنف انسان والذي يستقبل المعطيات [منطق، رقم] موجود فعلا.";
        Assert.Equal(englishMessage, exception.Message);
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}