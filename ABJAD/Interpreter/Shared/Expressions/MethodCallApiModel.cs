﻿using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;

namespace ABJAD.Interpreter.Shared.Expressions;

public class MethodCallApiModel
{
    public IdentifierPrimitive Method { get; }
    public List<object> Arguments { get; }

    public MethodCallApiModel(IdentifierPrimitive method, List<object> arguments)
    {
        Method = method;
        Arguments = arguments;
    }
}