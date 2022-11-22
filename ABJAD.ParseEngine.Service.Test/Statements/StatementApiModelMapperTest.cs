using ABJAD.ParseEngine.Bindings;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Expressions.Assignments;
using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Service.Bindings;
using ABJAD.ParseEngine.Service.Declarations;
using ABJAD.ParseEngine.Service.Expressions.Assignments;
using ABJAD.ParseEngine.Service.Primitives;
using ABJAD.ParseEngine.Service.Statements;
using ABJAD.ParseEngine.Statements;
using FluentAssertions;
using static ABJAD.ParseEngine.Service.Statements.StatementApiModelMapper;

namespace ABJAD.ParseEngine.Service.Test.Statements;

public class StatementApiModelMapperTest
{
    [Fact(DisplayName = "maps assignment statement correctly")]
    public void maps_assignment_statement_correctly()
    {
        var statementApiModel = Map(new AssignmentStatement("target",
            new PrimitiveExpression(IdentifierPrimitive.From("value"))));
        var expectedApiModel = new AssignmentStatementApiModel("target",
            new IdentifierPrimitiveApiModel("value"));
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps expression statement correctly")]
    public void maps_expression_statement_correctly()
    {
        var statementApiModel = Map(new ExpressionStatement(new PrimitiveExpression(NumberPrimitive.From("2"))));
        var expectedApiModel = new ExpressionStatementApiModel(new NumberPrimitiveApiModel(2));
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps if statement correctly")]
    public void maps_if_statement_correctly()
    {
        var assignmentStatement = new AssignmentStatement("target", new PrimitiveExpression(NumberPrimitive.From("4")));
        var ifStatement = new IfStatement(new PrimitiveExpression(BoolPrimitive.True()), assignmentStatement);
        var statementApiModel = Map(ifStatement);

        var assignmentStatementApiModel = new AssignmentStatementApiModel("target", new NumberPrimitiveApiModel(4));
        var expectedApiModel = new IfStatementApiModel(new BoolPrimitiveApiModel(true), assignmentStatementApiModel);
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps if else statement correctly")]
    public void maps_if_else_statement_correctly()
    {
        var assignmentStatement = new AssignmentStatement("target", new PrimitiveExpression(NumberPrimitive.From("4")));
        var ifStatement = new IfStatement(new PrimitiveExpression(BoolPrimitive.True()), assignmentStatement);
        var ifElseStatement = new IfElseStatement(ifStatement, new List<IfStatement> { ifStatement }, assignmentStatement);
        var statementApiModel = Map(ifElseStatement);

        var assignmentStatementApiModel = new AssignmentStatementApiModel("target", new NumberPrimitiveApiModel(4));
        var ifStatementApiModel = new IfStatementApiModel(new BoolPrimitiveApiModel(true), assignmentStatementApiModel);
        var expectedApiModel = new IfElseStatementApiModel(ifStatementApiModel,
            new List<IfStatementApiModel> { ifStatementApiModel }, assignmentStatementApiModel);
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps print statement correctly")]
    public void maps_print_statement_correctly()
    {
        var statementApiModel = Map(new PrintStatement(new PrimitiveExpression(IdentifierPrimitive.From("target"))));
        var expectedApiModel = new PrintStatementApiModel(new IdentifierPrimitiveApiModel("target"));
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps return statement correctly")]
    public void maps_return_statement_correctly()
    {
        var statementApiModel = Map(new ReturnStatement(new PrimitiveExpression(IdentifierPrimitive.From("target"))));
        var expectedApiModel = new ReturnStatementApiModel(new IdentifierPrimitiveApiModel("target"));
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps while statement correctly")]
    public void maps_while_statement_correctly()
    {
        var statementApiModel = Map(new WhileStatement(new PrimitiveExpression(BoolPrimitive.True()),
            new PrintStatement(new PrimitiveExpression(IdentifierPrimitive.From("target")))));
        var expectedApiModel = new WhileStatementApiModel(new BoolPrimitiveApiModel(true),
            new PrintStatementApiModel(new IdentifierPrimitiveApiModel("target")));
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps for statement correctly")]
    public void maps_for_statement_correctly()
    {
        var target = new DeclarationBinding(new VariableDeclaration("int", "i", null));
        var condition = new ExpressionStatement(new PrimitiveExpression(BoolPrimitive.True()));
        var callback = new AdditionAssignmentExpression(IdentifierPrimitive.From("i"), new PrimitiveExpression(NumberPrimitive.From("1")));
        var body = new PrintStatement(new PrimitiveExpression(StringPrimitive.From("hello")));
        var statementApiModel = Map(new ForStatement(target, condition, callback, body));

        var expectedTarget = new VariableDeclarationApiModel("int", "i");
        var expectedCondition = new ExpressionStatementApiModel(new BoolPrimitiveApiModel(true));
        var expectedCallback = new AdditionAssignmentExpressionApiModel("i", new NumberPrimitiveApiModel(1));
        var expectedBody = new PrintStatementApiModel(new StringPrimitiveApiModel("hello"));
        var expectedApiModel = new ForStatementApiModel(expectedTarget, expectedCondition, expectedCallback, expectedBody);
        
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps block statement correctly")]
    public void maps_block_statement_correctly()
    {
        var statementApiModel = Map(new BlockStatement(new List<Binding>
            { new DeclarationBinding(new VariableDeclaration("type", "name", null)) }));
        var expectedApiModel = new BlockStatementApiModel(new List<BindingApiModel>
            { new VariableDeclarationApiModel("type", "name") });
       
        statementApiModel.Should().BeEquivalentTo(expectedApiModel, options => options.RespectingRuntimeTypes());
    }
}