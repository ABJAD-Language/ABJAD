namespace ABJAD.Interpreter.Domain.Shared.Statements;

public class IfElse : Statement
{
    public Conditional MainConditional { get; set; }
    public List<Conditional> OtherConditionals { get; set; } = new();
    public Statement? ElseBody { get; set; }
}