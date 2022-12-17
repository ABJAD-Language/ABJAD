﻿using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine;

public interface IScope
{
    bool ReferenceExists(string name);
    DataType GetType(string name);
    object Get(string name);
    void Set(string name, object value);
}