using ABJAD.Parser.Domain.Bindings;
using ABJAD.Parser.Domain.Expressions;

namespace ABJAD.Parser.Domain.Statements;

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
    public Statement Condition { get; } // TODO refactor to ExpressionStatement
    public Expression TargetCallback { get; }
    public Statement Body { get; }
}