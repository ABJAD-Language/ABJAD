namespace ABJAD.InterpretEngine.Expressions;

public class NumberNotPositiveException: InterpretationException
{
    public NumberNotPositiveException(double number) 
        : base(FormulateArabicMessage(number), Formulate(number))
    {
    }

    private static string FormulateArabicMessage(double number)
    {
        return $"الرقم {number} هو عدد سلبي ولا يناسب هذه العملية.";
    }

    private static string Formulate(double number)
    {
        return $"{number} is a negative number and does not fit this operation.";
    }
}