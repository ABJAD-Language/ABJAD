namespace ABJAD.Parser.Expressions.Primitives;

public class NumberPrimitiveApiModel : PrimitiveExpressionApiModel
{
    public double Value { get; }

    public NumberPrimitiveApiModel(double value) : base("number")
    {
        Value = value;
    }
}