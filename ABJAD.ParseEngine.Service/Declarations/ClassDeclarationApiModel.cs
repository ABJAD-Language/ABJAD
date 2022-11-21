using ABJAD.ParseEngine.Service.Statements;

namespace ABJAD.ParseEngine.Service.Declarations;

public class ClassDeclarationApiModel : DeclarationApiModel
{
    public string Name { get; }
    public BlockStatementApiModel Body { get; }

    public ClassDeclarationApiModel(string name, BlockStatementApiModel body)
    {
        Name = name;
        Body = body;
        Type = "declaration.class";
    }
}