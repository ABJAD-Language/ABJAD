using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;

namespace ABJAD.InterpretEngine.Expressions;

public class ExpressionEvaluator : Evaluator<Expression>
{
    private readonly IExpressionStrategyFactory expressionStrategyFactory;

    public ExpressionEvaluator(IExpressionStrategyFactory expressionStrategyFactory)
    {
        this.expressionStrategyFactory = expressionStrategyFactory;
    }

    public EvaluatedResult Evaluate(Expression target)
    {
        return target switch
        {
            AssignmentExpression expression => expressionStrategyFactory.GetAssignmentInterpretingStrategy(expression, this).Apply(),
            BinaryExpression expression => expressionStrategyFactory.GetBinaryExpressionInterpretingStrategy(expression, this).Apply(),
            FixExpression expression => expressionStrategyFactory.GetFixesInterpretingStrategy(expression).Apply(),
            UnaryExpression expression => expressionStrategyFactory.GetUnaryExpressionInterpretingStrategy(expression, this).Apply(),
            _ => throw new ArgumentException()
        };
    }
}