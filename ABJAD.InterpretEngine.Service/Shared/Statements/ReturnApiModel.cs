namespace ABJAD.InterpretEngine.Service.Shared.Statements;

public class ReturnApiModel
{
    public object Target { get; }

    public ReturnApiModel(object target)
    {
        Target = target;
    }
}