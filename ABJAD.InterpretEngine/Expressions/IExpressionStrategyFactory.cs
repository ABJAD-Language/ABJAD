using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;

namespace ABJAD.InterpretEngine.Expressions;

public interface IExpressionStrategyFactory
{
    ExpressionInterpretingStrategy GetAssignmentInterpretingStrategy(AssignmentExpression expression, Evaluator<Expression> expressionEvaluator, ScopeFacade scopeFacade);
    ExpressionInterpretingStrategy GetBinaryExpressionInterpretingStrategy(BinaryExpression expression, Evaluator<Expression> expressionEvaluator);
    ExpressionInterpretingStrategy GetFixesInterpretingStrategy(FixExpression expression, ScopeFacade scopeFacade);
    ExpressionInterpretingStrategy GetPrimitiveInterpretingStrategy(Primitive primitive, ScopeFacade scopeFacade);
    ExpressionInterpretingStrategy GetUnaryExpressionInterpretingStrategy(UnaryExpression expression, Evaluator<Expression> expressionEvaluator);
}