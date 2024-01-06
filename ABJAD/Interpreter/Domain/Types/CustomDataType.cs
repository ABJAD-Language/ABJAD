namespace ABJAD.Interpreter.Domain.Types;

public struct CustomDataType : DataType
{
    private readonly string value;

    public CustomDataType(string value)
    {
        this.value = value;
    }

    public string GetValue()
    {
        return value;
    }
}