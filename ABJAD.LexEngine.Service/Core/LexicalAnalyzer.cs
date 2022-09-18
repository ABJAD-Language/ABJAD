using ABJAD.LexEngine.Tokens;

namespace ABJAD.LexEngine.Service.Core;

public class LexicalAnalyzer : Analyzer
{
    private readonly ILexer lexer;

    public LexicalAnalyzer(ILexer lexer)
    {
        this.lexer = lexer;
    }

    public List<LexicalToken> AnalyzeCode(string code)
    {
        var tokens = lexer.Lex(code);
        return tokens.Select(MapToLexicalToken).ToList();
    }

    private static LexicalToken MapToLexicalToken(Token t)
    {
        return new LexicalToken
        {
            Line = t.StartLine ?? 0,
            Index = t.StartLineIndex,
            Content = t.Label,
            Type = t.Type.ToString()
        };
    }
}