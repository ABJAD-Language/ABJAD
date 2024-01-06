using ABJAD.Interpreter.Domain.Expressions;

namespace ABJAD.Test.Domain.Expressions;

public class NumberNotNaturalExceptionTest
{
    [Fact(DisplayName = "generates the expected messages")]
    public void generates_the_expected_messages()
    {
        var exception = new NumberNotNaturalException(3.4);
        var englishMessage = "3.4 is a decimal number and does not fit this operation.";
        var arabicMessage = "الرقم 3.4 هو عدد عشري ولا يناسب هذه العملية.";
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}