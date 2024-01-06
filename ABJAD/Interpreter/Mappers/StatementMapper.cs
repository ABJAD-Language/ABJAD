using ABJAD.Interpreter.Domain.Shared;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Shared.Statements;

namespace ABJAD.Interpreter.Mappers;

public static class StatementMapper
{
    public static Statement Map(object jsonObject)
    {
        var type = JsonUtils.GetType(jsonObject);
        var statementType = type.Split(".")[1];

        return statementType switch
        {
            "assignment" => MapAssignment(jsonObject),
            "block" => MapBlock(jsonObject),
            "expression" => MapExpression(jsonObject),
            "for" => MapForLoop(jsonObject),
            "if" => MapIf(jsonObject),
            "ifElse" => MapIfElse(jsonObject),
            "print" => MapPrint(jsonObject),
            "return" => MapReturn(jsonObject),
            "while" => MapWhile(jsonObject)
        };
    }

    private static Statement MapWhile(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<WhileApiModel>(jsonObject);
        return new WhileLoop()
        {
            Condition = ExpressionMapper.Map(apiModel.Condition),
            Body = Map(apiModel.Body)
        };
    }

    private static Statement MapReturn(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<ReturnApiModel>(jsonObject);

        return new Return() { Target = ExpressionMapper.Map(apiModel.Target) };
    }

    private static Statement MapPrint(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<PrintApiModel>(jsonObject);

        return new Print() { Target = ExpressionMapper.Map(apiModel.Target) };
    }

    private static Statement MapIf(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<IfApiModel>(jsonObject);
        return new IfElse()
        {
            MainConditional = MapCondition(apiModel),
            OtherConditionals = new List<Conditional>(),
            ElseBody = null
        };
    }

    private static Statement MapIfElse(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<IfElseApiModel>(jsonObject);
        return new IfElse()
        {
            MainConditional = MapCondition(apiModel.MainIfStatement),
            OtherConditionals = apiModel.OtherIfStatements.Select(MapCondition).ToList(),
            ElseBody = Map(apiModel.ElseBody)
        };
    }

    private static Conditional MapCondition(IfApiModel apiModel)
    {
        return new Conditional()
        {
            Condition = ExpressionMapper.Map(apiModel.Condition),
            Body = Map(apiModel.Body)
        };
    }

    private static Statement MapForLoop(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<ForLoopApiModel>(jsonObject);
        return new ForLoop()
        {
            TargetDefinition = MapBinding(apiModel.Target),
            Condition = (ExpressionStatement)Map(apiModel.Condition),
            Callback = ExpressionMapper.Map(apiModel.TargetCallback),
            Body = Map(apiModel.Body)
        };
    }

    private static Statement MapExpression(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<ExpressionStatementApiModel>(jsonObject);
        return new ExpressionStatement() { Target = ExpressionMapper.Map(apiModel.Expression) };
    }

    private static Statement MapBlock(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<BlockApiModel>(jsonObject);
        return new Block()
        {
            Bindings = apiModel.Bindings.Select(MapBinding).ToList()
        };
    }

    private static Binding MapBinding(object o)
    {
        var type = JsonUtils.GetType(o).Split(".")[0];

        return type switch
        {
            "statement" => Map(o),
            "declaration" => DeclarationMapper.Map(o)
        };
    }

    private static Statement MapAssignment(object jsonObject)
    {
        var apiModel = JsonUtils.Deserialize<AssignmentApiModel>(jsonObject);
        return new Assignment() { Target = apiModel.Target, Value = ExpressionMapper.Map(apiModel.Value) };
    }
}