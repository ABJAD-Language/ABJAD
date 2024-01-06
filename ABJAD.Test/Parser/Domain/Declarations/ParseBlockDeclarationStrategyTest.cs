using ABJAD.Parser.Domain;
using ABJAD.Parser.Domain.Bindings;
using ABJAD.Parser.Domain.Declarations;
using ABJAD.Parser.Domain.Shared;
using FluentAssertions;
using Moq;

namespace ABJAD.Test.Parser.Domain.Declarations;

public class ParseBlockDeclarationStrategyTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new Mock<ITokenConsumer>();
    private readonly Mock<IDeclarationStrategyFactory> declarationStrategyFactory = new Mock<IDeclarationStrategyFactory>();
    private readonly Mock<ParseDeclarationStrategy> parseDeclarationStrategy = new Mock<ParseDeclarationStrategy>();

    public ParseBlockDeclarationStrategyTest()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.CLOSE_BRACE)).Returns(true);
        declarationStrategyFactory.Setup(f => f.Get()).Returns(parseDeclarationStrategy.Object);
    }

    [Fact]
    private void InstantiationFailsIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseBlockDeclarationStrategy(null, declarationStrategyFactory.Object));
    }

    [Fact]
    private void InstantiationFailsIfBindingFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseBlockDeclarationStrategy(tokenConsumer.Object, null));
    }

    [Fact]
    private void ConsumesOpenBrace()
    {
        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.OPEN_BRACE));
    }

    [Fact]
    private void ParsesDeclarationUntilReachesCloseBraceToken()
    {
        tokenConsumer.SetupSequence(c => c.CanConsume(TokenType.CLOSE_BRACE))
            .Returns(false).Returns(false).Returns(true);

        GetStrategy().Parse();
        declarationStrategyFactory.Verify(f => f.Get(), Times.Exactly(2));
    }

    [Fact]
    private void ConsumesCloseBraceToken()
    {
        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.CLOSE_BRACE));
    }

    [Fact]
    private void ReturnsBlockDeclarationOnHappyPath()
    {
        var declaration = new Mock<Declaration>();
        tokenConsumer.SetupSequence(c => c.CanConsume(TokenType.CLOSE_BRACE)).Returns(false).Returns(true);
        parseDeclarationStrategy.Setup(f => f.Parse()).Returns(declaration.Object);
        var expectedStatement = new BlockDeclaration(new List<DeclarationBinding> { new DeclarationBinding(declaration.Object) });
        var statement = GetStrategy().Parse();
        statement.Should().BeEquivalentTo(expectedStatement, options => options.RespectingRuntimeTypes());
    }

    private ParseBlockDeclarationStrategy GetStrategy()
    {
        return new ParseBlockDeclarationStrategy(tokenConsumer.Object, declarationStrategyFactory.Object);
    }
}