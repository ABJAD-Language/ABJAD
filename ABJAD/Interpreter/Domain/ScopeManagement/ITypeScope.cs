﻿using ABJAD.Interpreter.Domain.Types;
using System.Diagnostics.Contracts;

namespace ABJAD.Interpreter.Domain.ScopeManagement;

public interface ITypeScope
{
    bool TypeExists(string name);
    bool HasConstructor(string className, params DataType[] parameterTypes);
    ClassElement Get(string name);
    ConstructorElement GetConstructor(string className, params DataType[] parameterTypes);
    void DefineConstructor(string className, ConstructorElement constructor);
    void Define(string name, ClassElement @class);
    ITypeScope Clone();

    [Pure]
    ITypeScope Aggregate(ITypeScope scope);
}