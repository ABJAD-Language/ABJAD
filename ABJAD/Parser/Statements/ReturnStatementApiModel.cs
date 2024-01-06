using ABJAD.Parser.Expressions;

namespace ABJAD.Parser.Statements;

public class ReturnStatementApiModel : StatementApiModel
{
    public ExpressionApiModel Target { get; }

    public ReturnStatementApiModel(ExpressionApiModel target)
    {
        Target = target;
        Type = "statement.return";
    }
}