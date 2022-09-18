using System;
using System.Collections.Generic;
using System.Linq;
using ABJAD.LexEngine;
using ABJAD.LexEngine.Tokens;
using Moq;
using Xunit;

namespace ABJAD.LexEngine.Test;

public class LexerTest
{
    private Lexer lexer;

    public LexerTest()
    {
        lexer = new Lexer(new StringUtils());
    }

    [Fact]
    private void InitializingLexerCallsStringUtilsIgnoreCaseSensitivity()
    {
        var stringUtils = new Mock<StringUtils>();
        stringUtils.Setup(m => m.IgnoreCaseSensitivity("code")).Returns("");

        lexer = new Lexer(stringUtils.Object);
        lexer.Lex("code");
        stringUtils.Verify(mock => mock.IgnoreCaseSensitivity("code"));
    }

    [Fact]
    private void ThrowsNullExceptionWhenCodeIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => lexer.Lex(null));
    }

    [Fact]
    private void ReturnsEmptyListForEmptyCode()
    {
        Assert.Empty(lexer.Lex(""));
    }

    [Fact]
    private void ReturnsListOfOneElementWhenCodeContainsSingleCharacter()
    {
        var tokens = lexer.Lex(")");
        Assert.Single(tokens);
        Assert.Equal(TokenType.CLOSE_PAREN, tokens[0].Type);
        Assert.Equal(1, tokens[0].StartIndex);
        Assert.Equal(1, tokens[0].EndIndex);
        Assert.Equal(1, tokens[0].StartLine);
    }

    [Fact]
    private void ReturnsListOfThreeElementWhenCodeContainsTwoCharactersSeparatedByNewLine()
    {
        var tokens = lexer.Lex(")\n!");
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
        var expectedTokens = new List<Token>
        {
            new()
            {
                StartIndex = 1,
                StartLine = 1,
                StartLineIndex = 1,
                EndIndex = 4, 
                EndLineIndex = 4,
                Label = "اكتب",
                Type = TokenType.PRINT
            },
            new()
            {
                StartIndex = 5,
                StartLine = 1,
                StartLineIndex = 5,
                EndIndex = 5, 
                EndLineIndex = 5,
                Label = "(",
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
                Label = ")",
                Type = TokenType.CLOSE_PAREN
            },
            new()
            {
                StartIndex = 22,
                StartLine = 1,
                StartLineIndex = 22,
                EndIndex = 22, 
                EndLineIndex = 22,
                Label = "؛",
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
                Label = "\n\n",
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 25,
                StartLine = 3,
                StartLineIndex = 1,
                EndIndex = 28, 
                EndLineIndex = 4,
                Label = "ارجع",
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
                Label = " ",
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 30,
                StartLine = 3,
                StartLineIndex = 6,
                EndIndex = 33, 
                EndLineIndex = 9,
                Label = "صحيح",
                Type = TokenType.TRUE
            },
            new()
            {
                StartIndex = 34,
                StartLine = 3,
                StartLineIndex = 10,
                EndIndex = 34, 
                EndLineIndex = 10,
                Label = "؛",
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
                Label = "\n",
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 36,
                StartLine = 4,
                StartLineIndex = 1,
                EndIndex = 39, 
                EndLineIndex = 4,
                Label = "اكتب",
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
                Label = "  ",
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 42,
                StartLine = 4,
                StartLineIndex = 7,
                EndIndex = 42,
                EndLineIndex = 7,
                Label = "(",
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
                Label = ")",
                Type = TokenType.CLOSE_PAREN
            },
            new()
            {
                StartIndex = 47,
                StartLine = 4,
                StartLineIndex = 12,
                EndIndex = 47, 
                EndLineIndex = 12,
                Label = "؛",
                Type = TokenType.SEMICOLON
            },
        };

        var actualTokens = lexer.Lex("أكتب(\"مرحبا بالعالم\")؛\n\nارجع صحيح؛\nأكتب  (123)؛");
        Assert.Equal(expectedTokens, actualTokens);
    }

    [Fact(DisplayName = "AnalyzeRandomTokensContainingComment")]
    private void AnalyzeRandomTokensContainingComment()
    {
        var code = "أكتب\n# هذا تعليق\n==\n";
        var expectedTokens = new List<Token>
        {
            new()
            {
                StartIndex = 1,
                StartLine = 1,
                StartLineIndex = 1,
                EndIndex = 4, 
                EndLineIndex = 4,
                Label = "اكتب",
                Type = TokenType.PRINT
            },
            new()
            {
                StartIndex = 5,
                StartLine = 1,
                StartLineIndex = 5,
                EndIndex = 5, 
                EndLineIndex = 1,
                EndLine = 2,
                Label = "\n",
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 6,
                StartLine = 2,
                StartLineIndex = 1,
                EndIndex = 16, 
                EndLineIndex = 11,
                Label = " هذا تعليق",
                Type = TokenType.COMMENT
            },
            new()
            {
                StartIndex = 17,
                StartLine = 2,
                StartLineIndex = 12,
                EndIndex = 17, 
                EndLineIndex = 1,
                EndLine = 3,
                Label = "\n",
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 18,
                StartLine = 3,
                StartLineIndex = 1,
                EndIndex = 19, 
                EndLineIndex = 2,
                Label = "==",
                Type = TokenType.EQUAL_EQUAL
            },
            new()
            {
                StartIndex = 20,
                StartLine = 3,
                StartLineIndex = 3,
                EndIndex = 20, 
                EndLineIndex = 1,
                EndLine = 4,
                Label = "\n",
                Type = TokenType.WHITE_SPACE
            },
           
        };
        
        var actualTokens = lexer.Lex(code);
        var diff = actualTokens.Where(t => !expectedTokens.Contains(t)).ToList();
        Assert.Equal(expectedTokens, actualTokens);
    }

    [Fact(DisplayName = "AnalyzeThreeStatementsIncludingComment")]
    private void AnalyzeThreeStatementsIncludingComment()
    {
        var code = "متغير ب = 2؛\n# هذا تعليق\n==\n";
        var expectedTokens = new List<Token>
        {
            new()
            {
                StartIndex = 1,
                StartLine = 1,
                StartLineIndex = 1,
                EndIndex = 5, 
                EndLineIndex = 5,
                Label = "متغير",
                Type = TokenType.VAR
            },
            new()
            {
                StartIndex = 6,
                StartLine = 1,
                StartLineIndex = 6,
                EndIndex = 6, 
                EndLineIndex = 6,
                EndLine = 1,
                Label = " ",
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 7,
                StartLine = 1,
                StartLineIndex = 7,
                EndIndex = 7, 
                EndLineIndex = 7,
                Label = "ب",
                Type = TokenType.ID
            },
            new()
            {
                StartIndex = 8,
                StartLine = 1,
                StartLineIndex = 8,
                EndIndex = 8, 
                EndLineIndex = 8,
                EndLine = 1,
                Label = " ",
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 9,
                StartLine = 1,
                StartLineIndex = 9,
                EndIndex = 9, 
                EndLineIndex = 9,
                Label = "=",
                Type = TokenType.EQUAL
            },
            new()
            {
                StartIndex = 10,
                StartLine = 1,
                StartLineIndex = 10,
                EndIndex = 10, 
                EndLineIndex = 10,
                EndLine = 1,
                Label = " ",
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 11,
                StartLine = 1,
                StartLineIndex = 11,
                EndIndex = 11, 
                EndLineIndex = 11,
                Label = "2",
                Type = TokenType.NUMBER_CONST
            },
            new()
            {
                StartIndex = 12,
                StartLine = 1,
                StartLineIndex = 12,
                EndIndex = 12, 
                EndLineIndex = 12,
                Label = "؛",
                Type = TokenType.SEMICOLON
            },
            new()
            {
                StartIndex = 13,
                StartLine = 1,
                StartLineIndex = 13,
                EndIndex = 13, 
                EndLineIndex = 1,
                EndLine = 2,
                Label = "\n",
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 14,
                StartLine = 2,
                StartLineIndex = 1,
                EndIndex = 24, 
                EndLineIndex = 11,
                Label = " هذا تعليق",
                Type = TokenType.COMMENT
            },
            new()
            {
                StartIndex = 25,
                StartLine = 2,
                StartLineIndex = 12,
                EndIndex = 25, 
                EndLineIndex = 1,
                EndLine = 3,
                Label = "\n",
                Type = TokenType.WHITE_SPACE
            },
            new()
            {
                StartIndex = 26,
                StartLine = 3,
                StartLineIndex = 1,
                EndIndex = 27, 
                EndLineIndex = 2,
                Label = "==",
                Type = TokenType.EQUAL_EQUAL
            },
            new()
            {
                StartIndex = 28,
                StartLine = 3,
                StartLineIndex = 3,
                EndIndex = 28, 
                EndLineIndex = 1,
                EndLine = 4,
                Label = "\n",
                Type = TokenType.WHITE_SPACE
            }
        };
        
        var actualTokens = lexer.Lex(code);
        Assert.Equal(expectedTokens, actualTokens);
    }
}