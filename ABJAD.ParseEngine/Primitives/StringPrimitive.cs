using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Primitives;

public class StringPrimitive : Primitive<string>
{
    private StringPrimitive(string value)
    {
        Guard.Against.Null(value);
        Value = value;
    }

    public static StringPrimitive From(string value)
    {
        return new StringPrimitive(value);
    }
}