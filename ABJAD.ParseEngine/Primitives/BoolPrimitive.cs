namespace ABJAD.ParseEngine.Primitives;

public class BoolPrimitive : Primitive<bool>
{
    private BoolPrimitive(bool value)
    {
        Value = value;
    }
    
    public static BoolPrimitive True()
    {
        return new BoolPrimitive(true);
    }
    
    public static BoolPrimitive False()
    {
        return new BoolPrimitive(false);
    }
}