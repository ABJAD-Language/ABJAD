using ABJAD.Interpreter.Domain;
using ABJAD.Interpreter.Domain.Expressions;
using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Statements.Strategies;
using ABJAD.Interpreter.Domain.Types;
using NSubstitute;

namespace ABJAD.Test.Domain.Statements.Strategies;

public class AssignmentInterpretationStrategyTest
{
    private readonly IExpressionEvaluator expressionEvaluator = Substitute.For<IExpressionEvaluator>();
    private readonly ScopeFacade scopeFacade = Substitute.For<ScopeFacade>();

    [Fact(DisplayName = "throws error if target reference does not exist in scope")]
    public void throws_error_if_target_reference_does_not_exist_in_scope()
    {
        scopeFacade.ReferenceExists("id").Returns(false);
        var assignment = new Assignment { Target = "id" };
        var strategy = new AssignmentInterpretationStrategy(assignment, scopeFacade, expressionEvaluator);
        Assert.Throws<ReferenceNameDoesNotExistException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if type of target does not match with the type of the value")]
    public void throws_error_if_type_of_target_does_not_match_with_the_type_of_the_value()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        var targetType = Substitute.For<DataType>();
        scopeFacade.GetReferenceType("id").Returns(targetType);
        var value = Substitute.For<Expression>();
        var valueType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(value).Returns(new EvaluatedResult { Type = valueType });
        targetType.Is(valueType).Returns(false);

        var assignment = new Assignment { Target = "id", Value = value };
        var strategy = new AssignmentInterpretationStrategy(assignment, scopeFacade, expressionEvaluator);
        Assert.Throws<IncompatibleTypesException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "does not throw an error if the type of target is string and the value is null")]
    public void does_not_throw_an_error_if_the_type_of_target_is_string_and_the_value_is_null()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        scopeFacade.GetReferenceType("id").Returns(DataType.String());
        var value = Substitute.For<Expression>();
        expressionEvaluator.Evaluate(value).Returns(new EvaluatedResult { Type = DataType.Undefined(), Value = SpecialValues.NULL });

        var assignment = new Assignment { Target = "id", Value = value };
        var strategy = new AssignmentInterpretationStrategy(assignment, scopeFacade, expressionEvaluator);
        strategy.Apply();
    }

    [Fact(DisplayName = "does not throw an error if the type of target is custom and the value is null")]
    public void does_not_throw_an_error_if_the_type_of_target_is_custom_and_the_value_is_null()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        scopeFacade.GetReferenceType("id").Returns(DataType.Custom("type"));
        var value = Substitute.For<Expression>();
        expressionEvaluator.Evaluate(value).Returns(new EvaluatedResult { Type = DataType.Undefined(), Value = SpecialValues.NULL });

        var assignment = new Assignment { Target = "id", Value = value };
        var strategy = new AssignmentInterpretationStrategy(assignment, scopeFacade, expressionEvaluator);
        strategy.Apply();
    }

    [Fact(DisplayName = "throws error if the type of target is number and value is null")]
    public void throws_error_if_the_type_of_target_is_number_and_value_is_null()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        scopeFacade.GetReferenceType("id").Returns(DataType.Number());
        var value = Substitute.For<Expression>();
        expressionEvaluator.Evaluate(value).Returns(new EvaluatedResult { Type = DataType.Undefined(), Value = SpecialValues.NULL });

        var assignment = new Assignment { Target = "id", Value = value };
        var strategy = new AssignmentInterpretationStrategy(assignment, scopeFacade, expressionEvaluator);
        Assert.Throws<IllegalNullAssignmentException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the type of target is bool and value is null")]
    public void throws_error_if_the_type_of_target_is_bool_and_value_is_null()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        scopeFacade.GetReferenceType("id").Returns(DataType.Bool());
        var value = Substitute.For<Expression>();
        expressionEvaluator.Evaluate(value).Returns(new EvaluatedResult { Type = DataType.Undefined(), Value = SpecialValues.NULL });

        var assignment = new Assignment { Target = "id", Value = value };
        var strategy = new AssignmentInterpretationStrategy(assignment, scopeFacade, expressionEvaluator);
        Assert.Throws<IllegalNullAssignmentException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the value to be assigned evaluates to undefined")]
    public void throws_error_if_the_value_to_be_assigned_evaluates_to_undefined()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        var targetType = Substitute.For<DataType>();
        scopeFacade.GetReferenceType("id").Returns(targetType);
        var value = Substitute.For<Expression>();
        var valueType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(value).Returns(new EvaluatedResult { Type = valueType, Value = SpecialValues.UNDEFINED });
        targetType.Is(valueType).Returns(true);

        var assignment = new Assignment { Target = "id", Value = value };
        var strategy = new AssignmentInterpretationStrategy(assignment, scopeFacade, expressionEvaluator);
        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "updates the value of target in the scope")]
    public void updates_the_value_of_target_in_the_scope()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        var targetType = Substitute.For<DataType>();
        scopeFacade.GetReferenceType("id").Returns(targetType);
        var valueExpression = Substitute.For<Expression>();
        var valueType = Substitute.For<DataType>();
        var newValue = new object();
        expressionEvaluator.Evaluate(valueExpression).Returns(new EvaluatedResult { Type = valueType, Value = newValue });
        targetType.Is(valueType).Returns(true);

        var assignment = new Assignment { Target = "id", Value = valueExpression };
        var strategy = new AssignmentInterpretationStrategy(assignment, scopeFacade, expressionEvaluator);
        strategy.Apply();
        scopeFacade.Received(1).UpdateReference("id", newValue);
    }

    [Fact(DisplayName = "returns a returning result with the flag set to false")]
    public void returns_a_returning_result_with_the_flag_set_to_false()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        var targetType = Substitute.For<DataType>();
        scopeFacade.GetReferenceType("id").Returns(targetType);
        var valueExpression = Substitute.For<Expression>();
        var valueType = Substitute.For<DataType>();
        var newValue = new object();
        expressionEvaluator.Evaluate(valueExpression).Returns(new EvaluatedResult { Type = valueType, Value = newValue });
        targetType.Is(valueType).Returns(true);

        var assignment = new Assignment { Target = "id", Value = valueExpression };
        var strategy = new AssignmentInterpretationStrategy(assignment, scopeFacade, expressionEvaluator);
        var result = strategy.Apply();

        Assert.False(result.Returned);
    }
}