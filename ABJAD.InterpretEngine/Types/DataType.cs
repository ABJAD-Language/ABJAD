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
    
    bool IsNumber()
    {
        return this is NumberDataType;
    }

    bool IsString()
    {
        return this is StringDataType;
    }
}