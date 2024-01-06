using ABJAD.Parser.Domain.Expressions;

namespace ABJAD.Parser.Domain.Statements;

public class ReturnStatement : Statement
{
    public ReturnStatement(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}