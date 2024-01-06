namespace ABJAD.Interpreter.Domain.Shared.Declarations;

public class ClassDeclaration : Declaration
{
    public string Name { get; set; }
    public List<Declaration> Declarations { get; set; }
}