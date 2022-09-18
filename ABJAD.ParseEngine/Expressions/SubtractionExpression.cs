namespace ABJAD.ParseEngine.Expressions;

public class SubtractionExpression : BinaryExpression
{
    public SubtractionExpression(Expression firstOperand, Expression secondOperand) : base(firstOperand, secondOperand)
    {
    }
}