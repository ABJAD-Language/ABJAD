namespace ABJAD.Parser.Domain.Primitives;

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