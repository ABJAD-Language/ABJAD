using ABJAD.Parser.Domain.Shared;
using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Declarations;

public class ParseClassDeclarationStrategy : ParseDeclarationStrategy
{
    private readonly ITokenConsumer tokenConsumer;
    private readonly ParseDeclarationStrategy blockDeclarationParser;

    public ParseClassDeclarationStrategy(ITokenConsumer tokenConsumer, ParseDeclarationStrategy blockDeclarationParser)
    {
        Guard.Against.Null(tokenConsumer);
        Guard.Against.Null(blockDeclarationParser);
        this.tokenConsumer = tokenConsumer;
        this.blockDeclarationParser = blockDeclarationParser;
    }

    public Declaration Parse()
    {
        tokenConsumer.Consume(TokenType.CLASS);
        var nameId = tokenConsumer.Consume(TokenType.ID);
        // var body = blockStatementParser.Parse();
        var body = blockDeclarationParser.Parse();

        // var bindings = new List<Binding>();
        // while (BlockContainsMoreBindings())
        // {
        //     var parseDeclarationStrategy = declarationStrategyFactory.Get();
        //     bindings.Add(new DeclarationBinding(parseDeclarationStrategy.Parse()));
        // }
        // var body = new BlockStatement(bindings);


        return new ClassDeclaration(nameId.Content, body as BlockDeclaration);
    }

    private bool BlockContainsMoreBindings()
    {
        return !tokenConsumer.CanConsume(TokenType.CLOSE_BRACE);
    }
}