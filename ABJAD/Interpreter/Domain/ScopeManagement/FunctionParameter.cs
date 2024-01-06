using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.ScopeManagement;

public class FunctionParameter
{
    public DataType Type { get; set; }
    public string Name { get; set; }
}