using ABJAD.Parser.Domain.Bindings;
using ABJAD.Parser.Domain.Declarations;
using ABJAD.Parser.Domain.Shared;
using ABJAD.Parser.Domain.Statements;

namespace ABJAD.Parser.Domain;

public class ParserService : IParser
{
    private readonly TokenConsumer tokenConsumer;
    private readonly BindingFactory bindingFactory;

    public ParserService(List<Token> tokens)
    {
        tokenConsumer = new TokenConsumer(tokens, 0);

        bindingFactory = new BindingFactory(tokenConsumer, new DeclarationStrategyFactory(tokenConsumer),
            new StatementStrategyFactory(tokenConsumer));
    }

    public List<Binding> Parse()
    {
        var bindings = new List<Binding>();
        while (tokenConsumer.CanConsume())
        {
            bindings.Add(bindingFactory.Get());
        }

        return bindings;
    }
}