namespace ABJAD.Parser.Expressions.Binary;

public abstract class BinaryExpressionApiModel : ExpressionApiModel
{
    public ExpressionApiModel FirstOperand { get; }
    public ExpressionApiModel SecondOperand { get; }

    protected BinaryExpressionApiModel(string type, ExpressionApiModel firstOperand, ExpressionApiModel secondOperand)
    {
        FirstOperand = firstOperand;
        SecondOperand = secondOperand;
        Type = $"expression.binary.{type}";
    }
}