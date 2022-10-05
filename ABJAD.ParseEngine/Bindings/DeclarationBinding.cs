using ABJAD.ParseEngine.Declarations;

namespace ABJAD.ParseEngine.Bindings;

public class DeclarationBinding : Binding
{
    public DeclarationBinding(Declaration declaration)
    {
        Declaration = declaration;
    }

    public Declaration Declaration { get; }
}