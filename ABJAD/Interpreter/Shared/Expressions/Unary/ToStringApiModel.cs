﻿namespace ABJAD.Interpreter.Shared.Expressions.Unary;

public class ToStringApiModel
{
    public object Target { get; }

    public ToStringApiModel(object target)
    {
        Target = target;
    }
}