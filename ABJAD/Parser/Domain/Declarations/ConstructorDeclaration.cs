using ABJAD.Parser.Domain.Statements;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Declarations;

public class ConstructorDeclaration : Declaration
{
    public ConstructorDeclaration(List<FunctionParameter> parameters, BlockStatement body)
    {
        Guard.Against.Null(parameters);
        Guard.Against.Null(body);
        Parameters = parameters;
        Body = body;
    }

    public List<FunctionParameter> Parameters { get; }
    public BlockStatement Body { get; }
}