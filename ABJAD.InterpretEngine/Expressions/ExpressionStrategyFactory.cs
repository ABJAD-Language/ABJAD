using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;

namespace ABJAD.InterpretEngine.Expressions;

public class ExpressionStrategyFactory : IExpressionStrategyFactory
{
    private readonly IScope scope;

    public ExpressionStrategyFactory(IScope scope)
    {
        this.scope = scope;
    }

    public ExpressionInterpretingStrategy GetAssignmentInterpretingStrategy(AssignmentExpression expression, 
        Evaluator<Expression> expressionEvaluator)
    {
        return new AssignmentInterpretingStrategy(expression, scope, expressionEvaluator);
    }

    public ExpressionInterpretingStrategy GetBinaryExpressionInterpretingStrategy(BinaryExpression expression,
        Evaluator<Expression> expressionEvaluator)
    {
        return new BinaryExpressionInterpretingStrategy(expression, expressionEvaluator);
    }

    public ExpressionInterpretingStrategy GetFixesInterpretingStrategy(FixExpression expression)
    {
        return new FixesInterpretingStrategy(expression, scope);
    }

    public ExpressionInterpretingStrategy GetPrimitiveInterpretingStrategy(Primitive primitive)
    {
        return new PrimitiveInterpretingStrategy(primitive, scope);
    }

    public ExpressionInterpretingStrategy GetUnaryExpressionInterpretingStrategy(UnaryExpression expression,
        Evaluator<Expression> expressionEvaluator)
    {
        return new UnaryExpressionInterpretingStrategy(expression, expressionEvaluator);
    }
}