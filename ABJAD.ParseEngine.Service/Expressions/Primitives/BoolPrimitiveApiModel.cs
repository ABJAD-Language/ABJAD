namespace ABJAD.ParseEngine.Service.Primitives;

public class BoolPrimitiveApiModel : PrimitiveExpressionApiModel
{
    public bool Value { get; }

    public BoolPrimitiveApiModel(bool value)
    {
        Value = value;
        Type = "primitive.bool";
    }
}