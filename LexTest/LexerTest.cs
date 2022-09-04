using System;
using System.Collections.Generic;
using LexEngine;
using LexEngine.Tokens;
using Moq;
using Xunit;

namespace LexTest;

public class LexerTest
{
    [Fact]
    private void InitializingLexerCallsStringUtilsIgnoreCaseSensitivity()
    {
        var stringUtils = new Mock<StringUtils>();
        
        new Lexer("code", stringUtils.Object);
        stringUtils.Verify(mock => mock.IgnoreCaseSensitivity("code"));
    }

    [Fact]
    private void ThrowsNullExceptionWhenCodeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new Lexer(null, new StringUtils()));
    }

    [Fact]
    private void ReturnsEmptyListForEmptyCode()
    {
        var lexer = new Lexer("", new StringUtils());
        Assert.Empty(lexer.Lex());
    }

    [Fact]
    private void ReturnsListOfOneElementWhenCodeContainsSingleCharacter()
    {
        var lexer = new Lexer(")", new StringUtils());
        var tokens = lexer.Lex();
        Assert.Single(tokens);
        Assert.Equal(TokenType.CLOSE_PAREN, tokens[0].Type);
        Assert.Equal(1, tokens[0].StartIndex);
        Assert.Equal(1, tokens[0].EndIndex);
        Assert.Equal(1, tokens[0].StartLine);
    }

    [Fact]
    private void ReturnsListOfThreeElementWhenCodeContainsTwoCharactersSeparatedByNewLine()
    {
        var lexer = new Lexer(")\n!", new StringUtils());
        var tokens = lexer.Lex();
        Assert.Equal(3, tokens.Count);

        var parenthesisToken = tokens[0];
        Assert.Equal(TokenType.CLOSE_PAREN, parenthesisToken.Type);
        Assert.Equal(1, parenthesisToken.StartIndex);
        Assert.Equal(1, parenthesisToken.EndIndex);
        Assert.Equal(1, parenthesisToken.StartLine);
        Assert.Equal(1, parenthesisToken.StartLineIndex);
        Assert.Equal(1, parenthesisToken.EndLineIndex);

        var whiteSpaceToken = tokens[1];
        Assert.Equal(TokenType.WHITE_SPACE, whiteSpaceToken.Type);
        Assert.Equal(2, whiteSpaceToken.StartIndex);
        Assert.Equal(2, whiteSpaceToken.EndIndex);
        Assert.Equal(1, whiteSpaceToken.StartLine);
        Assert.Equal(2, whiteSpaceToken.EndIndex);
        Assert.Equal(2, whiteSpaceToken.StartLineIndex);
        Assert.Equal(1, whiteSpaceToken.EndLineIndex);

        var bangToken = tokens[2];
        Assert.Equal(TokenType.BANG, bangToken.Type);
        Assert.Equal(3, bangToken.StartIndex);
        Assert.Equal(3, bangToken.EndIndex);
        Assert.Equal(2, bangToken.StartLine);
        Assert.Equal(1, bangToken.StartLineIndex);
        Assert.Equal(1, bangToken.EndLineIndex);
    }

    [Fact]
    private void LexThreeStatements()
    {
        var lexer = new Lexer("أكتب(\"مرحبا بالعالم\")؛\n\nارجع صحيح؛\nأكتب  (123)؛", new StringUtils());

        var expectedTokens = new List<Token>
        {
            new()
            {
                StartIndex = 1,
                StartLine = 1,
                StartLineIndex = 1,
                EndIndex = 4, 
                EndLineIndex = 4,
                Type = TokenType.PRINT
            },
            new()
            {
                StartIndex = 5,
                StartLine = 1,
                StartLineIndex = 5,
                EndIndex = 5, 
                EndLineIndex = 5,
                Type = TokenType.OPEN_PAREN
            },
            new()
            {
                StartIndex = 6,
                StartLine = 1,
                StartLineIndex = 6,
                EndIndex = 20, 
                EndLineIndex = 20,
                Type = TokenType.STRING_CONST,
                Label = "مرحبا بالعالم"
            },
            new()
            {
                StartIndex = 21,
                StartLine = 1,
                StartLineIndex = 21,
                EndIndex = 21, 
                EndLineIndex = 21,
                Type = TokenType.CLOSE_PAREN
            },
            new()
            {
                StartIndex = 22,
                StartLine = 1,
                StartLineIndex = 22,
                EndIndex = 22, 
                EndLineIndex = 22,
                Type = TokenType.SEMICOLON
            },
            new()
            {
                StartIndex = 23,
                StartLine = 1,
                StartLineIndex = 23,
                EndIndex = 24, 
                EndLineIndex = 1,
                EndLine = 3,
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 25,
                StartLine = 3,
                StartLineIndex = 1,
                EndIndex = 28, 
                EndLineIndex = 4,
                Type = TokenType.RETURN
            },
            new()
            {
                StartIndex = 29,
                StartLine = 3,
                StartLineIndex = 5,
                EndIndex = 29,
                EndLine = 3,
                EndLineIndex = 5,
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 30,
                StartLine = 3,
                StartLineIndex = 6,
                EndIndex = 33, 
                EndLineIndex = 9,
                Type = TokenType.TRUE
            },
            new()
            {
                StartIndex = 34,
                StartLine = 3,
                StartLineIndex = 10,
                EndIndex = 34, 
                EndLineIndex = 10,
                Type = TokenType.SEMICOLON
            },
            new()
            {
                StartIndex = 35,
                StartLine = 3,
                StartLineIndex = 11,
                EndIndex = 35, 
                EndLine = 4, 
                EndLineIndex = 1,
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 36,
                StartLine = 4,
                StartLineIndex = 1,
                EndIndex = 39, 
                EndLineIndex = 4,
                Type = TokenType.PRINT
            },
            new()
            {
                StartIndex = 40,
                StartLine = 4,
                StartLineIndex = 5,
                EndIndex = 41, 
                EndLineIndex = 6,
                EndLine = 4,
                Type = TokenType.WHITE_SPACE
            },
            
            new()
            {
                StartIndex = 42,
                StartLine = 4,
                StartLineIndex = 7,
                EndIndex = 42,
                EndLineIndex = 7,
                Type = TokenType.OPEN_PAREN
            },
            new()
            {
                StartIndex = 43,
                StartLine = 4,
                StartLineIndex = 8,
                EndIndex = 45, 
                EndLineIndex = 10,
                Type = TokenType.NUMBER_CONST,
                Label = "123"
            },
            new()
            {
                StartIndex = 46,
                StartLine = 4,
                StartLineIndex = 11,
                EndIndex = 46, 
                EndLineIndex = 11,
                Type = TokenType.CLOSE_PAREN
            },
            new()
            {
                StartIndex = 47,
                StartLine = 4,
                StartLineIndex = 12,
                EndIndex = 47, 
                EndLineIndex = 12,
                Type = TokenType.SEMICOLON
            },
        };

        var actualTokens = lexer.Lex();
        Assert.Equal(expectedTokens, actualTokens);
    }
}