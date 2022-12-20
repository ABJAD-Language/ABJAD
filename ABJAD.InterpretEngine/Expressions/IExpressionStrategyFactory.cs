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
    ExpressionEvaluationStrategy GetAssignmentEvaluationStrategy(AssignmentExpression expression, Evaluator<Expression> expressionEvaluator, ScopeFacade scopeFacade);
    ExpressionEvaluationStrategy GetBinaryExpressionEvaluationStrategy(BinaryExpression expression, Evaluator<Expression> expressionEvaluator);
    ExpressionEvaluationStrategy GetFixesEvaluationStrategy(FixExpression expression, ScopeFacade scopeFacade);
    ExpressionEvaluationStrategy GetPrimitiveEvaluationStrategy(Primitive primitive, ScopeFacade scopeFacade);
    ExpressionEvaluationStrategy GetUnaryExpressionEvaluationStrategy(UnaryExpression expression, Evaluator<Expression> expressionEvaluator);
}