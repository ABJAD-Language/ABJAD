using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public class Environment : ScopeManager, ScopeFacade
{
    private readonly List<IScope> scopes;

    public Environment(List<IScope> scopes)
    {
        this.scopes = scopes;
    }

    public void AddNewScope()
    {
        scopes.Add(new Scope(new Dictionary<string, StateElement>()));
    }

    public bool ReferenceExists(string name)
    {
        return scopes.Any(s => s.ReferenceExists(name));
    }

    public bool ReferenceExistsInCurrentScope(string name)
    {
        return scopes.Last().ReferenceExists(name);
    }

    public DataType GetType(string name)
    {
        return scopes.FindLast(s => s.ReferenceExists(name)).GetType(name);
    }

    public object Get(string name)
    {
        return scopes.FindLast(s => s.ReferenceExists(name)).Get(name);
    }

    public void Set(string name, object value)
    {
        if (scopes.Last().ReferenceExists(name))
        {
            scopes.Last().Set(name, value);
        }
        else
        {
            var referenceType = scopes.FindLast(s => s.ReferenceExists(name)).GetType(name);
            scopes.Last().Define(name, referenceType, value);
        }
    }

    public void Define(string name, DataType type, object value)
    {
        scopes.Last().Define(name, type, value);
    }

    public ScopeManager CloneScope()
    {
        return new Environment(scopes.Select(s => s.Clone()).ToList());
    }
}