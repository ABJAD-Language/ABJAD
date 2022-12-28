namespace ABJAD.InterpretEngine.Service.Shared.Declarations;

public class ConstantDeclarationApiModel
{
    public string Name { get; }
    public string Type { get; }
    public object Value { get; }

    public ConstantDeclarationApiModel(string name, string type, object value)
    {
        Name = name;
        Type = type;
        Value = value;
    }
}