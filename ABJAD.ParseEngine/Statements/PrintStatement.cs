using ABJAD.ParseEngine.Expressions;

namespace ABJAD.ParseEngine.Statements;

public class PrintStatement : Statement
{
    public PrintStatement(Expression target)
    {
        Target = target;
    }

    public Expression Target { get; }
}