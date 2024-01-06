using ABJAD.Interpreter.Domain.Shared.Statements;

namespace ABJAD.Interpreter.Domain.ScopeManagement;

public class ConstructorElement
{
    public List<FunctionParameter> Parameters { get; set; }
    public Block Body { get; set; }
}