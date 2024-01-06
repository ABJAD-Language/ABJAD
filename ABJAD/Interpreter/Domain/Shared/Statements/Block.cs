namespace ABJAD.Interpreter.Domain.Shared.Statements;

public class Block : Statement
{
    public List<Binding> Bindings { get; set; }
}