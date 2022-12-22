namespace ABJAD.InterpretEngine.Shared.Statements;

public class IfElse
{
    public Conditional MainConditional { get; set; }
    public List<Conditional> OtherConditionals { get; set; }
    public Statement ElseBody { get; set; }
}