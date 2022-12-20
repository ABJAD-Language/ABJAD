using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements.Strategies;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Statements.Strategies;

public class AssignmentInterpretationStrategyTest
{
    private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
    private readonly IScope scope = Substitute.For<IScope>();

    [Fact(DisplayName = "throws error if target reference does not exist in scope")]
    public void throws_error_if_target_reference_does_not_exist_in_scope()
    {
        scope.ReferenceExists("id").Returns(false);
        var assignment = new Assignment { Target = "id" };
        var strategy = new AssignmentInterpretationStrategy(assignment, scope, expressionEvaluator);
        Assert.Throws<ReferenceNameDoesNotExistException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if type of target does not match with the type of the value")]
    public void throws_error_if_type_of_target_does_not_match_with_the_type_of_the_value()
    {
        scope.ReferenceExists("id").Returns(true);
        var targetType = Substitute.For<DataType>();
        scope.GetType("id").Returns(targetType);
        var value = Substitute.For<Expression>();
        var valueType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(value).Returns(new EvaluatedResult { Type = valueType });
        targetType.Is(valueType).Returns(false);

        var assignment = new Assignment { Target = "id", Value = value };
        var strategy = new AssignmentInterpretationStrategy(assignment, scope, expressionEvaluator);
        Assert.Throws<IncompatibleTypesException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the value to be assigned evaluates to undefined")]
    public void throws_error_if_the_value_to_be_assigned_evaluates_to_undefined()
    {
        scope.ReferenceExists("id").Returns(true);
        var targetType = Substitute.For<DataType>();
        scope.GetType("id").Returns(targetType);
        var value = Substitute.For<Expression>();
        var valueType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(value).Returns(new EvaluatedResult { Type = valueType, Value = SpecialValues.UNDEFINED });
        targetType.Is(valueType).Returns(true);

        var assignment = new Assignment { Target = "id", Value = value };
        var strategy = new AssignmentInterpretationStrategy(assignment, scope, expressionEvaluator);
        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "updates the value of target in the scope")]
    public void updates_the_value_of_target_in_the_scope()
    {
        scope.ReferenceExists("id").Returns(true);
        var targetType = Substitute.For<DataType>();
        scope.GetType("id").Returns(targetType);
        var valueExpression = Substitute.For<Expression>();
        var valueType = Substitute.For<DataType>();
        var newValue = new object();
        expressionEvaluator.Evaluate(valueExpression).Returns(new EvaluatedResult { Type = valueType, Value = newValue });
        targetType.Is(valueType).Returns(true);

        var assignment = new Assignment { Target = "id", Value = valueExpression };
        var strategy = new AssignmentInterpretationStrategy(assignment, scope, expressionEvaluator);
        strategy.Apply();
        scope.Received(1).Set("id", newValue);
    }
}