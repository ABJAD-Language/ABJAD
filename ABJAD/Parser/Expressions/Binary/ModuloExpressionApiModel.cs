namespace ABJAD.Parser.Expressions.Binary;

public class ModuloExpressionApiModel : BinaryExpressionApiModel
{
    public ModuloExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("modulo", firstOperand, secondOperand)
    {
    }
}