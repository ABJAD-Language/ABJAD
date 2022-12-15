namespace ABJAD.InterpretEngine.Types;

public interface DataType
{
    string GetValue();
    
    static DataType Number()
    {
        return new NumberDataType();
    }
    
    bool IsNumber()
    {
        return this is NumberDataType;
    }
}