using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Primitives;

public class IdentifierPrimitive : Primitive<string>
{
    private IdentifierPrimitive(string value)
    {
        Guard.Against.Null(value);
        Value = value;
    }

    public static IdentifierPrimitive From(string value)
    {
        return new IdentifierPrimitive(value);
    }
}