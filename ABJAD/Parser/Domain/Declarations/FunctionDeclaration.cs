using ABJAD.Parser.Domain.Statements;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Declarations;

public class FunctionDeclaration : Declaration
{
    public FunctionDeclaration(string name, string? returnType, List<FunctionParameter> parameters, BlockStatement body)
    {
        Guard.Against.NullOrEmpty(name);
        Guard.Against.Null(parameters);
        Guard.Against.Null(body);
        Name = name;
        ReturnType = returnType;
        Parameters = parameters;
        Body = body;
    }

    public string Name { get; }
    public string? ReturnType { get; }
    public List<FunctionParameter> Parameters { get; }
    public BlockStatement Body { get; }
}