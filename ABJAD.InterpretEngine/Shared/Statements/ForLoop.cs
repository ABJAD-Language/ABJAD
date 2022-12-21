using ABJAD.InterpretEngine.Shared.Expressions;

namespace ABJAD.InterpretEngine.Shared.Statements;

public class ForLoop : Statement
{
    public Binding TargetDefinition { get; set; }
    public ExpressionStatement Condition { get; set; }
    public Expression Callback { get; set; }
    public Statement Body { get; set; }
}