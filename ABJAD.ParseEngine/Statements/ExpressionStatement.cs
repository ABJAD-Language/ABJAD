using ABJAD.ParseEngine.Expressions;

namespace ABJAD.ParseEngine.Statements;

public class ExpressionStatement : Statement
{
    public ExpressionStatement(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}