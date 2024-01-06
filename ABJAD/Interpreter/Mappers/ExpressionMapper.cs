using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Shared.Expressions;

namespace ABJAD.Interpreter.Mappers;

public static class ExpressionMapper
{
    public static Expression Map(object jsonObject)
    {
        var type = JsonUtils.GetType(jsonObject);
        var subtype = type.Split(".")[1];

        return subtype switch
        {
            "primitive" => PrimitiveMapper.MapPrimitive(jsonObject),
            "assignment" => AssignmentExpressionMapper.MapAssignment(jsonObject),
            "unary" => UnaryExpressionMapper.MapUnary(jsonObject),
            "binary" => BinaryExpressionMapper.MapBinary(jsonObject),
            "call" => MapMethodCall(jsonObject),
            "instanceField" => MapInstanceFieldAccess(jsonObject),
            "instanceMethodCall" => MapInstanceMethodCall(jsonObject),
            "instantiation" => MapInstantiation(jsonObject)
        };
    }

    private static Expression MapInstantiation(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<InstantiationApiModel>(jsonObject);

        return new Instantiation()
        {
            ClassName = apiModel.Class.Value,
            Arguments = apiModel.Arguments.Select(Map).ToList()
        };
    }

    private static Expression MapInstanceMethodCall(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<InstanceMethodCallApiModel>(jsonObject);

        return new InstanceMethodCall()
        {
            Instances = apiModel.Instances.Select(i => i.Value).ToList(),
            MethodName = apiModel.Method.Value,
            Arguments = apiModel.Arguments.Select(Map).ToList()
        };
    }

    private static Expression MapInstanceFieldAccess(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<InstanceFieldAccessApiModel>(jsonObject);
        return new InstanceFieldAccess()
        {
            Instance = apiModel.Instance.Value,
            NestedFields = apiModel.Fields.Select(f => f.Value).ToList()
        };
    }

    private static Expression MapMethodCall(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<MethodCallApiModel>(jsonObject);
        return new MethodCall()
        {
            MethodName = apiModel.Method.Value,
            Arguments = apiModel.Arguments.Select(Map).ToList()
        };
    }
}