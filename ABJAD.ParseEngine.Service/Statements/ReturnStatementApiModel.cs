using ABJAD.ParseEngine.Service.Expressions;

namespace ABJAD.ParseEngine.Service.Statements;

public class ReturnStatementApiModel : StatementApiModel
{
    public ExpressionApiModel Target { get; }

    public ReturnStatementApiModel(ExpressionApiModel target)
    {
        Target = target;
        Type = "statement.return";
    }
}