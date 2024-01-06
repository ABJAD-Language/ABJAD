using ABJAD.Parser.Domain.Bindings;

namespace ABJAD.Parser.Domain.Declarations;

public class BlockDeclaration : Declaration
{
    public List<DeclarationBinding> DeclarationBindings { get; }

    public BlockDeclaration(List<DeclarationBinding> declarationBindings)
    {
        DeclarationBindings = declarationBindings;
    }
}