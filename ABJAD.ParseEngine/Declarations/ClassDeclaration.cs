using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Declarations;

public class ClassDeclaration : Declaration
{
    public ClassDeclaration(string name, BlockDeclaration body)
    {
        Guard.Against.Null(name);
        Guard.Against.Null(body);
        Name = name;
        Body = body;
    }

    public string Name { get; }
    public BlockDeclaration Body { get; }
}