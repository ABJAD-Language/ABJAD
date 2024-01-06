using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared.Expressions.Fixes;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.Expressions.Strategies;

public class FixesEvaluationStrategy : ExpressionEvaluationStrategy
{
    private readonly FixExpression fixExpression;
    private readonly ScopeFacade scopeFacade;

    public FixesEvaluationStrategy(FixExpression fixExpression, ScopeFacade scopeFacade)
    {
        this.fixExpression = fixExpression;
        this.scopeFacade = scopeFacade;
    }

    public EvaluatedResult Apply()
    {
        ValidateTargetExists();
        ValidateTargetIsNumber();
        ValidateTargetValueIsNotUndefined();

        return fixExpression switch
        {
            AdditionPostfix => HandleAdditionPostfix(),
            AdditionPrefix => HandleAdditionPrefix(),
            SubtractionPostfix => HandleSubtractionPostfix(),
            SubtractionPrefix => HandleSubtractionPrefix(),
            _ => throw new ArgumentException()
        };
    }

    private EvaluatedResult HandleSubtractionPrefix()
    {
        var oldValue = (double)scopeFacade.GetReference(fixExpression.Target);
        scopeFacade.UpdateReference(fixExpression.Target, oldValue - 1);
        return new EvaluatedResult { Type = DataType.Number(), Value = oldValue - 1 };
    }

    private EvaluatedResult HandleSubtractionPostfix()
    {
        var oldValue = (double)scopeFacade.GetReference(fixExpression.Target);
        scopeFacade.UpdateReference(fixExpression.Target, oldValue - 1);
        return new EvaluatedResult { Type = DataType.Number(), Value = oldValue };
    }

    private EvaluatedResult HandleAdditionPrefix()
    {
        var oldValue = (double)scopeFacade.GetReference(fixExpression.Target);
        scopeFacade.UpdateReference(fixExpression.Target, oldValue + 1);
        return new EvaluatedResult { Type = DataType.Number(), Value = oldValue + 1 };
    }

    private EvaluatedResult HandleAdditionPostfix()
    {
        var oldValue = (double)scopeFacade.GetReference(fixExpression.Target);
        scopeFacade.UpdateReference(fixExpression.Target, oldValue + 1);
        return new EvaluatedResult { Type = DataType.Number(), Value = oldValue };
    }

    private void ValidateTargetValueIsNotUndefined()
    {
        if (scopeFacade.GetReference(fixExpression.Target).Equals(SpecialValues.UNDEFINED))
        {
            throw new OperationOnUndefinedValueException();
        }
    }

    private void ValidateTargetIsNumber()
    {
        var targetType = scopeFacade.GetReferenceType(fixExpression.Target);
        if (!targetType.IsNumber())
        {
            throw new InvalidTypeException(targetType, DataType.Number());
        }
    }

    private void ValidateTargetExists()
    {
        if (!scopeFacade.ReferenceExists(fixExpression.Target))
        {
            throw new ReferenceNameDoesNotExistException(fixExpression.Target);
        }
    }
}