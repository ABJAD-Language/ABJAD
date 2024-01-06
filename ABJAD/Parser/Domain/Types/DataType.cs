namespace ABJAD.Parser.Domain.Types;

public interface DataType
{
    string GetValue();

    public static StringDataType String()
    {
        return new StringDataType();
    }

    public static NumberDataType Number()
    {
        return new NumberDataType();
    }

    public static BoolDataType Bool()
    {
        return new BoolDataType();
    }

    public static VoidDataType Void()
    {
        return new VoidDataType();
    }

    public static CustomDataType Custom(string value)
    {
        return new CustomDataType(value);
    }
}