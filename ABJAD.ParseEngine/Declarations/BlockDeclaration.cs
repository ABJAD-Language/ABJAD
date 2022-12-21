using ABJAD.ParseEngine.Bindings;

namespace ABJAD.ParseEngine.Declarations;

public class BlockDeclaration : Declaration
{
    public List<DeclarationBinding> DeclarationBindings { get; }

    public BlockDeclaration(List<DeclarationBinding> declarationBindings)
    {
        DeclarationBindings = declarationBindings;
    }
}