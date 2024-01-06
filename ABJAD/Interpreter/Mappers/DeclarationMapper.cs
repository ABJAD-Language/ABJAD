using ABJAD.Interpreter.Domain.Shared.Declarations;
using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Types;
using ABJAD.Interpreter.Shared.Declarations;

namespace ABJAD.Interpreter.Mappers;

public static class DeclarationMapper
{
    public static Declaration Map(object jsonObject)
    {
        var type = JsonUtils.GetType(jsonObject).Split(".")[1];

        return type switch
        {
            "variable" => MapVariable(jsonObject),
            "constant" => MapConstant(jsonObject),
            "function" => MapFunction(jsonObject),
            "class" => MapClass(jsonObject),
            "constructor" => MapConstructor(jsonObject)
        };
    }

    private static Declaration MapConstructor(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<ConstructorDeclarationApiModel>(jsonObject);

        return new ConstructorDeclaration()
        {
            Parameters = apiModel.Parameters.Select(MapParameter).ToList(),
            Body = (Block)StatementMapper.Map(apiModel.Body)
        };
    }

    private static Declaration MapClass(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<ClassDeclarationApiModel>(jsonObject);

        return new ClassDeclaration()
        {
            Name = apiModel.Name,
            Declarations = apiModel.Body.Declarations.Select(Map).ToList()
        };
    }

    private static Declaration MapFunction(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<FunctionDeclarationApiModel>(jsonObject);

        return new FunctionDeclaration()
        {
            Name = apiModel.Name,
            ReturnType = apiModel.ReturnType == null ? null : DataType.From(apiModel.ReturnType),
            Parameters = apiModel.Parameters.Select(MapParameter).ToList(),
            Body = (Block)StatementMapper.Map(apiModel.Body)
        };
    }

    private static Parameter MapParameter(ParameterApiModel arg)
    {
        return new Parameter() { Name = arg.Name, Type = DataType.From(arg.Type) };
    }

    private static Declaration MapConstant(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<ConstantDeclarationApiModel>(jsonObject);

        return new ConstantDeclaration()
        {
            Name = apiModel.Name,
            Type = DataType.From(apiModel.Type),
            Value = (Primitive)PrimitiveMapper.MapPrimitive(apiModel.Value)
        };
    }

    private static Declaration MapVariable(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<VariableDeclarationApiModel>(jsonObject);

        return new VariableDeclaration()
        {
            Name = apiModel.Name,
            Type = DataType.From(apiModel.Type),
            Value = apiModel.Value != null ? ExpressionMapper.Map(apiModel.Value) : null
        };
    }
}