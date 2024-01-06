using ABJAD.Parser.Domain.Expressions;

namespace ABJAD.Parser.Domain.Statements;

public class PrintStatement : Statement
{
    public PrintStatement(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}