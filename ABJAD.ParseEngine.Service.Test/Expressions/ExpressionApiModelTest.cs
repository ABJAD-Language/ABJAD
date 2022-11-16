using ABJAD.ParseEngine.Service.Expressions;
using ABJAD.ParseEngine.Service.Expressions.Assignments;
using ABJAD.ParseEngine.Service.Primitives;
using Moq;

namespace ABJAD.ParseEngine.Service.Test.Expressions;

public class ExpressionApiModelTest
{
    [Fact(DisplayName = "primitive expression api model returns correct type")]
    public void primitive_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.primitive", new PrimitiveExpressionApiModel(new Mock<PrimitiveApiModel>().Object).Type);
    }

    [Fact(DisplayName = "instantiation expression api model returns correct type")]
    public void instantiation_expression_api_model_returns_correct_type()
    {
        var classNameExpression = new PrimitiveExpressionApiModel(new Mock<PrimitiveApiModel>().Object);
        Assert.Equal("expression.instantiation", new InstantiationExpressionApiModel(classNameExpression, new List<ExpressionApiModel>()).Type);
    }

    [Fact(DisplayName = "instance method call expression api model returns correct type")]
    public void instance_method_call_expression_api_model_returns_correct_type()
    {
        var methodNameExpression = new PrimitiveExpressionApiModel(new Mock<PrimitiveApiModel>().Object);
        Assert.Equal("expression.instanceMethodCall",
            new InstanceMethodCallExpressionApiModel(new List<PrimitiveExpressionApiModel>(), methodNameExpression,
                new List<ExpressionApiModel>()).Type);
    }

    [Fact(DisplayName = "instance field expression api model returns correct type")]
    public void instance_field_expression_api_model_returns_correct_type()
    {
        var classNameExpression = new PrimitiveExpressionApiModel(new Mock<PrimitiveApiModel>().Object);
        Assert.Equal("expression.instanceField", new InstanceFieldExpressionApiModel(classNameExpression, new List<PrimitiveExpressionApiModel>()).Type);
    }

    [Fact(DisplayName = "call expression api model returns correct type")]
    public void call_expression_api_model_returns_correct_type()
    {
        var methodNameExpression = new PrimitiveExpressionApiModel(new Mock<PrimitiveApiModel>().Object);
        Assert.Equal("expression.call", new CallExpressionApiModel(methodNameExpression, new List<ExpressionApiModel>()).Type);
    }

    [Fact(DisplayName = "addition assignment expression api model returns correct type")]
    public void addition_assignment_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.assignment.addition",
            new AdditionAssignmentExpressionApiModel(new IdentifierPrimitiveApiModel(""),
                new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "division assignment expression api model returns correct type")]
    public void division_assignment_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.assignment.division",
            new DivisionAssignmentExpressionApiModel(new IdentifierPrimitiveApiModel(""),
                new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "multiplication assignment expression api model returns correct type")]
    public void multiplication_assignment_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.assignment.multiplication",
            new MultiplicationAssignmentExpressionApiModel(new IdentifierPrimitiveApiModel(""),
                new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "subtraction assignment expression api model returns correct type")]
    public void subtraction_assignment_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.assignment.subtraction",
            new SubtractionAssignmentExpressionApiModel(new IdentifierPrimitiveApiModel(""),
                new Mock<ExpressionApiModel>().Object).Type);
    }
}