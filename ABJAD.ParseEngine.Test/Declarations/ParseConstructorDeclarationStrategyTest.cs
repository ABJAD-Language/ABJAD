using System;
using System.Collections.Generic;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Declarations;

public class ParseConstructorDeclarationStrategyTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();
    private readonly Mock<ITypeConsumer> typeConsumer = new();
    private readonly Mock<IParameterListConsumer> parameterListConsumer = new();
    private readonly Mock<ParseBlockStatementStrategy> blockStatementParser = new();
    private readonly Mock<List<FunctionParameter>> parameterList = new();
    private readonly Mock<BlockStatement> blockStatement = new();

    public ParseConstructorDeclarationStrategyTest()
    {
        parameterListConsumer.Setup(c => c.Consume()).Returns(parameterList.Object);
        blockStatementParser.Setup(p => p.Parse()).Returns(blockStatement.Object);
    }

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseConstructorDeclarationStrategy(null,
            typeConsumer.Object, parameterListConsumer.Object, blockStatementParser.Object));
    }

    [Fact]
    private void ThrowsExceptionIfTypeConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseConstructorDeclarationStrategy(tokenConsumer.Object,
            null, parameterListConsumer.Object, blockStatementParser.Object));
    }

    [Fact]
    private void ThrowsExceptionIfParameterListConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseConstructorDeclarationStrategy(tokenConsumer.Object,
            typeConsumer.Object, null, blockStatementParser.Object));
    }

    [Fact]
    private void ThrowsExceptionIfBlockStatementParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new ParseConstructorDeclarationStrategy(tokenConsumer.Object,
            typeConsumer.Object, parameterListConsumer.Object, null));
    }

    [Fact]
    private void ConsumesConstructorToken()
    {
        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.CONSTRUCTOR));
    }

    [Fact]
    private void ConsumesOpenParenthesisAfterConstructor()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.CONSTRUCTOR)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));

        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.OPEN_PAREN));
    }

    [Fact]
    private void ConsumesParameterListAfterOpenParenthesis()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.CONSTRUCTOR)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        parameterListConsumer.Setup(c => c.Consume()).Returns(parameterList.Object)
            .Callback(() => Assert.Equal(3, order++));

        GetStrategy().Parse();
        parameterListConsumer.Verify(c => c.Consume());
    }

    [Fact]
    private void ConsumesCloseParenthesisAfterParameterList()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.CONSTRUCTOR)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        parameterListConsumer.Setup(c => c.Consume()).Returns(parameterList.Object)
            .Callback(() => Assert.Equal(3, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Callback(() => Assert.Equal(4, order++));

        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.CLOSE_PAREN));
    }

    [Fact]
    private void ConsumesBlockStatementAfterCloseParenthesis()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.CONSTRUCTOR)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(2, order++));
        parameterListConsumer.Setup(c => c.Consume()).Returns(parameterList.Object)
            .Callback(() => Assert.Equal(3, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Callback(() => Assert.Equal(4, order++));
        blockStatementParser.Setup(c => c.Parse()).Returns(blockStatement.Object)
            .Callback(() => Assert.Equal(5, order++));

        GetStrategy().Parse();
        blockStatementParser.Verify(c => c.Parse());
    }

    [Fact]
    private void ReturnsConstructorDeclarationOnHappyPath()
    {
        var declaration = GetStrategy().Parse();
        Assert.True(declaration is ConstructorDeclaration);

        var constructorDeclaration = declaration as ConstructorDeclaration;
        Assert.Equal(parameterList.Object, constructorDeclaration.Parameters);
        Assert.Equal(blockStatement.Object, constructorDeclaration.Body);
    }

    private ParseConstructorDeclarationStrategy GetStrategy()
    {
        return new ParseConstructorDeclarationStrategy(tokenConsumer.Object, typeConsumer.Object,
            parameterListConsumer.Object, blockStatementParser.Object);
    }
}