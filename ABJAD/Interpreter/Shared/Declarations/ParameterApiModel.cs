namespace ABJAD.Interpreter.Shared.Declarations;

public class ParameterApiModel
{
    public string Name { get; }
    public string Type { get; }

    public ParameterApiModel(string name, string type)
    {
        Name = name;
        Type = type;
    }
}