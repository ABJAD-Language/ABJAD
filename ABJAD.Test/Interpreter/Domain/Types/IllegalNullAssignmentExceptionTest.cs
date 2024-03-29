﻿using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Test.Interpreter.Domain.Types;

public class IllegalNullAssignmentExceptionTest
{
    [Fact(DisplayName = "generates the expected messages")]
    public void generates_the_expected_messages()
    {
        var exception = new IllegalNullAssignmentException(DataType.Bool());
        var englishMessage = "The value null cannot be assigned to reference of type منطق.";
        var arabicMessage = "القيمة عدم لا يمكن تعيينها لمؤشر من نوع منطق.";
        Assert.Equal(englishMessage, exception.Message);
        Assert.Equal(englishMessage, exception.EnglishMessage);
        Assert.Equal(arabicMessage, exception.ArabicMessage);
    }
}