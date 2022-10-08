using ABJAD.ParseEngine.Bindings;
using ABJAD.ParseEngine.Expressions;

namespace ABJAD.ParseEngine.Statements;

public class ForStatement : Statement
{
    public ForStatement(Binding targetDefinition, Statement condition, Expression targetCallback, Statement body)
    {
        TargetDefinition = targetDefinition;
        Condition = condition;
        TargetCallback = targetCallback;
        Body = body;
    }

    public Binding TargetDefinition { get; }
    public Statement Condition { get; }
    public Expression TargetCallback { get; }
    public Statement Body { get; }
}