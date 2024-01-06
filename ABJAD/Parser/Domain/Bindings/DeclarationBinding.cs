using ABJAD.Parser.Domain.Declarations;

namespace ABJAD.Parser.Domain.Bindings;

public class DeclarationBinding : Binding
{
    public DeclarationBinding(Declaration declaration)
    {
        Declaration = declaration;
    }

    public Declaration Declaration { get; }
}