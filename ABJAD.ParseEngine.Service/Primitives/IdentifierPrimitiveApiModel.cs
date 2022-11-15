namespace ABJAD.ParseEngine.Service.Primitives;

public class IdentifierPrimitiveApiModel : PrimitiveApiModel
{
    public String Value { get; }

    public IdentifierPrimitiveApiModel(string value)
    {
        Value = value;
        Type = "primitive.identifier";
    }
}