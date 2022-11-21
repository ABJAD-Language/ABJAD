using ABJAD.ParseEngine.Service.Bindings;

namespace ABJAD.ParseEngine.Service.Statements;

public class BlockStatementApiModel : StatementApiModel
{
    public List<BindingApiModel> Bindings { get; }

    public BlockStatementApiModel(List<BindingApiModel> bindings)
    {
        Bindings = bindings;
        Type = "statement.block";
    }
}