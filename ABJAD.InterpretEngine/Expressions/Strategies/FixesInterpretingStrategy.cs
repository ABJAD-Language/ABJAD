﻿using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Expressions.Strategies;

public class FixesInterpretingStrategy : ExpressionInterpretingStrategy
{
    private readonly FixExpression fixExpression;
    private readonly ScopeFacade scopeFacade;

    public FixesInterpretingStrategy(FixExpression fixExpression, ScopeFacade scopeFacade)
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
        var oldValue = (double)scopeFacade.Get(fixExpression.Target.Value);
        scopeFacade.Set(fixExpression.Target.Value, oldValue - 1);
        return new EvaluatedResult { Type = DataType.Number(), Value = oldValue - 1 };
    }

    private EvaluatedResult HandleSubtractionPostfix()
    {
        var oldValue = (double)scopeFacade.Get(fixExpression.Target.Value);
        scopeFacade.Set(fixExpression.Target.Value, oldValue - 1);
        return new EvaluatedResult { Type = DataType.Number(), Value = oldValue };
    }

    private EvaluatedResult HandleAdditionPrefix()
    {
        var oldValue = (double) scopeFacade.Get(fixExpression.Target.Value);
        scopeFacade.Set(fixExpression.Target.Value, oldValue + 1);
        return new EvaluatedResult { Type = DataType.Number(), Value = oldValue + 1 };
    }

    private EvaluatedResult HandleAdditionPostfix()
    {
        var oldValue = (double) scopeFacade.Get(fixExpression.Target.Value);
        scopeFacade.Set(fixExpression.Target.Value, oldValue + 1);
        return new EvaluatedResult { Type = DataType.Number(), Value = oldValue };
    }

    private void ValidateTargetValueIsNotUndefined()
    {
        if (scopeFacade.Get(fixExpression.Target.Value).Equals(SpecialValues.UNDEFINED))
        {
            throw new OperationOnUndefinedValueException();
        }
    }

    private void ValidateTargetIsNumber()
    {
        var targetType = scopeFacade.GetType(fixExpression.Target.Value);
        if (!targetType.IsNumber())
        {
            throw new InvalidTypeException(targetType, DataType.Number());
        }
    }

    private void ValidateTargetExists()
    {
        if (!scopeFacade.ReferenceExists(fixExpression.Target.Value))
        {
            throw new ReferenceNameDoesNotExistException(fixExpression.Target.Value);
        }
    }
}