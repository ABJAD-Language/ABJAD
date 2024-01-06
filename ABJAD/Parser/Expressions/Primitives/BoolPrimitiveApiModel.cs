namespace ABJAD.Parser.Expressions.Primitives;

public class BoolPrimitiveApiModel : PrimitiveExpressionApiModel
{
    public bool Value { get; }

    public BoolPrimitiveApiModel(bool value) : base("bool")
    {
        Value = value;
    }
}