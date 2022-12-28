namespace ABJAD.InterpretEngine.Types;

public interface DataType
{
    string GetValue();
    
    static DataType Number()
    {
        return new NumberDataType();
    }

    static DataType String()
    {
        return new StringDataType();
    }

    static DataType Bool()
    {
        return new BoolDataType();
    }

    static DataType Undefined()
    {
        return new UndefinedDataType();
    }

    static DataType Custom(string value)
    {
        return new CustomDataType(value);
    }

    static DataType From(string type)
    {
        return type switch
        {
            "رقم" => Number(),
            "مقطع" => String(),
            "منطق" => Bool(),
            _ => Custom(type)
        };
    }
    
    bool IsNumber()
    {
        return this is NumberDataType;
    }

    bool IsString()
    {
        return this is StringDataType;
    }

    bool IsBool()
    {
        return this is BoolDataType;
    }

    bool IsUndefined()
    {
        return this is UndefinedDataType;
    }

    bool Is(DataType type)
    {
        return GetValue() == type.GetValue();
    }

    bool Is(string type)
    {
        return GetValue() == type;
    }
}