using ABJAD.ParseEngine.Service.Statements;

namespace ABJAD.ParseEngine.Service.Declarations;

public class ClassDeclarationApiModel : DeclarationApiModel
{
    public string Name { get; }
    public BlockDeclarationApiModel Body { get; }

    public ClassDeclarationApiModel(string name, BlockDeclarationApiModel body)
    {
        Name = name;
        Body = body;
        Type = "declaration.class";
    }
}