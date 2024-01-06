using ABJAD.Interpreter.Domain.Expressions;

namespace ABJAD.Test.Interpreter.Domain.Expressions;

public class InvalidVoidReturnStatementExceptionTest
{
    [Fact(DisplayName = "generates expected messages")]
    public void generates_expected_messages()
    {
        var exception = new InvalidVoidReturnStatementException("اكتب");
        var englishMessage = "Method اكتب should not return a value.";
        var arabicMessage = "الدالة اكتب لا يمكن أن ترجع قيمة.";
        Assert.Equal(englishMessage, exception.Message);
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}