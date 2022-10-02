using ABJAD.ParseEngine.Statements;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Declarations;

public class ClassDeclaration : Declaration
{
    public ClassDeclaration(string name, BlockStatement body)
    {
        Guard.Against.Null(name);
        Guard.Against.Null(body);
        Name = name;
        Body = body;
    }

    public string Name { get; }
    public BlockStatement Body { get; }
}