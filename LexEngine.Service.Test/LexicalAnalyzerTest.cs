using System.Collections.Generic;
using LexEngine.Service.Core;
using LexEngine.Tokens;
using Moq;
using Xunit;

namespace LexEngine.Service.Test;

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
                Type = "PRINT"
            },
            new()
            {
                Line = 1,
                Index = 5,
                Type = "OPEN_PAREN"
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
                Type = "CLOSE_PAREN"
            },
            new()
            {
                Line = 1,
                Index = 22,
                Type = "SEMICOLON"
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
            }
        };
    }
}