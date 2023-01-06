using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions.Strategies;

public class AssignmentEvaluationStrategyTest
{
    private readonly ScopeFacade scopeFacade;
    private readonly IExpressionEvaluator expressionEvaluator;

    public AssignmentEvaluationStrategyTest()
    {
        scopeFacade = Substitute.For<ScopeFacade>();
        expressionEvaluator = Substitute.For<IExpressionEvaluator>();
    }

    [Fact(DisplayName = "throws error if the target reference did not exist")]
    public void throws_error_if_the_target_reference_did_not_exist()
    {
        scopeFacade.ReferenceExists("id").Returns(false);
        var strategy = new AssignmentEvaluationStrategy(new AdditionAssignment { Target = "id" }, scopeFacade, expressionEvaluator);
        Assert.Throws<ReferenceNameDoesNotExistException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the target was not of type number")]
    public void throws_error_if_the_target_was_not_of_type_number()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        var targetType = Substitute.For<DataType>();
        targetType.IsNumber().Returns(false);
        targetType.GetValue().Returns("notNumber");
        scopeFacade.GetReferenceType("id").Returns(targetType);
        var strategy = new AssignmentEvaluationStrategy(new AdditionAssignment { Target = "id" }, scopeFacade, expressionEvaluator);
        Assert.Throws<InvalidTypeException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the target value was undefined")]
    public void throws_error_if_the_target_value_was_undefined()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        var targetType = Substitute.For<DataType>();
        targetType.IsNumber().Returns(true);
        scopeFacade.GetReferenceType("id").Returns(targetType);
        scopeFacade.GetReference("id").Returns(SpecialValues.UNDEFINED);
        var strategy = new AssignmentEvaluationStrategy(new AdditionAssignment { Target = "id" }, scopeFacade, expressionEvaluator);
        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the offset was not of type number")]
    public void throws_error_if_the_offset_was_not_of_type_number()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        scopeFacade.GetReferenceType("id").Returns(DataType.Number());
        scopeFacade.GetReference("id").Returns(1.0);

        var expression = Substitute.For<Expression>();
        var expressionType = Substitute.For<DataType>();
        expressionType.IsNumber().Returns(false);
        expressionEvaluator.Evaluate(expression).Returns(new EvaluatedResult { Type = expressionType });

        var assignmentExpression = new AdditionAssignment { Target = "id", Value = expression };
        var strategy = new AssignmentEvaluationStrategy(assignmentExpression, scopeFacade, expressionEvaluator);

        Assert.Throws<InvalidTypeException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "updates the value of target correctly and return the result when the expression is addition assignment")]
    public void updates_the_value_of_target_correctly_and_return_the_result_when_the_expression_is_addition_assignment()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        scopeFacade.GetReference("id").Returns(2.0);
        scopeFacade.GetReferenceType("id").Returns(DataType.Number());

        var offset = Substitute.For<Expression>();

        expressionEvaluator.Evaluate(offset).Returns(new EvaluatedResult { Type = DataType.Number(), Value = 3.0});

        var assignmentExpression = new AdditionAssignment { Target = "id", Value = offset };
        var strategy = new AssignmentEvaluationStrategy(assignmentExpression, scopeFacade, expressionEvaluator);
        var result = strategy.Apply();

        scopeFacade.Received(1).UpdateReference("id", 5.0);
            
        Assert.True(result.Type.IsNumber());
        Assert.Equal(5.0, result.Value);
    }
    
    [Fact(DisplayName = "updates the value of target correctly and return the result when the expression is subtraction assignment")]
    public void updates_the_value_of_target_correctly_and_return_the_result_when_the_expression_is_subtraction_assignment()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        scopeFacade.GetReference("id").Returns(7.0);
        scopeFacade.GetReferenceType("id").Returns(DataType.Number());

        var offset = Substitute.For<Expression>();

        expressionEvaluator.Evaluate(offset).Returns(new EvaluatedResult { Type = DataType.Number(), Value = 4.0});

        var assignmentExpression = new SubtractionAssignment { Target = "id", Value = offset };
        var strategy = new AssignmentEvaluationStrategy(assignmentExpression, scopeFacade, expressionEvaluator);
        var result = strategy.Apply();

        scopeFacade.Received(1).UpdateReference("id", 3.0);
            
        Assert.True(result.Type.IsNumber());
        Assert.Equal(3.0, result.Value);
    }
    
    [Fact(DisplayName = "updates the value of target correctly and return the result when the expression is multiplication assignment")]
    public void updates_the_value_of_target_correctly_and_return_the_result_when_the_expression_is_multiplication_assignment()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        scopeFacade.GetReference("id").Returns(8.0);
        scopeFacade.GetReferenceType("id").Returns(DataType.Number());

        var offset = Substitute.For<Expression>();

        expressionEvaluator.Evaluate(offset).Returns(new EvaluatedResult { Type = DataType.Number(), Value = -1.0});

        var assignmentExpression = new MultiplicationAssignment { Target = "id", Value = offset };
        var strategy = new AssignmentEvaluationStrategy(assignmentExpression, scopeFacade, expressionEvaluator);
        var result = strategy.Apply();

        scopeFacade.Received(1).UpdateReference("id", -8.0);
            
        Assert.True(result.Type.IsNumber());
        Assert.Equal(-8.0, result.Value);
    }
    
    [Fact(DisplayName = "updates the value of target correctly and return the result when the expression is division assignment")]
    public void updates_the_value_of_target_correctly_and_return_the_result_when_the_expression_is_division_assignment()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        scopeFacade.GetReference("id").Returns(9.0);
        scopeFacade.GetReferenceType("id").Returns(DataType.Number());

        var offset = Substitute.For<Expression>();

        expressionEvaluator.Evaluate(offset).Returns(new EvaluatedResult { Type = DataType.Number(), Value = 2.0});

        var assignmentExpression = new DivisionAssignment { Target = "id", Value = offset };
        var strategy = new AssignmentEvaluationStrategy(assignmentExpression, scopeFacade, expressionEvaluator);
        var result = strategy.Apply();

        scopeFacade.Received(1).UpdateReference("id", 4.5);
            
        Assert.True(result.Type.IsNumber());
        Assert.Equal(4.5, result.Value);
    }
}