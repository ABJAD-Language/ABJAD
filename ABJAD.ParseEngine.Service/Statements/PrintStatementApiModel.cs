using ABJAD.ParseEngine.Service.Expressions;

namespace ABJAD.ParseEngine.Service.Statements;

public class PrintStatementApiModel : StatementApiModel
{
    public ExpressionApiModel Target { get; }

    public PrintStatementApiModel(ExpressionApiModel target)
    {
        Target = target;
        Type = "statement.print";
    }
}