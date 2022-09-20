namespace ABJAD.ParseEngine.Primitives;

public class NumberPrimitive : Primitive<double>
{
    private NumberPrimitive(string value)
    {
        Value = double.Parse(value);
    }

    public static NumberPrimitive From(string value)
    {
        return new NumberPrimitive(value);
    }
}