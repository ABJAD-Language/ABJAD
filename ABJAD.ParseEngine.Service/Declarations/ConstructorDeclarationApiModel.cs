using ABJAD.ParseEngine.Service.Statements;

namespace ABJAD.ParseEngine.Service.Declarations;

public class ConstructorDeclarationApiModel : DeclarationApiModel
{
    public List<FunctionParameterApiModel> Parameters { get; }
    public BlockStatementApiModel Body { get; }

    public ConstructorDeclarationApiModel(List<FunctionParameterApiModel> parameters, BlockStatementApiModel body)
    {
        Parameters = parameters;
        Body = body;
        Type = "declaration.constructor";
    }
}