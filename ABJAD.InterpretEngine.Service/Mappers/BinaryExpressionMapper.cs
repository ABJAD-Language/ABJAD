using ABJAD.InterpretEngine.Service.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;

namespace ABJAD.InterpretEngine.Service.Mappers;

public static class BinaryExpressionMapper
{
    public static Expression MapBinary(object jsonObject)
    {
        var type = JsonUtils.GetType(jsonObject);
        var subtype = type.Split(".")[2];

        return subtype switch
        {
            "addition" => MapAddition(jsonObject),
            "subtraction" => MapSubtraction(jsonObject),
            "multiplication" => MapMultiplication(jsonObject),
            "division" => MapDivision(jsonObject),
            "and" => MapsAndOperation(jsonObject),
            "or" => MapOrOperation(jsonObject),
            "equalityCheck" => MapEqualityCheck(jsonObject),
            "greaterCheck" => MapGreaterCheck(jsonObject),
            "greaterOrEqualCheck" => MapGreaterOrEqualCheck(jsonObject),
            "inequalityCheck" => MapInequalityCheck(jsonObject),
            "lessCheck" => MapLessCheck(jsonObject),
            "lessOrEqualCheck" => MapLessOrEqualCheck(jsonObject),
            "modulo" => MapModulo(jsonObject)
        };
    }

    private static Expression MapModulo(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<ModuloApiModel>(jsonObject);
        return new Modulo()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }

    private static Expression MapLessOrEqualCheck(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<LessOrEqualCheckApiModel>(jsonObject);
        return new LessOrEqualCheck()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }

    private static Expression MapLessCheck(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<LessCheckApiModel>(jsonObject);
        return new LessCheck()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }

    private static Expression MapInequalityCheck(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<InequalityCheckApiModel>(jsonObject);
        return new InequalityCheck()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }

    private static Expression MapGreaterOrEqualCheck(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<GreaterOrEqualCheckApiModel>(jsonObject);
        return new GreaterOrEqualCheck()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }

    private static Expression MapGreaterCheck(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<GreaterCheckApiModel>(jsonObject);
        return new GreaterCheck()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }

    private static Expression MapEqualityCheck(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<EqualityCheckApiModel>(jsonObject);
        return new EqualityCheck()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }

    private static Expression MapOrOperation(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<OrOperationApiModel>(jsonObject);
        return new LogicalOr()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }

    private static Expression MapsAndOperation(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<LogicalAndApiModel>(jsonObject);
        return new LogicalAnd()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }

    private static Expression MapDivision(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<DivisionApiModel>(jsonObject);
        return new Division()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }

    private static Expression MapMultiplication(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<MultiplicationApiModel>(jsonObject);
        return new Multiplication()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }

    private static Expression MapSubtraction(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<SubtractionApiModel>(jsonObject);
        return new Subtraction()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }

    private static Expression MapAddition(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<AdditionApiModel>(jsonObject);
        return new Addition()
        {
            FirstOperand = ExpressionMapper.Map(apiModel.FirstOperand),
            SecondOperand = ExpressionMapper.Map(apiModel.SecondOperand)
        };
    }
}