using ABJAD.Parser.Domain.Bindings;
using ABJAD.Parser.Domain.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Statements;

public class ParseBlockStatementStrategy : ParseStatementStrategy, BlockStatementParser
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly IBindingFactory bindingFactory;

    public ParseBlockStatementStrategy(ITokenConsumer tokenConsumer, IBindingFactory bindingFactory)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(bindingFactory);
        this.tokenConsumer = tokenConsumer;
        this.bindingFactory = bindingFactory;
    }

    public Statement Parse()
    {
        tokenConsumer.Consume(TokenType.OPEN_BRACE);

        var bindings = ParseBindings();

        tokenConsumer.Consume(TokenType.CLOSE_BRACE);

        return new BlockStatement(bindings);
    }

    private List<Binding> ParseBindings()
    {
        var bindings = new List<Binding>();
        while (BlockContainsMoreBindings())
        {
            bindings.Add(bindingFactory.Get());
        }

        return bindings;
    }

    private bool BlockContainsMoreBindings()
    {
        return !tokenConsumer.CanConsume(TokenType.CLOSE_BRACE);
    }
}