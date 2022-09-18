namespace ABJAD.ParseEngine.Primitives;

public class NumberPrimitive : Primitive<double>
{
    public NumberPrimitive(string value)
    {
        Value = double.Parse(value);
    }
}