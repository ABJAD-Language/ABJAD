using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements.Strategies;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Statements.Strategies;

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
}