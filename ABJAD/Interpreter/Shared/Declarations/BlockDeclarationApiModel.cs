namespace ABJAD.Interpreter.Shared.Declarations;

public class BlockDeclarationApiModel
{
    public List<object> Declarations { get; }

    public BlockDeclarationApiModel(List<object> declarations)
    {
        Declarations = declarations;
    }
}