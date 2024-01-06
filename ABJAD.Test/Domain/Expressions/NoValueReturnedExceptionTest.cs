using ABJAD.Interpreter.Domain.Expressions;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Test.Domain.Expressions;

public class NoValueReturnedExceptionTest
{
    [Fact(DisplayName = "return the expected messages")]
    public void return_the_expected_messages()
    {
        var exception = new NoValueReturnedException("احسب", DataType.Number());
        var englishMessage = "Method احسب was expected to return a value of type رقم, but did not.";
        var arabicMessage = "الدالة احسب كانت من المتوقع أن ترجع قيمة من نوع رقم، لكنها لم تفعل.";
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}