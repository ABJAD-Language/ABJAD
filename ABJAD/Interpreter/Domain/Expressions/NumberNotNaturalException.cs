namespace ABJAD.Interpreter.Domain.Expressions;

public class NumberNotNaturalException : InterpretationException
{
    public NumberNotNaturalException(double number)
        : base(FormulateArabicMessage(number), Formulate(number))
    {
    }

    private static string FormulateArabicMessage(double number)
    {
        return $"الرقم {number} هو عدد عشري ولا يناسب هذه العملية.";
    }

    private static string Formulate(double number)
    {
        return $"{number} is a decimal number and does not fit this operation.";
    }
}