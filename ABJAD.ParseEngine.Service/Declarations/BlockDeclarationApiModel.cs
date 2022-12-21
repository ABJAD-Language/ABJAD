namespace ABJAD.ParseEngine.Service.Declarations;

public class BlockDeclarationApiModel : DeclarationApiModel
{
    public List<DeclarationApiModel> Declarations { get; }

    public BlockDeclarationApiModel(List<DeclarationApiModel> declarations)
    {
        Declarations = declarations;
        Type = "declaration.block";
    }
}