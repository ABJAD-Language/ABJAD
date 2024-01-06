using ABJAD.Parser.Statements;

namespace ABJAD.Parser.Declarations;

public class FunctionDeclarationApiModel : DeclarationApiModel
{
    public string Name { get; }
    public string ReturnType { get; }
    public List<FunctionParameterApiModel> Parameters { get; }
    public BlockStatementApiModel Body { get; }

    public FunctionDeclarationApiModel(string name, string returnType, List<FunctionParameterApiModel> parameters, BlockStatementApiModel body)
    {
        Name = name;
        ReturnType = returnType;
        Parameters = parameters;
        Body = body;
        Type = "declaration.function";
    }
}