namespace ABJAD.Interpreter.Domain.Shared.Expressions;

public class InstanceFieldAccess : Expression
{
    public string Instance { get; set; }
    public List<string> NestedFields { get; set; } = new();
}