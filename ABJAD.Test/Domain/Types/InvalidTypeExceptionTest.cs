using ABJAD.Interpreter.Domain.Types;
using NSubstitute;

namespace ABJAD.Test.Domain.Types;

public class InvalidTypeExceptionTest
{
    [Fact(DisplayName = "generates the expected message when one required type is passed")]
    public void generates_the_expected_message_when_one_required_type_is_passed()
    {
        var actualType = Substitute.For<DataType>();
        var requiredType = Substitute.For<DataType>();

        actualType.GetValue().Returns("نوع1");
        requiredType.GetValue().Returns("نوع2");

        var exception = new InvalidTypeException(actualType, requiredType);
        var expectedEnglishMessage = "Required value of type [نوع2] but found value of type نوع1 instead.";
        var expectedArabicMessage = "مطلوب قيمة من نوع [نوع2] لكن الموجود هو قيمة من نوع نوع1.";
        Assert.Equal(expectedArabicMessage, exception.ArabicMessage);
        Assert.Equal(expectedEnglishMessage, exception.EnglishMessage);
    }

    [Fact(DisplayName = "generates the expected message when more than one required type is passed")]
    public void generates_the_expected_message_when_more_than_one_required_type_is_passed()
    {
        var actualType = Substitute.For<DataType>();
        var requiredType1 = Substitute.For<DataType>();
        var requiredType2 = Substitute.For<DataType>();

        actualType.GetValue().Returns("نوع1");
        requiredType1.GetValue().Returns("نوع2");
        requiredType2.GetValue().Returns("نوع3");

        var exception = new InvalidTypeException(actualType, requiredType1, requiredType2);
        var expectedEnglishMessage = "Required value of type [نوع2, نوع3] but found value of type نوع1 instead.";
        var expectedArabicMessage = "مطلوب قيمة من نوع [نوع2، نوع3] لكن الموجود هو قيمة من نوع نوع1.";
        Assert.Equal(expectedArabicMessage, exception.ArabicMessage);
        Assert.Equal(expectedEnglishMessage, exception.EnglishMessage);
    }
}