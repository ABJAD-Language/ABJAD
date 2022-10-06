using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Statements;
using Ardalis.GuardClauses;

namespace ABJAD.ParseEngine.Bindings;

public class BindingFactory : IBindingFactory
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly IDeclarationStrategyFactory declarationStrategyFactory;
    private readonly IStatementStrategyFactory statementStrategyFactory;

    public BindingFactory(ITokenConsumer tokenConsumer, IDeclarationStrategyFactory declarationStrategyFactory,
        IStatementStrategyFactory statementStrategyFactory)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(declarationStrategyFactory);
        Guard.Against.Null(statementStrategyFactory);
        this.tokenConsumer = tokenConsumer;
        this.declarationStrategyFactory = declarationStrategyFactory;
        this.statementStrategyFactory = statementStrategyFactory;
    }

    public Binding Get()
    {
        if (IsHeadTokenDeclarative())
        {
            var parseDeclarationStrategy = declarationStrategyFactory.Get();
            return new DeclarationBinding(parseDeclarationStrategy.Parse());
        }

        var parseStatementStrategy = statementStrategyFactory.Get();
        return new StatementBinding(parseStatementStrategy.Parse());
    }

    private bool IsHeadTokenDeclarative()
    {
        return IDeclarationStrategyFactory.DeclarationTokenTypes.Contains(tokenConsumer.Peek().Type);
    }
}