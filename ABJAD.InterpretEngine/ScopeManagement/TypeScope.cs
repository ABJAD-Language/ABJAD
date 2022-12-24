﻿using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public class TypeScope : ITypeScope
{
    private readonly IDictionary<string, ClassElement> state;

    public TypeScope(IDictionary<string, ClassElement> state)
    {
        this.state = state;
    }

    public bool TypeExists(string name)
    {
        return state.ContainsKey(name);
    }

    public bool HasConstructor(string className, params DataType[] parameterTypes)
    {
        return state[className].Constructors
            .SingleOrDefault(c => TypesMatch(c.Parameters.Select(p => p.Type).ToList(), parameterTypes.ToList())) != null;
    }

    private static bool TypesMatch(List<DataType> list1, List<DataType> list2)
    {
        if (list1.Count != list2.Count) return false;

        return !list1.Where((t, i) => !t.Is(list2[i])).Any();
    }

    public ClassElement Get(string name)
    {
        return state[name];
    }

    public void DefineConstructor(string className, ConstructorElement constructor)
    {
        state[className].Constructors.Add(constructor);
    }

    public void Define(string name, ClassElement @class)
    {
        state.Add(name, @class);
    }

    public ITypeScope Clone()
    {
        return new TypeScope(state.ToDictionary(pair => pair.Key, pair => pair.Value));
    }
}