using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;

namespace ABJAD.InterpretEngine.Expressions;

public class ExpressionStrategyFactory : IExpressionStrategyFactory
{
    public ExpressionInterpretingStrategy GetAssignmentInterpretingStrategy(AssignmentExpression expression, Evaluator<Expression> expressionEvaluator, ScopeFacade scopeFacade)
    {
        return new AssignmentInterpretingStrategy(expression, scopeFacade, expressionEvaluator);
    }

    public ExpressionInterpretingStrategy GetBinaryExpressionInterpretingStrategy(BinaryExpression expression, Evaluator<Expression> expressionEvaluator)
    {
        return new BinaryExpressionInterpretingStrategy(expression, expressionEvaluator);
    }

    public ExpressionInterpretingStrategy GetFixesInterpretingStrategy(FixExpression expression, ScopeFacade scopeFacade)
    {
        return new FixesInterpretingStrategy(expression, scopeFacade);
    }

    public ExpressionInterpretingStrategy GetPrimitiveInterpretingStrategy(Primitive primitive, ScopeFacade scopeFacade)
    {
        return new PrimitiveInterpretingStrategy(primitive, scopeFacade);
    }

    public ExpressionInterpretingStrategy GetUnaryExpressionInterpretingStrategy(UnaryExpression expression,
        Evaluator<Expression> expressionEvaluator)
    {
        return new UnaryExpressionInterpretingStrategy(expression, expressionEvaluator);
    }
}