namespace ABJAD.ParseEngine.Service.Primitives;

public class IdentifierPrimitiveApiModel : PrimitiveExpressionApiModel
{
    public String Value { get; }

    public IdentifierPrimitiveApiModel(string value)
    {
        Value = value;
        Type = "primitive.identifier";
    }
}