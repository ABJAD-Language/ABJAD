using ABJAD.InterpretEngine.Shared.Statements;

namespace ABJAD.InterpretEngine.ScopeManagement;

public class ConstructorElement
{
    public List<FunctionParameter> Parameters { get; set; }
    public Block Body { get; set; }
}