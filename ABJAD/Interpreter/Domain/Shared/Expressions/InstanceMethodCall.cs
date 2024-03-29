﻿namespace ABJAD.Interpreter.Domain.Shared.Expressions;

public class InstanceMethodCall : Expression
{
    public List<string> Instances { get; set; }
    public string MethodName { get; set; }
    public List<Expression> Arguments { get; set; }
}