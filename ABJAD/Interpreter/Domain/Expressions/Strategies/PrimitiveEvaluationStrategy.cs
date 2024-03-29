﻿using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.Expressions.Strategies;

public class PrimitiveEvaluationStrategy : ExpressionEvaluationStrategy
{
    private readonly Primitive primitive;
    private readonly ScopeFacade scopeFacade;

    public PrimitiveEvaluationStrategy(Primitive primitive, ScopeFacade scopeFacade)
    {
        this.primitive = primitive;
        this.scopeFacade = scopeFacade;
    }

    public EvaluatedResult Apply()
    {
        return primitive switch
        {
            BoolPrimitive boolPrimitive => new EvaluatedResult { Type = DataType.Bool(), Value = boolPrimitive.Value },
            NumberPrimitive numberPrimitive => new EvaluatedResult { Type = DataType.Number(), Value = numberPrimitive.Value },
            StringPrimitive stringPrimitive => new EvaluatedResult { Type = DataType.String(), Value = stringPrimitive.Value },
            NullPrimitive => new EvaluatedResult { Type = DataType.Undefined(), Value = SpecialValues.NULL },
            IdentifierPrimitive identifierPrimitive => GetIdentifierValueIfExists(identifierPrimitive)
        };
    }

    private EvaluatedResult GetIdentifierValueIfExists(IdentifierPrimitive identifierPrimitive)
    {
        if (scopeFacade.ReferenceExists(identifierPrimitive.Value))
        {
            var type = scopeFacade.GetReferenceType(identifierPrimitive.Value);
            var value = scopeFacade.GetReference(identifierPrimitive.Value);
            return new EvaluatedResult { Type = type, Value = value };
        }

        throw new ReferenceNameDoesNotExistException(identifierPrimitive.Value);
    }
}