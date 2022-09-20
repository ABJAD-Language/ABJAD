namespace ABJAD.ParseEngine.Primitives;

public class NullPrimitive : Primitive<dynamic?>
{
    private NullPrimitive()
    {
        Value = null;
    }

    public static NullPrimitive Instance()
    {
        return new NullPrimitive();
    }
}