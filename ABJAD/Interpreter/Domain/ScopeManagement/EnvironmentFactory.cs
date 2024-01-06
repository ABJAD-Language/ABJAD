namespace ABJAD.Interpreter.Domain.ScopeManagement;

public class EnvironmentFactory
{
    public static Environment NewEnvironment()
    {
        return new Environment(new List<Scope> { ScopeFactory.NewScope() });
    }
}