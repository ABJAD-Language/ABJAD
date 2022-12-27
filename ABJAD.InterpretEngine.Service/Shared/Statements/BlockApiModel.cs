namespace ABJAD.InterpretEngine.Service.Shared.Statements;

public class BlockApiModel
{
    public List<object> Bindings { get; }

    public BlockApiModel(List<object> bindings)
    {
        Bindings = bindings;
    }
}