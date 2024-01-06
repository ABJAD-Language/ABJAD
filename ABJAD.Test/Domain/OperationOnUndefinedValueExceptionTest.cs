using ABJAD.Interpreter.Domain;

namespace ABJAD.Test.Domain;

public class OperationOnUndefinedValueExceptionTest
{
    [Fact(DisplayName = "generates the expected messages")]
    public void generates_the_expected_messages()
    {
        var exception = new OperationOnUndefinedValueException("اسم");
        var englishMessage = "Cannot operate on reference اسم since it has not been assigned a value yet.";
        var arabicMessage = "لا يمكن إجراء هذه العملية على المؤشر اسم حيث أنه لا يحمل قيمة بعد.";
        Assert.Equal(englishMessage, exception.Message);
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }

    [Fact(DisplayName = "generates the expected messages when not passed a reference")]
    public void generates_the_expected_messages_when_not_passed_a_reference()
    {
        var exception = new OperationOnUndefinedValueException();
        var englishMessage = "Cannot operate on a reference that has not been assigned a value yet.";
        var arabicMessage = "لا يمكن إجراء هذه العملية على مؤشر لا يحمل قيمة بعد.";
        Assert.Equal(englishMessage, exception.Message);
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}