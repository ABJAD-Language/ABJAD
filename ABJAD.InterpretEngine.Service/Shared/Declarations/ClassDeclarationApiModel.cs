namespace ABJAD.InterpretEngine.Service.Shared.Declarations;

public class ClassDeclarationApiModel
{
    public string Name { get; }   
    public BlockDeclarationApiModel Body { get; }

    public ClassDeclarationApiModel(string name, BlockDeclarationApiModel body)
    {
        Name = name;
        Body = body;
    }
}