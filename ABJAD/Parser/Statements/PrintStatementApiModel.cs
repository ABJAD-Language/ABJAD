using ABJAD.Parser.Expressions;

namespace ABJAD.Parser.Statements;

public class PrintStatementApiModel : StatementApiModel
{
    public ExpressionApiModel Target { get; }

    public PrintStatementApiModel(ExpressionApiModel target)
    {
        Target = target;
        Type = "statement.print";
    }
}