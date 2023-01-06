using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions.Strategies;

public class InstanceFieldMethodCallEvaluationStrategyTest
{

    private readonly IExpressionEvaluator expressionEvaluator = Substitute.For<IExpressionEvaluator>();
    private readonly ScopeFacade scope = Substitute.For<ScopeFacade>();
    private readonly IExpressionEvaluatorFactory expressionEvaluatorFactory = Substitute.For<IExpressionEvaluatorFactory>();
    private readonly TextWriter writer = Substitute.For<TextWriter>();

    [Fact(DisplayName = "delegates to the expression evaluator to retrieve the child instance when instances are more than one")]
    public void delegates_to_the_expression_evaluator_to_retrieve_the_child_instance_when_instances_are_more_than_one()
    {
        var instanceMethodCall = new InstanceMethodCall() { Instances = new List<string> { "instance1", "instance2", "instance3" } };
        var childType = Substitute.For<DataType>();
        expressionEvaluator.Evaluate(Arg.Any<InstanceFieldAccess>()).Returns(new EvaluatedResult()
            { Type = childType, Value = new InstanceElement() });
        var strategy = new InstanceFieldMethodCallEvaluationStrategy(instanceMethodCall, scope, expressionEvaluator, expressionEvaluatorFactory, writer);
        strategy.Apply();
        expressionEvaluator.Received(1).Evaluate(Arg.Compat.Is<InstanceFieldAccess>(instanceFieldAccess =>
            instanceFieldAccess.Instance == "instance1" &&
            instanceFieldAccess.NestedFields.Count == 2 &&
            instanceFieldAccess.NestedFields[0] == "instance2" &&
            instanceFieldAccess.NestedFields[1] == "instance3"));
    }

    [Fact(DisplayName = "throws error if the child instance is not an instance")]
    public void throws_error_if_the_child_instance_is_not_an_instance()
    {
        var instanceMethodCall = new InstanceMethodCall() { Instances = new List<string> { "instance1", "instance2", "instance3" } };
        expressionEvaluator.Evaluate(Arg.Any<InstanceFieldAccess>()).Returns(new EvaluatedResult()
            { Type = Substitute.For<DataType>(), Value = new object() });
        var strategy = new InstanceFieldMethodCallEvaluationStrategy(instanceMethodCall, scope, expressionEvaluator, expressionEvaluatorFactory, writer);
        Assert.Throws<NotInstanceReferenceException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the parent reference does not exist when one is passed")]
    public void throws_error_if_the_parent_reference_does_not_exist_when_one_is_passed()
    {
        var instanceMethodCall = new InstanceMethodCall() { Instances = new List<string> { "instance" } };
        scope.ReferenceExists("instance").Returns(false);
        var strategy = new InstanceFieldMethodCallEvaluationStrategy(instanceMethodCall, scope, expressionEvaluator, expressionEvaluatorFactory, writer);
        Assert.Throws<ReferenceNameDoesNotExistException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if the parent reference is not an instance when one is only passed")]
    public void throws_error_if_the_parent_reference_is_not_an_instance_when_one_is_only_passed()
    {
        var instanceMethodCall = new InstanceMethodCall() { Instances = new List<string> { "instance" } };
        scope.ReferenceExists("instance").Returns(true);
        scope.GetReference("instance").Returns(new object());
        var strategy = new InstanceFieldMethodCallEvaluationStrategy(instanceMethodCall, scope, expressionEvaluator, expressionEvaluatorFactory, writer);
        Assert.Throws<NotInstanceReferenceException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if no instances were passed")]
    public void throws_error_if_no_instances_were_passed()
    {
        var instanceMethodCall = new InstanceMethodCall() { Instances = new List<string>() };
        var strategy = new InstanceFieldMethodCallEvaluationStrategy(instanceMethodCall, scope, expressionEvaluator, expressionEvaluatorFactory, writer);
        Assert.Throws<ArgumentException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "delegates to the expression evaluator to evaluate a method call")]
    public void delegates_to_the_expression_evaluator_to_evaluate_a_method_call()
    {
        var arg1 = Substitute.For<Expression>();
        var arg2 = Substitute.For<Expression>();
        var instanceMethodCall = new InstanceMethodCall()
        {
            Instances = new List<string> { "instance" },
            MethodName = "method",
            Arguments = new List<Expression> { arg1, arg2 }
        };
        
        scope.ReferenceExists("instance").Returns(true);
        
        var instanceScope = Substitute.For<ScopeFacade>();
        var instance = new InstanceElement() { Scope = instanceScope} ;
        scope.GetReference("instance").Returns(instance);

        var localExpressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluatorFactory.NewExpressionEvaluator(scope, writer).Returns(localExpressionEvaluator);

        var strategy = new InstanceFieldMethodCallEvaluationStrategy(instanceMethodCall, scope, expressionEvaluator, expressionEvaluatorFactory, writer);
        strategy.Apply();
        
        Received.InOrder(() =>
        {
            scope.AddScope(instanceScope);
            localExpressionEvaluator.Evaluate(Arg.Is<MethodCall>(call =>
                call.MethodName == "method" && 
                call.Arguments.Count == 2 && 
                call.Arguments[0] == arg1 &&
                call.Arguments[1] == arg2));
            scope.RemoveLastScope();
        });
    }

    [Fact(DisplayName = "returns the value returned from the expression evaluator evaluating the method call")]
    public void returns_the_value_returned_from_the_expression_evaluator_evaluating_the_method_call()
    {
        var instanceMethodCall = new InstanceMethodCall()
        {
            Instances = new List<string> { "instance" },
            MethodName = "method",
            Arguments = new List<Expression>()
        };
        
        scope.ReferenceExists("instance").Returns(true);
        
        var instanceScope = Substitute.For<ScopeFacade>();
        var instance = new InstanceElement() { Scope = instanceScope} ;
        scope.GetReference("instance").Returns(instance);

        var localExpressionEvaluator = Substitute.For<IExpressionEvaluator>();
        expressionEvaluatorFactory.NewExpressionEvaluator(scope, writer).Returns(localExpressionEvaluator);

        var evaluatedResult = new EvaluatedResult();
        localExpressionEvaluator.Evaluate(Arg.Any<MethodCall>()).Returns(evaluatedResult);
        
        var strategy = new InstanceFieldMethodCallEvaluationStrategy(instanceMethodCall, scope, expressionEvaluator, expressionEvaluatorFactory, writer);
        Assert.Equal(evaluatedResult, strategy.Apply());
    }

}