namespace ABJAD.Interpreter.Shared.Expressions.Primitives;

public class IdentifierApiModel
{
    public string Value { get; }

    public IdentifierApiModel(string value)
    {
        Value = value;
    }
}