using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Test.ScopeManagement;

public class NoSuitableConstructorFoundExceptionTest
{
    [Fact(DisplayName = "generates the expected messages")]
    public void generates_the_expected_messages()
    {
        var exception = new NoSuitableConstructorFoundException("انسان", DataType.Number());
        var englishMessage = "Did not find a constructor for class انسان that takes [رقم] as parameters.";
        var arabicMessage = "لا يوجد منشئ للصنف انسان يأخذ المعطيات التالية [رقم].";
        Assert.Equal(englishMessage, exception.Message);
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}