namespace ABJAD.InterpretEngine.ScopeManagement;

public class EnvironmentFactory
{
    public static Environment NewEnvironment()
    {
        return new Environment(new List<Scope> { ScopeFactory.NewScope() });
    }
}