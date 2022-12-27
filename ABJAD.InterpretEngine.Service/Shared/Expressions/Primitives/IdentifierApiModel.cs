namespace ABJAD.InterpretEngine.Service.Shared.Expressions.Primitives;

public class IdentifierApiModel
{
    public string Value { get; }

    public IdentifierApiModel(string value)
    {
        Value = value;
    }
}