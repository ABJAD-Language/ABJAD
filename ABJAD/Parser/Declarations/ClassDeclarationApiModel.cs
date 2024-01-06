namespace ABJAD.Parser.Declarations;

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