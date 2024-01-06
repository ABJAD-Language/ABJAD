namespace ABJAD.Interpreter.Domain.ScopeManagement;

public class ScopeFactory
{
    public static Scope NewScope()
    {
        var referenceScope = new ReferenceScope(new Dictionary<string, StateElement>());
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        return new Scope(referenceScope, functionScope, typeScope);
    }
}