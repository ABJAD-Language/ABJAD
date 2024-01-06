namespace ABJAD.Interpreter.Domain.Expressions;

public static class NumberUtils
{
    public static bool IsNumberNatural(double number)
    {
        return number % 1 == 0;
    }
}