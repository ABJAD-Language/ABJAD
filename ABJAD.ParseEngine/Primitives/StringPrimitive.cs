using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Primitives;

public class StringPrimitive : Primitive<string>
{
    public StringPrimitive(string value)
    {
        Guard.Against.Null(value);
        Value = value;
    }
}