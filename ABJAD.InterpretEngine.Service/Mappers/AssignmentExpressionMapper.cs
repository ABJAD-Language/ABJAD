using ABJAD.InterpretEngine.Service.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;

namespace ABJAD.InterpretEngine.Service.Mappers;

public static class AssignmentExpressionMapper
{
    public static Expression MapAssignment(object jsonObject)
    {
        var type = JsonUtils.GetType(jsonObject);
        var assignmentType = type.Split(".").Last();

        return assignmentType switch
        {
            "addition" => MapAdditionAssignment(jsonObject),
            "subtraction" => MapSubtractionAssignment(jsonObject),
            "multiplication" => MapMultiplicationAssignment(jsonObject),
            "division" => MapDivisionAssignment(jsonObject)
        };
    }

    private static Expression MapDivisionAssignment(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<DivisionAssignmentApiModel>(jsonObject);
        return new DivisionAssignment() { Target = apiModel.Target, Value = ExpressionMapper.Map(apiModel.Value) };
    }

    private static Expression MapMultiplicationAssignment(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<MultiplicationAssignmentApiModel>(jsonObject);
        return new MultiplicationAssignment() { Target = apiModel.Target, Value = ExpressionMapper.Map(apiModel.Value) };
    }

    private static Expression MapSubtractionAssignment(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<SubtractionAssignmentApiModel>(jsonObject);
        return new SubtractionAssignment() { Target = apiModel.Target, Value = ExpressionMapper.Map(apiModel.Value) };
    }

    private static Expression MapAdditionAssignment(object jsonObject)
    {
        var additionAssignment = JsonUtils.Deserialize<AdditionAssignmentApiModel>(jsonObject);
        return new AdditionAssignment() { Target = additionAssignment.Target, Value = ExpressionMapper.Map(additionAssignment.Value) };
    }
}