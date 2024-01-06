namespace ABJAD.Interpreter.Shared.Declarations;

public class FunctionDeclarationApiModel
{
    public string Name { get; }
    public string? ReturnType { get; }
    public List<ParameterApiModel> Parameters { get; }
    public object Body { get; }

    public FunctionDeclarationApiModel(string name, string returnType, List<ParameterApiModel> parameters, object body)
    {
        Name = name;
        ReturnType = returnType;
        Parameters = parameters;
        Body = body;
    }
}