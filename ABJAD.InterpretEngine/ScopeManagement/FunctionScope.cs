using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public class FunctionScope : IFunctionScope
{
    private readonly IDictionary<(string, int), FunctionElement> state;

    public FunctionScope(IDictionary<(string, int), FunctionElement> state)
    {
        this.state = state;
    }

    public bool FunctionExists(string name, int numberOfParameters)
    {
        return state.ContainsKey((name, numberOfParameters));
    }

    public DataType? GetFunctionReturnType(string name, int numberOfParameters)
    {
        return state[(name, numberOfParameters)].ReturnType;
    }

    public FunctionElement GetFunction(string name, int numberOfParameters)
    {
        return state[(name, numberOfParameters)];
    }

    public void DefineFunction(string name, FunctionElement function)
    {
        state.Add((name, function.Parameters.Count), function);
    }

    public IFunctionScope Clone()
    {
        return new FunctionScope(state.ToDictionary(pair => pair.Key, pair => pair.Value));
    }
}