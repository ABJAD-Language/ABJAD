using ABJAD.ParseEngine.Service.Expressions;
using ABJAD.ParseEngine.Service.Expressions.Assignments;
using ABJAD.ParseEngine.Service.Expressions.Unary;
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

    [Fact(DisplayName = "negation expression api model returns correct type")]
    public void negation_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.negation", new NegationExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "negative expression api model returns correct type")]
    public void negative_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.negative", new NegativeExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "postfix addition expression api model returns correct type")]
    public void postfix_addition_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.postfix.addition", new PostfixAdditionExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "postfix subtraction expression api model returns correct type")]
    public void postfix_subtraction_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.postfix.subtraction", new PostfixSubtractionExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "prefix addition expression api model returns correct type")]
    public void prefix_addition_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.prefix.addition", new PrefixAdditionExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "prefix subtraction expression api model returns correct type")]
    public void prefix_subtraction_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.prefix.subtraction", new PrefixSubtractionExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "to bool expression api model returns correct type")]
    public void to_bool_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.toBool", new ToBoolExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "to number expression api model returns correct type")]
    public void to_number_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.toNumber", new ToNumberExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "to string expression api model returns correct type")]
    public void to_string_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.toString", new ToStringExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "typeof expression api model returns correct type")]
    public void typeof_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.typeof", new TypeofExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }
}