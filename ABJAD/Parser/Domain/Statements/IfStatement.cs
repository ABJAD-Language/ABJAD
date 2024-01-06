using ABJAD.Parser.Domain.Expressions;

namespace ABJAD.Parser.Domain.Statements;

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