using ABJAD.InterpretEngine.Service.Shared.Expressions.Unary;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;

namespace ABJAD.InterpretEngine.Service.Mappers;

public static class UnaryExpressionMapper
{
    public static Expression MapUnary(object jsonObject)
    {
        var type = JsonUtils.GetType(jsonObject);
        var subtype = type.Split(".")[2];

        return subtype switch
        {
            "negation" => MapNegation(jsonObject),
            "negative" => MapNegative(jsonObject),
            "postfix" => MapPostfix(jsonObject),
            "prefix" => MapPrefix(jsonObject),
            "toBool" => MapToBool(jsonObject),
            "toNumber" => MapToNumber(jsonObject),
            "toString" => MapToString(jsonObject),
            "typeOf" => MapTypeOf(jsonObject)
        };
    }

    private static Expression MapTypeOf(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<TypeOfApiModel>(jsonObject);
        return new TypeOf() { Target = ExpressionMapper.Map(apiModel.Target) };
    }

    private static Expression MapToString(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<ToStringApiModel>(jsonObject);
        return new ToString() { Target = ExpressionMapper.Map(apiModel.Target) };
    }

    private static Expression MapToNumber(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<ToNumberApiModel>(jsonObject);
        return new ToNumber() { Target = ExpressionMapper.Map(apiModel.Target) };
    }

    private static Expression MapToBool(object jsonObject)
    {
        var toBoolApiModel = JsonUtils.Deserialize<ToBoolApiModel>(jsonObject);
        return new ToBool() { Target = ExpressionMapper.Map(toBoolApiModel.Target) };
    }

    private static Expression MapPrefix(object jsonObject)
    {
        var type = JsonUtils.GetType(jsonObject);
        var prefixType = type.Split(".")[3];

        return prefixType switch
        {
            "addition" => MapAdditionPrefix(jsonObject),
            "subtraction" => MapSubtractionPrefix(jsonObject)
        };
    }

    private static Expression MapSubtractionPrefix(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<SubtractionPrefixApiModel>(jsonObject);
        var target = (IdentifierPrimitive) PrimitiveMapper.MapPrimitive(apiModel.Target);
        return new SubtractionPrefix() { Target = target.Value };
    }

    private static Expression MapAdditionPrefix(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<AdditionPrefixApiModel>(jsonObject);
        var target = (IdentifierPrimitive) PrimitiveMapper.MapPrimitive(apiModel.Target);
        return new AdditionPrefix() { Target = target.Value };
    }

    private static Expression MapPostfix(object jsonObject)
    {
        var type = JsonUtils.GetType(jsonObject);
        var postfixType = type.Split(".")[3];

        return postfixType switch
        {
            "addition" => MapAdditionPostfix(jsonObject),
            "subtraction" => MapSubtractionPostfix(jsonObject)
        };
    }

    private static Expression MapSubtractionPostfix(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<SubtractionPostfixApiModel>(jsonObject);
        var target = (IdentifierPrimitive) PrimitiveMapper.MapPrimitive(apiModel.Target);
        return new SubtractionPostfix() { Target = target.Value };
    }

    private static Expression MapAdditionPostfix(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<AdditionPostfixApiModel>(jsonObject);
        var target = (IdentifierPrimitive) PrimitiveMapper.MapPrimitive(apiModel.Target);
        return new AdditionPostfix() { Target = target.Value };
    }

    private static Expression MapNegative(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<NegativeApiModal>(jsonObject);
        return new Negative() { Target = ExpressionMapper.Map(apiModel.Target) };
    }

    private static Expression MapNegation(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<NegationApiModel>(jsonObject);
        return new Negation() { Target = ExpressionMapper.Map(apiModel.Target) };
    }
}