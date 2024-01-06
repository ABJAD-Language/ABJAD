using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.ScopeManagement;

public class FunctionElement
{
    public List<FunctionParameter> Parameters { get; set; }
    public DataType? ReturnType { get; set; }
    public Block Body { get; set; }
}