namespace ABJAD.Interpreter.Domain.Types;

public struct UndefinedDataType : DataType
{
    public string GetValue()
    {
        return "غير_معرف";
    }
}