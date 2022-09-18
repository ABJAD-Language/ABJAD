using System.Collections.Generic;
using ABJAD.LexEngine;
using ABJAD.LexEngine.Service.Core;
using ABJAD.LexEngine.Tokens;
using Moq;
using Xunit;

namespace ABJAD.LexEngine.Service.Test.Core;

public class LexicalAnalyzerTest
{
    private readonly LexicalAnalyzer lexicalAnalyzer;
    private readonly Mock<ILexer> lexer;

    public LexicalAnalyzerTest()
    {
        lexer = new Mock<ILexer>();
        lexicalAnalyzer = new LexicalAnalyzer(lexer.Object);
    }

    [Fact]
    public void CallsLexerOnInput()
    {
        lexer.Setup(m => m.Lex("ولد")).Returns(new List<Token>());
        lexicalAnalyzer.AnalyzeCode("ولد");
        lexer.Verify(m => m.Lex("ولد"));
    }

    [Fact]
    public void ReturnsCorrectResult()
    {
        lexer.Setup(m => m.Lex("ولد")).Returns(GetTokens());
        var lexicalTokens = lexicalAnalyzer.AnalyzeCode("ولد");
        Assert.Equal(GetLexicalTokens(), lexicalTokens);
    }

    private static List<LexicalToken> GetLexicalTokens()
    {
        return new List<LexicalToken>
        {
            new()
            {
                Line = 1,
                Index = 1,
                Type = "PRINT",
                Content = "اكتب"
            },
            new()
            {
                Line = 1,
                Index = 5,
                Type = "OPEN_PAREN",
                Content = "("
            },
            new()
            {
                Line = 1,
                Index = 6,
                Type = "STRING_CONST",
                Content = "مرحبا بالعالم"
            },
            new()
            {
                Line = 1,
                Index = 21,
                Type = "CLOSE_PAREN",
                Content = ")"
            },
            new()
            {
                Line = 1,
                Index = 22,
                Type = "SEMICOLON",
                Content = "؛"
            }
        };
    }

    private static List<Token> GetTokens()
    {
        return new List<Token>
        {
            new()
            {
                StartIndex = 1,
                StartLine = 1,
                StartLineIndex = 1,
                EndIndex = 4,
                EndLineIndex = 4,
                Type = TokenType.PRINT,
                Label = "اكتب"
            },
            new()
            {
                StartIndex = 5,
                StartLine = 1,
                StartLineIndex = 5,
                EndIndex = 5,
                EndLineIndex = 5,
                Type = TokenType.OPEN_PAREN,
                Label = "("
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
                Type = TokenType.CLOSE_PAREN,
                Label = ")"
            },
            new()
            {
                StartIndex = 22,
                StartLine = 1,
                StartLineIndex = 22,
                EndIndex = 22,
                EndLineIndex = 22,
                Type = TokenType.SEMICOLON,
                Label = "؛"
            }
        };
    }
}