namespace ABJAD.ParseEngine.Service.Primitives;

public class StringPrimitiveApiModel : PrimitiveExpressionApiModel
{
    public string Value { get; }

    public StringPrimitiveApiModel(string value)
    {
        Value = value;
        Type = "primitive.string";
    }
}