using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Expressions.Strategies;

public class AssignmentEvaluationStrategy : ExpressionEvaluationStrategy
{
    private readonly AssignmentExpression assignmentExpression;
    private readonly ScopeFacade scopeFacade;
    private readonly IExpressionEvaluator expressionEvaluator;

    public AssignmentEvaluationStrategy(AssignmentExpression assignmentExpression, ScopeFacade scopeFacade, IExpressionEvaluator expressionEvaluator)
    {
        this.assignmentExpression = assignmentExpression;
        this.scopeFacade = scopeFacade;
        this.expressionEvaluator = expressionEvaluator;
    }

    public EvaluatedResult Apply()
    {
        FailIfReferenceDoesNotExist();
        FailIfTargetIsNotNumber();
        FailIfTargetValueWasUndefined();
        
        var offset = EvaluateOffset();
        var oldValue = GetTargetOldValue();
        
        return ApplyOperationAndStoreNewValue(oldValue, offset);
    }

    private void FailIfTargetValueWasUndefined()
    {
        if (scopeFacade.GetReference(assignmentExpression.Target).Equals(SpecialValues.UNDEFINED))
        {
            throw new OperationOnUndefinedValueException(assignmentExpression.Target);
        }
    }

    private double GetTargetOldValue()
    {
        return (double)scopeFacade.GetReference(assignmentExpression.Target);
    }

    private EvaluatedResult ApplyOperationAndStoreNewValue(double oldValue, EvaluatedResult offset)
    {
        var newValue = EvaluateNewValue(oldValue, offset);
        scopeFacade.UpdateReference(assignmentExpression.Target, newValue);
        return new EvaluatedResult { Type = DataType.Number(), Value = newValue };
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
            throw new InvalidTypeException(offset.Type, DataType.Number());
        }

        return offset;
    }

    private void FailIfTargetIsNotNumber()
    {
        var targetType = scopeFacade.GetReferenceType(assignmentExpression.Target);
        if (!targetType.IsNumber())
        {
            throw new InvalidTypeException(targetType, DataType.Number());
        }
    }

    private void FailIfReferenceDoesNotExist()
    {
        if (!scopeFacade.ReferenceExists(assignmentExpression.Target))
        {
            throw new ReferenceNameDoesNotExistException(assignmentExpression.Target);
        }
    }
}