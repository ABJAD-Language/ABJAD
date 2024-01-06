namespace ABJAD.Parser.Expressions.Primitives;

public class IdentifierPrimitiveApiModel : PrimitiveExpressionApiModel
{
    public string Value { get; }

    public IdentifierPrimitiveApiModel(string value) : base("identifier")
    {
        Value = value;
    }
}