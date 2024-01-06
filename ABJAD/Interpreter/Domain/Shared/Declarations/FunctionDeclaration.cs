using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.Shared.Declarations;

public class FunctionDeclaration : Declaration
{
    public string Name { get; set; }
    public DataType? ReturnType { get; set; }
    public List<Parameter> Parameters { get; set; }
    public Block Body { get; set; }
}