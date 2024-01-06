namespace ABJAD.Interpreter.Shared.Declarations;

public class ConstructorDeclarationApiModel
{
    public List<ParameterApiModel> Parameters { get; }
    public object Body { get; }

    public ConstructorDeclarationApiModel(List<ParameterApiModel> parameters, object body)
    {
        Parameters = parameters;
        Body = body;
    }
}