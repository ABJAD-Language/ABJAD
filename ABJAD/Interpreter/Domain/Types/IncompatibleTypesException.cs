namespace ABJAD.Interpreter.Domain.Types;

public class IncompatibleTypesException : InterpretationException
{
    public IncompatibleTypesException(DataType firstType, DataType secondType) :
        base(FormulateArabicMessage(firstType, secondType), FormulateEnglishMessage(firstType, secondType))
    {
    }

    private static string FormulateArabicMessage(DataType firstType, DataType secondType)
    {
        return $"النوعين {firstType.GetValue()} و{secondType.GetValue()} ليسا متناسبين لهذه العملية";
    }

    private static string FormulateEnglishMessage(DataType firstType, DataType secondType)
    {
        return $"Types {firstType.GetValue()} and {secondType.GetValue()} are incompatible for this operation";
    }
}