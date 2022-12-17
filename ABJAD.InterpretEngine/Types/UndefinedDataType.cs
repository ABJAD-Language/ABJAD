namespace ABJAD.InterpretEngine.Types;

public struct UndefinedDataType : DataType
{
    public string GetValue()
    {
        return "غير_معرف";
    }
}