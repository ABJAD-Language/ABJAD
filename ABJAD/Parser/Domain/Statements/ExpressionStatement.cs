using ABJAD.Parser.Domain.Expressions;

namespace ABJAD.Parser.Domain.Statements;

public class ExpressionStatement : Statement
{
    public ExpressionStatement(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}