using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Expressions.Strategies;

public class AssignmentInterpretingStrategy : ExpressionInterpretingStrategy
{
    private readonly AssignmentExpression assignmentExpression;
    private readonly IScope scope;
    private readonly Evaluator<Expression> expressionEvaluator;

    public AssignmentInterpretingStrategy(AssignmentExpression assignmentExpression, IScope scope, Evaluator<Expression> expressionEvaluator)
    {
        this.assignmentExpression = assignmentExpression;
        this.scope = scope;
        this.expressionEvaluator = expressionEvaluator;
    }

    public object Apply()
    {
        FailIfReferenceDoesNotExist();
        FailIfTargetIsNotNumber();

        var offset = EvaluateOffset();
        var oldValue = GetTargetOldValue();
        
        return ApplyOperationAndStoreNewValue(oldValue, offset);
    }

    private double GetTargetOldValue()
    {
        return (double)scope.Get(assignmentExpression.Target);
    }

    private double ApplyOperationAndStoreNewValue(double oldValue, EvaluatedResult offset)
    {
        var newValue = EvaluateNewValue(oldValue, offset);
        scope.Set(assignmentExpression.Target, newValue);
        return newValue;
    }

    private double EvaluateNewValue(double oldValue, EvaluatedResult offset)
    {
        return assignmentExpression switch
        {
            AdditionAssignment => oldValue + (double)offset.Value,
            SubtractionAssignment => oldValue - (double)offset.Value,
            MultiplicationAssignment => oldValue * (double)offset.Value,
            DivisionAssignment => oldValue / (double)offset.Value,
            _ => throw new ArgumentException()
        };
    }

    private EvaluatedResult EvaluateOffset()
    {
        var offset = expressionEvaluator.Evaluate(assignmentExpression.Value);
        if (!offset.Type.IsNumber())
        {
            throw new InvalidTypeException(DataType.Number(), offset.Type);
        }

        return offset;
    }

    private void FailIfTargetIsNotNumber()
    {
        var targetType = scope.GetType(assignmentExpression.Target);
        if (!targetType.IsNumber())
        {
            throw new InvalidTypeException(DataType.Number(), targetType);
        }
    }

    private void FailIfReferenceDoesNotExist()
    {
        if (!scope.ReferenceExists(assignmentExpression.Target))
        {
            throw new ReferenceNameDoesNotExistException(assignmentExpression.Target);
        }
    }
}