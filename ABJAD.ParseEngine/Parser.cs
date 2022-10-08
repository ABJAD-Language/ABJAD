using ABJAD.ParseEngine.Bindings;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;

namespace ABJAD.ParseEngine;

public class Parser
{
    private readonly TokenConsumer tokenConsumer;
    private readonly BindingFactory bindingFactory;

    public Parser(List<Token> tokens)
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