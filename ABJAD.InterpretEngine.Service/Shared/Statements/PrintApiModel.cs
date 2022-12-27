namespace ABJAD.InterpretEngine.Service.Shared.Statements;

public class PrintApiModel
{
    public object Target { get; }

    public PrintApiModel(object target)
    {
        Target = target;
    }
}