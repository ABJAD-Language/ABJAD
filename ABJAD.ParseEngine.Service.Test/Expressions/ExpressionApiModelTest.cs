using ABJAD.ParseEngine.Service.Expressions;
using ABJAD.ParseEngine.Service.Expressions.Assignments;
using ABJAD.ParseEngine.Service.Expressions.Binary;
using ABJAD.ParseEngine.Service.Expressions.Unary;
using ABJAD.ParseEngine.Service.Primitives;
using Moq;

namespace ABJAD.ParseEngine.Service.Test.Expressions;

public class ExpressionApiModelTest
{
    [Fact(DisplayName = "instantiation expression api model returns correct type")]
    public void instantiation_expression_api_model_returns_correct_type()
    {
        var classNameExpression = new IdentifierPrimitiveApiModel("");
        Assert.Equal("expression.instantiation", new InstantiationExpressionApiModel(classNameExpression, new List<ExpressionApiModel>()).Type);
    }

    [Fact(DisplayName = "instance method call expression api model returns correct type")]
    public void instance_method_call_expression_api_model_returns_correct_type()
    {
        var methodNameExpression = new IdentifierPrimitiveApiModel("");
        Assert.Equal("expression.instanceMethodCall",
            new InstanceMethodCallExpressionApiModel(new List<PrimitiveExpressionApiModel>(), methodNameExpression,
                new List<ExpressionApiModel>()).Type);
    }

    [Fact(DisplayName = "instance field expression api model returns correct type")]
    public void instance_field_expression_api_model_returns_correct_type()
    {
        var classNameExpression = new IdentifierPrimitiveApiModel("");
        Assert.Equal("expression.instanceField", new InstanceFieldExpressionApiModel(classNameExpression, new List<PrimitiveExpressionApiModel>()).Type);
    }

    [Fact(DisplayName = "call expression api model returns correct type")]
    public void call_expression_api_model_returns_correct_type()
    {
        var methodNameExpression = new IdentifierPrimitiveApiModel("");
        Assert.Equal("expression.call", new CallExpressionApiModel(methodNameExpression, new List<ExpressionApiModel>()).Type);
    }

    [Fact(DisplayName = "addition assignment expression api model returns correct type")]
    public void addition_assignment_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.assignment.addition",
            new AdditionAssignmentExpressionApiModel("", new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "division assignment expression api model returns correct type")]
    public void division_assignment_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.assignment.division",
            new DivisionAssignmentExpressionApiModel("", new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "multiplication assignment expression api model returns correct type")]
    public void multiplication_assignment_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.assignment.multiplication",
            new MultiplicationAssignmentExpressionApiModel("", new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "subtraction assignment expression api model returns correct type")]
    public void subtraction_assignment_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.assignment.subtraction",
            new SubtractionAssignmentExpressionApiModel("", new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "negation expression api model returns correct type")]
    public void negation_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.unary.negation", new NegationExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "negative expression api model returns correct type")]
    public void negative_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.unary.negative", new NegativeExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "postfix addition expression api model returns correct type")]
    public void postfix_addition_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.unary.postfix.addition", new PostfixAdditionExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "postfix subtraction expression api model returns correct type")]
    public void postfix_subtraction_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.unary.postfix.subtraction", new PostfixSubtractionExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "prefix addition expression api model returns correct type")]
    public void prefix_addition_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.unary.prefix.addition", new PrefixAdditionExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "prefix subtraction expression api model returns correct type")]
    public void prefix_subtraction_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.unary.prefix.subtraction", new PrefixSubtractionExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "to bool expression api model returns correct type")]
    public void to_bool_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.unary.toBool", new ToBoolExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "to number expression api model returns correct type")]
    public void to_number_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.unary.toNumber", new ToNumberExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "to string expression api model returns correct type")]
    public void to_string_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.unary.toString", new ToStringExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "typeof expression api model returns correct type")]
    public void typeof_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.unary.typeOf", new TypeOfExpressionApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "addition expression api model returns correct type")]
    public void addition_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.addition", new AdditionExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "and operation expression api model returns correct type")]
    public void and_operation_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.and", new AndOperationExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "division expression api model returns correct type")]
    public void division_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.division", new DivisionExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "equality check expression api model returns correct type")]
    public void equality_check_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.equalityCheck", new EqualityCheckExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "greater check expression api model returns correct type")]
    public void greater_check_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.greaterCheck", new GreaterCheckExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "greater or equal check expression api model returns correct type")]
    public void greater_or_equal_check_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.greaterOrEqualCheck", new GreaterOrEqualCheckExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "less check expression api model returns correct type")]
    public void less_check_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.lessCheck", new LessCheckExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "less check or equal expression api model returns correct type")]
    public void less_or_equal_check_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.lessOrEqualCheck", new LessOrEqualCheckExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "inequality check expression api model returns correct type")]
    public void inequality_check_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.inequalityCheck", new InequalityCheckExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "modulo expression api model returns correct type")]
    public void modulo_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.modulo", new ModuloExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "multiplication expression api model returns correct type")]
    public void multiplication_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.multiplication", new MultiplicationExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "subtraction expression api model returns correct type")]
    public void subtraction_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.subtraction", new SubtractionExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "or operation expression api model returns correct type")]
    public void or_operation_expression_api_model_returns_correct_type()
    {
        Assert.Equal("expression.binary.or", new OrOperationExpressionApiModel(new Mock<ExpressionApiModel>().Object, new Mock<ExpressionApiModel>().Object).Type);
    }
}