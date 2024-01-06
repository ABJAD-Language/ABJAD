using ABJAD.Interpreter.Domain.Expressions;

namespace ABJAD.Test.Interpreter.Domain.Expressions;

public class NumberNotPositiveExceptionTest
{
    [Fact(DisplayName = "generates the expected messages")]
    public void generates_the_expected_messages()
    {
        var exception = new NumberNotPositiveException(-10.0);
        var englishMessage = "-10 is a negative number and does not fit this operation.";
        var arabicMessage = "الرقم -10 هو عدد سلبي ولا يناسب هذه العملية.";
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}