using ABJAD.Parser.Domain;
using ABJAD.Parser.Domain.Declarations;
using ABJAD.Parser.Domain.Shared;
using Moq;

namespace ABJAD.Test.Parser.Domain.Declarations;

public class DeclarationStrategyFactoryTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();

    [Fact]
    private void InstantiationFailsIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new DeclarationStrategyFactory(null));
    }

    [Fact]
    private void ReturnsParseVariableDeclarationStrategyWhenTokenTypeIsVar()
    {
        SetHeadTokenType(TokenType.VAR);
        Assert.IsType<ParseVariableDeclarationStrategy>(GetFactory().Get());
    }

    [Fact]
    private void ReturnsParseConstantDeclarationStrategyWhenTokenTypeIsConst()
    {
        SetHeadTokenType(TokenType.CONST);
        Assert.IsType<ParseConstantDeclarationStrategy>(GetFactory().Get());
    }

    [Fact]
    private void ReturnsParseFunctionDeclarationStrategyWhenTokenTypeIsConst()
    {
        SetHeadTokenType(TokenType.FUNC);
        Assert.IsType<ParseFunctionDeclarationStrategy>(GetFactory().Get());
    }

    [Fact]
    private void ReturnsParseConstructorDeclarationStrategyWhenTokenTypeIsConst()
    {
        SetHeadTokenType(TokenType.CONSTRUCTOR);
        Assert.IsType<ParseConstructorDeclarationStrategy>(GetFactory().Get());
    }

    [Fact]
    private void ReturnsParseClassDeclarationStrategyWhenTokenTypeIsConst()
    {
        SetHeadTokenType(TokenType.CLASS);
        Assert.IsType<ParseClassDeclarationStrategy>(GetFactory().Get());
    }

    private void SetHeadTokenType(TokenType tokenType)
    {
        tokenConsumer.Setup(c => c.Peek()).Returns(new Token { Type = tokenType });
    }

    private DeclarationStrategyFactory GetFactory() => new(tokenConsumer.Object);
}