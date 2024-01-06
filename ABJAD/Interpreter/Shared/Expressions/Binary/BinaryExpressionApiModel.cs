namespace ABJAD.Interpreter.Shared.Expressions.Binary;

public abstract class BinaryExpressionApiModel
{
    public object FirstOperand { get; }
    public object SecondOperand { get; }

    protected BinaryExpressionApiModel(object firstOperand, object secondOperand)
    {
        FirstOperand = firstOperand;
        SecondOperand = secondOperand;
    }
}