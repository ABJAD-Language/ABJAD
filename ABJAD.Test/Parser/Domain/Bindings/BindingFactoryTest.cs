using ABJAD.Parser.Domain;
using ABJAD.Parser.Domain.Bindings;
using ABJAD.Parser.Domain.Declarations;
using ABJAD.Parser.Domain.Shared;
using ABJAD.Parser.Domain.Statements;
using FluentAssertions;
using Moq;

namespace ABJAD.Test.Parser.Domain.Bindings;

public class BindingFactoryTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();
    private readonly Mock<IDeclarationStrategyFactory> declarationStrategyFactory = new();
    private readonly Mock<ParseDeclarationStrategy> parseDeclarationStrategy = new();
    private readonly Mock<Declaration> declaration = new();
    private readonly Mock<IStatementStrategyFactory> statementStrategyFactory = new();
    private readonly Mock<ParseStatementStrategy> parseStatementStrategy = new();
    private readonly Mock<Statement> statement = new();

    [Fact]
    private void InstantiationFailsIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new BindingFactory(null, declarationStrategyFactory.Object, statementStrategyFactory.Object));
    }

    [Fact]
    private void InstantiationFailsIfDeclarationStrategyFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new BindingFactory(tokenConsumer.Object, null, statementStrategyFactory.Object));
    }

    [Fact]
    private void InstantiationFailsIfStatementStrategyFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new BindingFactory(tokenConsumer.Object, declarationStrategyFactory.Object, null));
    }

    [Fact]
    private void ReturnsDeclarationBindingWhenHeadTokenIsDeclarative()
    {
        tokenConsumer.Setup(c => c.Peek()).Returns(new Token { Type = TokenType.VAR });
        declarationStrategyFactory.Setup(d => d.Get()).Returns(parseDeclarationStrategy.Object);
        parseDeclarationStrategy.Setup(d => d.Parse()).Returns(declaration.Object);

        var binding = GetFactory().Get();
        var expectedBinding = new DeclarationBinding(declaration.Object);
        expectedBinding.Should().BeEquivalentTo(binding, options => options.RespectingRuntimeTypes());
    }

    [Fact]
    private void ReturnsStatementBindingWhenHeadTokenIsNotDeclarative()
    {
        tokenConsumer.Setup(c => c.Peek()).Returns(new Token { Type = TokenType.ID });
        statementStrategyFactory.Setup(f => f.Get()).Returns(parseStatementStrategy.Object);
        parseStatementStrategy.Setup(s => s.Parse()).Returns(statement.Object);

        var binding = GetFactory().Get();
        var expectedBinding = new StatementBinding(statement.Object);
        expectedBinding.Should().BeEquivalentTo(binding, options => options.RespectingRuntimeTypes());
    }


    private BindingFactory GetFactory() => new BindingFactory(tokenConsumer.Object, declarationStrategyFactory.Object,
        statementStrategyFactory.Object);
}