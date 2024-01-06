namespace ABJAD.Interpreter.Domain.Shared.Expressions.Binary;

public class DivisionByZeroException : InterpretationException
{
    public DivisionByZeroException() : base("لا يمكنك القسمة على صفر.", "Division by zero is not allowed.")
    {
    }
}