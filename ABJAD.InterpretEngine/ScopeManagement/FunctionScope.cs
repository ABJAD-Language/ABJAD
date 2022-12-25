using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public class FunctionScope : IFunctionScope
{
    private readonly IDictionary<string, List<FunctionElement>> state;

    public FunctionScope(IDictionary<string, List<FunctionElement>> state)
    {
        this.state = state;
    }

    public bool FunctionExists(string name, params DataType[] parametersTypes)
    {
        return state.ContainsKey(name) && state[name].Any(f =>
            TypesMatch(f.Parameters, parametersTypes.ToList()));
    }
    
    private static bool TypesMatch(List<FunctionParameter> parameters, List<DataType> types)
    {
        if (parameters.Count != types.Count) return false;

        return !parameters.Where((param, i) => !param.Type.Is(types[i])).Any();
    }

    public DataType? GetFunctionReturnType(string name, params DataType[] parametersTypes)
    {
        return GetFunction(name, parametersTypes).ReturnType;
    }

    public FunctionElement GetFunction(string name, params DataType[] parametersTypes)
    {
        return state[name].Single(func => TypesMatch(func.Parameters, parametersTypes.ToList()));
    }

    public void DefineFunction(string name, FunctionElement function)
    {
        if (state.ContainsKey(name))
        {
            state[name].Add(function);
        }
        else
        {
            state.Add(name, new List<FunctionElement> { function });
        }
    }

    public IFunctionScope Clone()
    {
        return new FunctionScope(state.ToDictionary(pair => pair.Key, pair => pair.Value));
    }
}