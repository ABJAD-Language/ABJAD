using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;

namespace ABJAD.InterpretEngine.Expressions;

public class ExpressionEvaluator : Evaluator<Expression>
{
    private readonly IExpressionStrategyFactory expressionStrategyFactory;
    private readonly ScopeFacade scopeFacade;

    public ExpressionEvaluator(IExpressionStrategyFactory expressionStrategyFactory, ScopeFacade scopeFacade)
    {
        this.expressionStrategyFactory = expressionStrategyFactory;
        this.scopeFacade = scopeFacade;
    }

    public EvaluatedResult Evaluate(Expression target)
    {
        return target switch
        {
            AssignmentExpression expression => expressionStrategyFactory.GetAssignmentInterpretingStrategy(expression, this, scopeFacade).Apply(),
            BinaryExpression expression => expressionStrategyFactory.GetBinaryExpressionInterpretingStrategy(expression, this).Apply(),
            FixExpression expression => expressionStrategyFactory.GetFixesInterpretingStrategy(expression, scopeFacade).Apply(),
            UnaryExpression expression => expressionStrategyFactory.GetUnaryExpressionInterpretingStrategy(expression, this).Apply(),
            Primitive primitive => expressionStrategyFactory.GetPrimitiveInterpretingStrategy(primitive, scopeFacade).Apply(),
            _ => throw new ArgumentException()
        };
    }
}