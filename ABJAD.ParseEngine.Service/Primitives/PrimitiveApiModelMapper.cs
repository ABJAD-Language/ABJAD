using ABJAD.ParseEngine.Primitives;

namespace ABJAD.ParseEngine.Service.Primitives;

public static class PrimitiveApiModelMapper
{
    public static PrimitiveApiModel Map(Primitive primitive)
    {
        return primitive switch
        {
            StringPrimitive stringPrimitive => new StringPrimitiveApiModel(stringPrimitive.Value),
            NumberPrimitive numberPrimitive => new NumberPrimitiveApiModel(numberPrimitive.Value),
            BoolPrimitive boolPrimitive => new BoolPrimitiveApiModel(boolPrimitive.Value),
            IdentifierPrimitive identifierPrimitive => new IdentifierPrimitiveApiModel(identifierPrimitive.Value),
            NullPrimitive => new NullPrimitiveApiModel()
        };
    }
}