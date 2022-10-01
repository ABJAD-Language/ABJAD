using ABJAD.ParseEngine.Expressions;

namespace ABJAD.ParseEngine.Statements;

public class WhileStatement : Statement
{
    public WhileStatement(Expression condition, Statement body)
    {
        Condition = condition;
        Body = body;
    }

    public Expression Condition { get; }
    public Statement Body { get; }
}