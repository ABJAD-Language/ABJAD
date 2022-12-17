using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Test.Types;

public class IncompatibleTypesExceptionTest
{
    [Fact(DisplayName = "formulates the expected messages")]
    public void formulates_the_expected_messages()
    {
        var exception = new IncompatibleTypesException(DataType.Number(), DataType.String());
        var arabicMessage = "النوعين رقم ومقطع ليسا متناسبين لهذه العملية";
        var englishMessage = "Types رقم and مقطع are incompatible for this operation";
        Assert.Equal(arabicMessage, exception.ArabicMessage);
        Assert.Equal(englishMessage, exception.EnglishMessage);
    }
}