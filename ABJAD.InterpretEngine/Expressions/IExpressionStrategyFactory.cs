using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;

namespace ABJAD.InterpretEngine.Expressions;

public interface IExpressionStrategyFactory
{
    ExpressionInterpretingStrategy GetAssignmentInterpretingStrategy(AssignmentExpression expression, Evaluator<Expression> expressionEvaluator);
    ExpressionInterpretingStrategy GetBinaryExpressionInterpretingStrategy(BinaryExpression expression, Evaluator<Expression> expressionEvaluator);
    ExpressionInterpretingStrategy GetFixesInterpretingStrategy(FixExpression expression);
    ExpressionInterpretingStrategy GetPrimitiveInterpretingStrategy(Primitive primitive);
    ExpressionInterpretingStrategy GetUnaryExpressionInterpretingStrategy(UnaryExpression expression, Evaluator<Expression> expressionEvaluator);
}