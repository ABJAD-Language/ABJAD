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

    public ClassElement Get(string name)
    {
        return state[name];
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