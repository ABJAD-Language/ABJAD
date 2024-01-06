namespace ABJAD.Parser.Expressions.Primitives;

public class StringPrimitiveApiModel : PrimitiveExpressionApiModel
{
    public string Value { get; }

    public StringPrimitiveApiModel(string value) : base("string")
    {
        Value = value;
    }
}