using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared.Expressions.Assignments;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.Expressions.Strategies;

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
        ValidateTargetIsNumberOrString();
        FailIfTargetValueWasUndefined();

        var targetType = GetTargetType();
        var offset = EvaluateOffset();
        var oldValue = GetTargetOldValue();

        return ApplyOperationAndStoreNewValue(targetType, oldValue, offset);
    }

    private DataType GetTargetType()
    {
        return scopeFacade.GetReferenceType(assignmentExpression.Target);
    }

    private void FailIfTargetValueWasUndefined()
    {
        if (scopeFacade.GetReference(assignmentExpression.Target).Equals(SpecialValues.UNDEFINED))
        {
            throw new OperationOnUndefinedValueException(assignmentExpression.Target);
        }
    }

    private object GetTargetOldValue()
    {
        return scopeFacade.GetReference(assignmentExpression.Target);
    }

    private EvaluatedResult ApplyOperationAndStoreNewValue(DataType targetType, object oldValue, EvaluatedResult offset)
    {
        var result = targetType.IsNumber()
            ? HandleNumberAssignmentExpression(oldValue, offset)
            : HandleStringAssignmentExpression(oldValue, offset);

        scopeFacade.UpdateReference(assignmentExpression.Target, result.Value);
        return result;
    }

    private EvaluatedResult HandleStringAssignmentExpression(object oldValue, EvaluatedResult offset)
    {
        return assignmentExpression switch
        {
            AdditionAssignment => HandleStringAdditionAssignmentExpression(oldValue, offset),
            MultiplicationAssignment when offset.Type.IsNumber() => HandleStringMultiplicationAssignmentExpression(
                oldValue, offset),
            _ => throw new IllegalStringAssignmentException()
        };
    }

    private static EvaluatedResult HandleStringAdditionAssignmentExpression(object oldValue, EvaluatedResult offset)
    {
        return new EvaluatedResult
        {
            Type = DataType.String(),
            Value = (string)oldValue + offset.Value
        };
    }

    private static EvaluatedResult HandleStringMultiplicationAssignmentExpression(object oldValue, EvaluatedResult offset)
    {
        ValidateNumberIsNatural(offset);
        ValidateNumberIsPositive(offset);

        return new EvaluatedResult
        {
            Type = DataType.String(),
            Value = string.Concat(Enumerable.Repeat((string)oldValue, Convert.ToInt32(offset.Value)))
        };
    }

    private static void ValidateNumberIsPositive(EvaluatedResult offset)
    {
        if ((double)offset.Value < 0)
        {
            throw new NumberNotPositiveException((double)offset.Value);
        }
    }

    private static void ValidateNumberIsNatural(EvaluatedResult offset)
    {
        if (!NumberUtils.IsNumberNatural((double)offset.Value))
        {
            throw new NumberNotNaturalException((double)offset.Value);
        }
    }

    private EvaluatedResult HandleNumberAssignmentExpression(object oldValue, EvaluatedResult offset)
    {
        if (!offset.Type.IsNumber())
        {
            throw new InvalidTypeException(offset.Type, DataType.Number());
        }

        return new EvaluatedResult
        {
            Type = DataType.Number(),
            Value = EvaluateNewNumberAssignment((double)oldValue, offset)
        };
    }

    private double EvaluateNewNumberAssignment(double oldValue, EvaluatedResult offset)
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
        if (!offset.Type.IsNumber() && !offset.Type.IsString())
        {
            throw new InvalidTypeException(offset.Type, DataType.Number(), DataType.String());
        }

        return offset;
    }

    private void ValidateTargetIsNumberOrString()
    {
        var targetType = scopeFacade.GetReferenceType(assignmentExpression.Target);
        if (!targetType.IsNumber() && !targetType.IsString())
        {
            throw new InvalidTypeException(targetType, DataType.Number(), DataType.String());
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