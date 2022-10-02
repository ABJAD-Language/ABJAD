using System;
using System.Collections.Generic;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Declarations;

public class ParseFunctionDeclarationStrategyTest
{
    private readonly Mock<ITokenConsumer> tokenConsumer = new();
    private readonly Mock<ParseBlockStatementStrategy> blockStatementParser = new();
    private readonly Mock<ITypeConsumer> typeConsumer = new();
    private readonly Mock<IParameterListConsumer> parameterListConsumer = new();
    private readonly Mock<List<FunctionParameter>> parameters = new();
    private readonly Mock<BlockStatement> body = new();

    public ParseFunctionDeclarationStrategyTest()
    {
        tokenConsumer.Setup(c => c.CanConsume(TokenType.CLOSE_PAREN)).Returns(true);
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "className" });
        parameterListConsumer.Setup(c => c.Consume()).Returns(parameters.Object);
        typeConsumer.Setup(c => c.ConsumeTypeOrVoid()).Returns("functionReturnType");
        blockStatementParser.Setup(p => p.Parse()).Returns(body.Object);
    }

    [Fact]
    private void ThrowsExceptionIfTokenConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseFunctionDeclarationStrategy(null, blockStatementParser.Object,
                typeConsumer.Object, parameterListConsumer.Object));
    }

    [Fact]
    private void ThrowsExceptionIfBlockStatementParserIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseFunctionDeclarationStrategy(tokenConsumer.Object, null,
                typeConsumer.Object, parameterListConsumer.Object));
    }

    [Fact]
    private void ThrowsExceptionIfTypeConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseFunctionDeclarationStrategy(tokenConsumer.Object, blockStatementParser.Object,
                null, parameterListConsumer.Object));
    }

    [Fact]
    private void ThrowsExceptionIfParameterListConsumerIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new ParseFunctionDeclarationStrategy(tokenConsumer.Object, blockStatementParser.Object,
                typeConsumer.Object, null));
    }

    [Fact]
    private void ConsumesFunctionToken()
    {
        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.FUNC));
    }

    [Fact]
    private void ConsumesFunctionNameInOrder()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.FUNC)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "className" })
            .Callback(() => Assert.Equal(2, order++));

        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.ID));
    }

    [Fact]
    private void ConsumesOpenParenthesisInOrder()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.FUNC)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "className" })
            .Callback(() => Assert.Equal(2, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(3, order++));

        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.OPEN_PAREN));
    }

    [Fact]
    private void ConsumesParametersAfterOpenParenthesis()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.FUNC)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "className" })
            .Callback(() => Assert.Equal(2, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(3, order++));
        parameterListConsumer.Setup(c => c.Consume()).Returns(parameters.Object)
            .Callback(() => Assert.Equal(4, order++));

        GetStrategy().Parse();
        parameterListConsumer.Verify(c => c.Consume());
    }

    [Fact]
    private void ConsumesCloseParenthesisAfterParameters()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.FUNC)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "className" })
            .Callback(() => Assert.Equal(2, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(3, order++));
        parameterListConsumer.Setup(c => c.Consume()).Returns(parameters.Object)
            .Callback(() => Assert.Equal(4, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Callback(() => Assert.Equal(5, order++));

        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.CLOSE_PAREN));
    }

    [Fact]
    private void ConsumesFunctionReturnTypeAfterCloseParenthesis()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.FUNC)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "className" })
            .Callback(() => Assert.Equal(2, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(3, order++));
        parameterListConsumer.Setup(c => c.Consume()).Returns(parameters.Object)
            .Callback(() => Assert.Equal(4, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Callback(() => Assert.Equal(5, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.COLON)).Callback(() => Assert.Equal(6, order++));
        typeConsumer.Setup(c => c.ConsumeTypeOrVoid()).Returns("functionReturnType")
            .Callback(() => Assert.Equal(7, order++));

        GetStrategy().Parse();
        tokenConsumer.Verify(c => c.Consume(TokenType.COLON));
        typeConsumer.Verify(c => c.ConsumeTypeOrVoid());
    }

    [Fact]
    private void ParsesBlockStatementAfterCloseParenthesis()
    {
        var order = 1;
        tokenConsumer.Setup(c => c.Consume(TokenType.FUNC)).Callback(() => Assert.Equal(1, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "className" })
            .Callback(() => Assert.Equal(2, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.OPEN_PAREN)).Callback(() => Assert.Equal(3, order++));
        parameterListConsumer.Setup(c => c.Consume()).Returns(parameters.Object)
            .Callback(() => Assert.Equal(4, order++));
        tokenConsumer.Setup(c => c.Consume(TokenType.CLOSE_PAREN)).Callback(() => Assert.Equal(5, order++));
        blockStatementParser.Setup(c => c.Parse()).Returns(body.Object).Callback(() => Assert.Equal(6, order++));

        GetStrategy().Parse();
        blockStatementParser.Verify(c => c.Parse());
    }

    [Fact]
    private void ReturnsFunctionDeclarationOnHappyPath()
    {
        tokenConsumer.Setup(c => c.Consume(TokenType.ID)).Returns(new Token { Content = "className" });
        parameterListConsumer.Setup(c => c.Consume()).Returns(parameters.Object);
        typeConsumer.Setup(c => c.ConsumeTypeOrVoid()).Returns("functionReturnType");
        blockStatementParser.Setup(p => p.Parse()).Returns(body.Object);

        var declaration = GetStrategy().Parse();
        Assert.True(declaration is FunctionDeclaration);

        var functionDeclaration = declaration as FunctionDeclaration;
        Assert.Equal("className", functionDeclaration.Name);
        Assert.Equal(parameters.Object, functionDeclaration.Parameters);
        Assert.Equal("functionReturnType", functionDeclaration.ReturnType);
        Assert.Equal(body.Object, functionDeclaration.Body);
    }

    private ParseFunctionDeclarationStrategy GetStrategy()
    {
        return new ParseFunctionDeclarationStrategy(tokenConsumer.Object,
            blockStatementParser.Object, typeConsumer.Object, parameterListConsumer.Object);
    }
}