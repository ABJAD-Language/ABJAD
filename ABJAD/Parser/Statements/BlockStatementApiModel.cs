using ABJAD.Parser.Bindings;

namespace ABJAD.Parser.Statements;

public class BlockStatementApiModel : StatementApiModel
{
    public List<BindingApiModel> Bindings { get; }

    public BlockStatementApiModel(List<BindingApiModel> bindings)
    {
        Bindings = bindings;
        Type = "statement.block";
    }
}