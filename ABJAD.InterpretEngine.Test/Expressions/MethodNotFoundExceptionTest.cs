using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Test.Expressions;

public class MethodNotFoundExceptionTest
{
    [Fact(DisplayName = "generates the expected messages")]
    public void generates_the_expected_messages()
    {
        var exception = new MethodNotFoundException("احسب", DataType.Number(), DataType.Custom("انسان"));
        var englishMessage = "Method with name احسب that takes the parameters (رقم, انسان) does not exist.";
        var arabicMessage = "لا يوجد في المجال دالة باسم احسب تأخذ المعطيات (رقم، انسان).";
        Assert.Equal(englishMessage, exception.Message);
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}