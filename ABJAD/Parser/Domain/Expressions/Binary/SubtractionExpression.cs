namespace ABJAD.Parser.Domain.Expressions.Binary;

public class SubtractionExpression : BinaryExpression
{
    public SubtractionExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}