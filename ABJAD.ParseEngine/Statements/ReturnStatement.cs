using ABJAD.ParseEngine.Expressions;

namespace ABJAD.ParseEngine.Statements;

public class ReturnStatement : Statement
{
    public ReturnStatement(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}