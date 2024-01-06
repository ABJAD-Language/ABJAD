namespace ABJAD.Interpreter.Shared.Declarations;

public class VariableDeclarationApiModel
{
    public string Name { get; }
    public string Type { get; }
    public object? Value { get; }

    public VariableDeclarationApiModel(string name, string type, object value)
    {
        Name = name;
        Type = type;
        Value = value;
    }
}