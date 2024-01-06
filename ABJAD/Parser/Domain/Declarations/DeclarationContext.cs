using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Declarations;

public class DeclarationContext
{
    private readonly ParseDeclarationStrategy strategy;

    public DeclarationContext(ParseDeclarationStrategy strategy)
    {
        Guard.Against.Null(strategy);
        this.strategy = strategy;
    }

    public Declaration ParseDeclaration()
    {
        return strategy.Parse();
    }
}