using ABJAD.ParseEngine.Expressions;

namespace ABJAD.ParseEngine.Statements;

public class IfStatement : Statement
{
    public IfStatement(Expression condition, Statement body)
    {
        Condition = condition;
        Body = body;
    }

    public Expression Condition { get; }
    public Statement Body { get; }
}