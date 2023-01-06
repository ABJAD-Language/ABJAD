using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements;
using ABJAD.InterpretEngine.Statements.Strategies;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Statements.Strategies;

public class ReturnInterpretationStrategyTest
{
    private readonly IExpressionEvaluator expressionEvaluator = Substitute.For<IExpressionEvaluator>();

    [Fact(DisplayName = "throws an error when function context is set to false")]
    public void throws_an_error_when_function_context_is_set_to_false()
    {
        var strategy = new ReturnInterpretationStrategy(new Return(), false, expressionEvaluator);
        Assert.Throws<IllegalUseOfReturnStatementException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "returns the target value when an expression is passed")]
    public void returns_the_target_value_when_an_expression_is_passed()
    {
        var target = Substitute.For<Expression>();
        var evaluatedResult = new EvaluatedResult();
        expressionEvaluator.Evaluate(target).Returns(evaluatedResult);
        
        var statement = new Return() { Target = target };
        var strategy = new ReturnInterpretationStrategy(statement, true, expressionEvaluator);
        var result = strategy.Apply();
        
        Assert.True(result.Returned);
        Assert.True(result.IsValueReturned);
        Assert.Equal(evaluatedResult, result.ReturnedValue);
    }

    [Fact(DisplayName = "returns an empty result when no target expression is passed")]
    public void returns_an_empty_result_when_no_target_expression_is_passed()
    {
        var strategy = new ReturnInterpretationStrategy(new Return(), true, expressionEvaluator);
        var result = strategy.Apply();

        expressionEvaluator.DidNotReceiveWithAnyArgs().Evaluate(Arg.Any<Expression>());
        Assert.True(result.Returned);
        Assert.False(result.IsValueReturned);
    }

}