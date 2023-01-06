using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Shared.Declarations;

public class FunctionDeclaration : Declaration
{
    public string Name { get; set; }
    public DataType? ReturnType { get; set; }
    public List<Parameter> Parameters { get; set; }
    public Block Body { get; set; }
}