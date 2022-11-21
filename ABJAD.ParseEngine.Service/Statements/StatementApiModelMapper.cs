using ABJAD.ParseEngine.Service.Bindings;
using ABJAD.ParseEngine.Service.Expressions;
using ABJAD.ParseEngine.Statements;

namespace ABJAD.ParseEngine.Service.Statements;

public static class StatementApiModelMapper
{
    public static StatementApiModel Map(Statement statement)
    {
        return statement switch
        {
            AssignmentStatement assignmentStatement => Map(assignmentStatement),
            ExpressionStatement expressionStatement => Map(expressionStatement),
            IfStatement ifStatement => Map(ifStatement),
            IfElseStatement ifElseStatement => Map(ifElseStatement),
            PrintStatement printStatement => Map(printStatement),
            ReturnStatement returnStatement => Map(returnStatement),
            WhileStatement whileStatement => Map(whileStatement),
            ForStatement forStatement => Map(forStatement),
            BlockStatement blockStatement => Map(blockStatement)
        };
    }

    public static BlockStatementApiModel Map(BlockStatement blockStatement)
    {
        return new BlockStatementApiModel(blockStatement.Bindings.Select(BindingApiModelMapper.Map).ToList());
    }

    private static ForStatementApiModel Map(ForStatement forStatement)
    {
        return new ForStatementApiModel(BindingApiModelMapper.Map(forStatement.TargetDefinition),
            Map(forStatement.Condition), ExpressionApiModelMapper.Map(forStatement.TargetCallback),
            Map(forStatement.Body));
    }

    private static WhileStatementApiModel Map(WhileStatement whileStatement)
    {
        return new WhileStatementApiModel(ExpressionApiModelMapper.Map(whileStatement.Condition), Map(whileStatement.Body));
    }

    private static ReturnStatementApiModel Map(ReturnStatement returnStatement)
    {
        return new ReturnStatementApiModel(ExpressionApiModelMapper.Map(returnStatement.Target));
    }

    private static PrintStatementApiModel Map(PrintStatement printStatement)
    {
        return new PrintStatementApiModel(ExpressionApiModelMapper.Map(printStatement.Target));
    }

    private static IfElseStatementApiModel Map(IfElseStatement ifElseStatement)
    {
        return new IfElseStatementApiModel(Map(ifElseStatement.MainIfStatement),
            ifElseStatement.OtherIfStatements.Select(Map).ToList(), Map(ifElseStatement.ElseBody));
    }

    private static IfStatementApiModel Map(IfStatement ifStatement)
    {
        return new IfStatementApiModel(ExpressionApiModelMapper.Map(ifStatement.Condition), Map(ifStatement.Body));
    }

    private static ExpressionStatementApiModel Map(ExpressionStatement expressionStatement)
    {
        return new ExpressionStatementApiModel(ExpressionApiModelMapper.Map(expressionStatement.Target));
    }

    private static AssignmentStatementApiModel Map(AssignmentStatement assignmentStatement)
    {
        return new AssignmentStatementApiModel(assignmentStatement.Target, ExpressionApiModelMapper.Map(assignmentStatement.Value));
    }
}