using ABJAD.Parser.Domain.Bindings;
using ABJAD.Parser.Domain.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Declarations;

public class ParseBlockDeclarationStrategy : ParseDeclarationStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly IDeclarationStrategyFactory declarationStrategyFactory;

    public ParseBlockDeclarationStrategy(ITokenConsumer tokenConsumer, IDeclarationStrategyFactory declarationStrategyFactory)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(declarationStrategyFactory);
        this.tokenConsumer = tokenConsumer;
        this.declarationStrategyFactory = declarationStrategyFactory;
    }

    public Declaration Parse()
    {
        tokenConsumer.Consume(TokenType.OPEN_BRACE);

        var bindings = new List<DeclarationBinding>();
        while (BlockContainsMoreBindings())
        {
            var parseDeclarationStrategy = declarationStrategyFactory.Get();
            bindings.Add(new DeclarationBinding(parseDeclarationStrategy.Parse()));
        }

        tokenConsumer.Consume(TokenType.CLOSE_BRACE);
        return new BlockDeclaration(bindings);
    }

    private bool BlockContainsMoreBindings()
    {
        return !tokenConsumer.CanConsume(TokenType.CLOSE_BRACE);
    }
}