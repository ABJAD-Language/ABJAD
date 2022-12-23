using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public class FunctionElement
{
    public List<FunctionParameter> Parameters { get; set; }
    public DataType? ReturnType { get; set; }
    public Block Body { get; set; }
}