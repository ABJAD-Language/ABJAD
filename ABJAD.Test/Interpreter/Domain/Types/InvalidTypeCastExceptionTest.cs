using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Test.Interpreter.Domain.Types;

public class InvalidTypeCastExceptionTest
{
    [Fact(DisplayName = "generates the expected messages")]
    public void generates_the_expected_messages()
    {
        var exception = new InvalidTypeCastException(DataType.Number(), "3.-1");
        Assert.Equal("Cannot cast the value 3.-1 to type رقم.", exception.EnglishMessage);
        Assert.Equal("Cannot cast the value 3.-1 to type رقم.", exception.Message);
        Assert.Equal("لا يمكن تحويل القيمة 3.-1 إلى نوع رقم.", exception.ArabicMessage);
    }
}