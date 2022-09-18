namespace ABJAD.ParseEngine.Primitives;

public class NullPrimitive : Primitive<dynamic?>
{
    public NullPrimitive()
    {
        Value = null;
    }
}