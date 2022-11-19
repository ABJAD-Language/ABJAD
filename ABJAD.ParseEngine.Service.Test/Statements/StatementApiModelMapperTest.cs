using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Service.Expressions;
using ABJAD.ParseEngine.Service.Primitives;
using ABJAD.ParseEngine.Service.Statements;
using ABJAD.ParseEngine.Statements;
using FluentAssertions;

namespace ABJAD.ParseEngine.Service.Test.Statements;

public class StatementApiModelMapperTest
{
    [Fact(DisplayName = "maps assignment statement correctly")]
    public void maps_assignment_statement_correctly()
    {
        var statementApiModel = StatementApiModelMapper.Map(new AssignmentStatement("target",
            new PrimitiveExpression(IdentifierPrimitive.From("value"))));
        var expectedApiModel = new AssignmentStatementApiModel("target",
            new PrimitiveExpressionApiModel(new IdentifierPrimitiveApiModel("value")));
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps expression statement correctly")]
    public void maps_expression_statement_correctly()
    {
        var statementApiModel = StatementApiModelMapper.Map(new ExpressionStatement(new PrimitiveExpression(NumberPrimitive.From("2"))));
        var expectedApiModel = new ExpressionStatementApiModel(new PrimitiveExpressionApiModel(new NumberPrimitiveApiModel(2)));
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps if statement correctly")]
    public void maps_if_statement_correctly()
    {
        var assignmentStatement = new AssignmentStatement("target", new PrimitiveExpression(NumberPrimitive.From("4")));
        var ifStatement = new IfStatement(new PrimitiveExpression(BoolPrimitive.True()), assignmentStatement);
        var statementApiModel = StatementApiModelMapper.Map(ifStatement);

        var assignmentStatementApiModel = new AssignmentStatementApiModel("target", new PrimitiveExpressionApiModel(new NumberPrimitiveApiModel(4)));
        var expectedApiModel = new IfStatementApiModel(new PrimitiveExpressionApiModel(new BoolPrimitiveApiModel(true)), assignmentStatementApiModel);
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps if else statement correctly")]
    public void maps_if_else_statement_correctly()
    {
        var assignmentStatement = new AssignmentStatement("target", new PrimitiveExpression(NumberPrimitive.From("4")));
        var ifStatement = new IfStatement(new PrimitiveExpression(BoolPrimitive.True()), assignmentStatement);
        var ifElseStatement = new IfElseStatement(ifStatement, new List<IfStatement> { ifStatement }, assignmentStatement);
        var statementApiModel = StatementApiModelMapper.Map(ifElseStatement);

        var assignmentStatementApiModel = new AssignmentStatementApiModel("target", new PrimitiveExpressionApiModel(new NumberPrimitiveApiModel(4)));
        var ifStatementApiModel = new IfStatementApiModel(new PrimitiveExpressionApiModel(new BoolPrimitiveApiModel(true)), assignmentStatementApiModel);
        var expectedApiModel = new IfElseStatementApiModel(ifStatementApiModel,
            new List<IfStatementApiModel> { ifStatementApiModel }, assignmentStatementApiModel);
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps print statement correctly")]
    public void maps_print_statement_correctly()
    {
        var statementApiModel = StatementApiModelMapper.Map(new PrintStatement(new PrimitiveExpression(IdentifierPrimitive.From("target"))));
        var expectedApiModel = new PrintStatementApiModel(new PrimitiveExpressionApiModel(new IdentifierPrimitiveApiModel("target")));
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps return statement correctly")]
    public void maps_return_statement_correctly()
    {
        var statementApiModel = StatementApiModelMapper.Map(new ReturnStatement(new PrimitiveExpression(IdentifierPrimitive.From("target"))));
        var expectedApiModel = new ReturnStatementApiModel(new PrimitiveExpressionApiModel(new IdentifierPrimitiveApiModel("target")));
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps while statement correctly")]
    public void maps_while_statement_correctly()
    {
        var statementApiModel = StatementApiModelMapper.Map(new WhileStatement(new PrimitiveExpression(BoolPrimitive.True()),
            new PrintStatement(new PrimitiveExpression(IdentifierPrimitive.From("target")))));
        var expectedApiModel = new WhileStatementApiModel(
            new PrimitiveExpressionApiModel(new BoolPrimitiveApiModel(true)),
            new PrintStatementApiModel(new PrimitiveExpressionApiModel(new IdentifierPrimitiveApiModel("target"))));
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }
}