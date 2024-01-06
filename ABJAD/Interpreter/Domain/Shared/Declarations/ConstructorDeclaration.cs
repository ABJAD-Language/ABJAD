using ABJAD.Interpreter.Domain.Shared.Statements;

namespace ABJAD.Interpreter.Domain.Shared.Declarations;

public class ConstructorDeclaration : Declaration
{
    public List<Parameter> Parameters { get; set; }
    public Block Body { get; set; }
}