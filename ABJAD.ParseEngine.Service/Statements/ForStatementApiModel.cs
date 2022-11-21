using ABJAD.ParseEngine.Service.Bindings;
using ABJAD.ParseEngine.Service.Expressions;

namespace ABJAD.ParseEngine.Service.Statements;

public class ForStatementApiModel : StatementApiModel
{
    public BindingApiModel Target { get; }
    public StatementApiModel Condition { get; }
    public ExpressionApiModel TargetCallback { get; }
    public StatementApiModel Body { get; }

    public ForStatementApiModel(BindingApiModel target, StatementApiModel condition, ExpressionApiModel targetCallback, StatementApiModel body)
    {
        Target = target;
        Condition = condition;
        TargetCallback = targetCallback;
        Body = body;
        Type = "statement.for";
    }
}