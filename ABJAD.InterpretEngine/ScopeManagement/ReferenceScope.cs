using System.Diagnostics.Contracts;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public class ReferenceScope : IReferenceScope
{
    private readonly IDictionary<string, StateElement> state;

    public ReferenceScope(IDictionary<string, StateElement> state)
    {
        this.state = state;
    }

    public bool ReferenceExists(string name)
    {
        return state.ContainsKey(name);
    }

    public DataType GetType(string name)
    {
        return state[name].Type;
    }

    public object Get(string name)
    {
        return state[name].Value;
    }

    public void Update(string name, object value)
    {
        var oldElement = state[name];

        if (oldElement.IsConstant)
        {
            throw new IllegalConstantValueChangeException(name);
        }
        
        state.Remove(name);
        state.Add(name, oldElement with { Value = value });
    }

    public void DefineVariable(string name, DataType type, object value)
    {
        state.Add(name, new StateElement { Type = type, Value = value, IsConstant = false });
    }

    public void DefineConstant(string name, DataType type, object value)
    {
        state.Add(name, new StateElement { Type = type, Value = value, IsConstant = true });
    }

    public bool IsConstant(string name)
    {
        return state[name].IsConstant;
    }

    public IReferenceScope Clone()
    {
        var stateClone = state.ToDictionary(pair => pair.Key, pair => pair.Value);
        return new ReferenceScope(stateClone);
    }

    [Pure]
    public IReferenceScope Aggregate(IReferenceScope referenceScope)
    {
        var otherScope = (ReferenceScope) referenceScope;
        var newState = state.ToDictionary(pair => pair.Key, pair => pair.Value);
        foreach (var key in otherScope.state.Keys)
        {
            if (newState.ContainsKey(key))
            {
                newState[key] = otherScope.state[key];
            }
            else
            {
                newState.Add(key, otherScope.state[key]);
            }
        }

        return new ReferenceScope(newState);
    }
}