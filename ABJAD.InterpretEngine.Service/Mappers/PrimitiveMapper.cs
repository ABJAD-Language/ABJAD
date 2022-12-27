using ABJAD.InterpretEngine.Service.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;

namespace ABJAD.InterpretEngine.Service.Mappers;

public static class PrimitiveMapper
{
    public static Expression MapPrimitive(object jsonObject)
    {
        var type = JsonUtils.GetType(jsonObject);
        var primitiveType = type.Split(".").Last();

        return primitiveType switch
        {
            "number" => MapNumberPrimitive(JsonUtils.Deserialize<NumberPrimitiveApiModel>(jsonObject)),
            "bool" => MapBoolPrimitive(JsonUtils.Deserialize<BoolPrimitiveApiModel>(jsonObject)),
            "string" => MapStringPrimitive(JsonUtils.Deserialize<StringPrimitiveApiModel>(jsonObject)),
            "identifier" => MapIdentifierPrimitive(JsonUtils.Deserialize<IdentifierApiModel>(jsonObject)),
            "null" => new NullPrimitive()
        };
    }

    private static Expression MapIdentifierPrimitive(IdentifierApiModel apiModel)
    {
        return new IdentifierPrimitive() { Value = apiModel.Value };
    }

    private static Expression MapStringPrimitive(StringPrimitiveApiModel apiModel)
    {
        return new StringPrimitive() { Value = apiModel.Value };
    }

    private static Expression MapBoolPrimitive(BoolPrimitiveApiModel apiModel)
    {
        return new BoolPrimitive() { Value = apiModel.Value };
    }

    private static NumberPrimitive MapNumberPrimitive(NumberPrimitiveApiModel numberPrimitive)
    {
        return new NumberPrimitive() { Value = numberPrimitive.Value };
    }
}