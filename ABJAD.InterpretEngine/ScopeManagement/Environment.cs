using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public class Environment : ScopeFacade
{
    private readonly List<Scope> scopes;

    public Environment(List<Scope> scopes)
    {
        this.scopes = scopes;
    }

    public bool ReferenceExists(string name)
    {
        return scopes.Any(s => s.ReferenceScope.ReferenceExists(name));
    }

    public bool ReferenceExistsInCurrentScope(string name)
    {
        return scopes.Last().ReferenceScope.ReferenceExists(name);
    }

    public DataType GetReferenceType(string name)
    {
        return scopes.Last(s => s.ReferenceScope.ReferenceExists(name)).ReferenceScope.GetType(name);
    }

    public object GetReference(string name)
    {
        return scopes.Last(s => s.ReferenceScope.ReferenceExists(name)).ReferenceScope.Get(name);
    }

    public void UpdateReference(string name, object value) 
    {
        scopes.Last(scope => scope.ReferenceScope.ReferenceExists(name)).ReferenceScope.Update(name, value);
    }

    public void DefineVariable(string name, DataType type, object value)
    {
        scopes.Last().ReferenceScope.DefineVariable(name, type, value);
    }

    public void DefineConstant(string name, DataType type, object value)
    {
        scopes.Last().ReferenceScope.DefineConstant(name, type, value);
    }

    public bool IsReferenceConstant(string name)
    {
        return scopes.Last(s => s.ReferenceScope.ReferenceExists(name)).ReferenceScope.IsConstant(name);
    }

    public bool FunctionExists(string name, params DataType[] parametersTypes)
    {
        return scopes.Any(s => s.FunctionScope.FunctionExists(name, parametersTypes));
    }

    public bool FunctionExistsInCurrentScope(string name, params DataType[] parametersTypes)
    {
        return scopes.Last().FunctionScope.FunctionExists(name, parametersTypes);
    }

    public DataType? GetFunctionReturnType(string name, params DataType[] parametersTypes)
    {
        return scopes
            .Last(s => s.FunctionScope.FunctionExists(name, parametersTypes)).FunctionScope
            .GetFunctionReturnType(name, parametersTypes);
    }

    public FunctionElement GetFunction(string name, params DataType[] parametersTypes)
    {
        return scopes
            .Last(s => s.FunctionScope.FunctionExists(name, parametersTypes)).FunctionScope
            .GetFunction(name, parametersTypes);
    }

    public void DefineFunction(string name, FunctionElement function)
    {
        scopes.Last().FunctionScope.DefineFunction(name, function);
    }

    public bool TypeExists(string name)
    {
        return scopes.Any(s => s.TypeScope.TypeExists(name));
    }

    public bool TypeHasConstructor(string className, params DataType[] parameterTypes)
    {
        return scopes.Single(s => s.TypeScope.TypeExists(className)).TypeScope.HasConstructor("type", parameterTypes);
    }

    public ClassElement GetType(string name)
    {
        return scopes.Single(s => s.TypeScope.TypeExists(name)).TypeScope.Get(name);
    }

    public ConstructorElement GetTypeConstructor(string name, params DataType[] parameterTypes)
    {
        return scopes.Single(s => s.TypeScope.TypeExists(name)).TypeScope.GetConstructor(name, parameterTypes);
    }

    public void DefineType(string name, ClassElement @class)
    {
        scopes.Last().TypeScope.Define(name, @class);
    }

    public void DefineTypeConstructor(string className, ConstructorElement constructor)
    {
        scopes.Last().TypeScope.DefineConstructor(className, constructor);
    }

    public ScopeFacade CloneScope()
    {
        return new Environment(scopes.Select(s => new Scope(s.ReferenceScope.Clone(), s.FunctionScope.Clone(), s.TypeScope.Clone())).ToList());
    }

    public void AddNewScope()
    {
        scopes.Add(ScopeFactory.NewScope());
    }

    public void RemoveLastScope()
    {
        scopes.Remove(scopes.Last());
        if (!scopes.Any())
        {
            AddNewScope();
        }
    }

    public void AddScope(ScopeFacade scopeFacade)
    {
        var otherScopes = scopeFacade.GetScopes();
        var referenceScope = otherScopes.Select(s => s.ReferenceScope).Aggregate((scope1, scope2) => scope1.Aggregate(scope2));
        var functionScope = otherScopes.Select(s => s.FunctionScope).Aggregate((scope1, scope2) => scope1.Aggregate(scope2));
        var typeScope = otherScopes.Select(s => s.TypeScope).Aggregate((scope1, scope2) => scope1.Aggregate(scope2));
        scopes.Add(new Scope(referenceScope, functionScope, typeScope));
    }

    public List<Scope> GetScopes()
    {
        return scopes;
    }
}