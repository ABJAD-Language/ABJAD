using ABJAD.Interpreter.Domain.Shared.Expressions;

namespace ABJAD.Interpreter.Domain.Shared.Statements;

public class ForLoop : Statement
{
    public Binding TargetDefinition { get; set; }
    public ExpressionStatement Condition { get; set; }
    public Expression Callback { get; set; }
    public Statement Body { get; set; }
}