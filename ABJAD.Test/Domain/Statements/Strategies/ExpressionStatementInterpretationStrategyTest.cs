using ABJAD.Interpreter.Domain.Expressions;
using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Statements.Strategies;
using NSubstitute;

namespace ABJAD.Test.Domain.Statements.Strategies;

public class ExpressionStatementInterpretationStrategyTest
{
    private readonly IExpressionEvaluator expressionEvaluator = Substitute.For<IExpressionEvaluator>();

    [Fact(DisplayName = "delegates to the expression evaluator")]
    public void delegates_to_the_expression_evaluator()
    {
        var expression = Substitute.For<Expression>();
        var statement = new ExpressionStatement { Target = expression };
        var strategy = new ExpressionStatementInterpretationStrategy(statement, expressionEvaluator);
        strategy.Apply();
        expressionEvaluator.Received(1).Evaluate(expression);
    }

    [Fact(DisplayName = "returns a returning result with the flag set to false")]
    public void returns_a_returning_result_with_the_flag_set_to_false()
    {
        var expression = Substitute.For<Expression>();
        var statement = new ExpressionStatement { Target = expression };
        var strategy = new ExpressionStatementInterpretationStrategy(statement, expressionEvaluator);
        var result = strategy.Apply();
        Assert.False(result.Returned);
    }
}