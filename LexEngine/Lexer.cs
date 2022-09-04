using Ardalis.GuardClauses;
using LexEngine.Characters;
using LexEngine.Tokens;

namespace LexEngine;

public class Lexer : ILexer
{
    private readonly string code;

    private int line;
    private int lineIndex;
    private int current;
    private List<Token> tokens;

    public Lexer(string code, StringUtils stringUtils)
    {
        Guard.Against.Null(code);
        this.code = stringUtils.IgnoreCaseSensitivity(code);
    }
    
    public List<Token> Lex()
    {
        line = 1;
        lineIndex = 1;
        current = 0;
        
        tokens = new List<Token>();
        char c;
        while (HasNext(out c))
        {
            var characterType = CharacterAnalyzer.AnalyzeCharacterType(c);
            var token = TokenScanner.ScanToken(code, current, lineIndex, line, characterType);
            
            current = token.EndIndex?? current;
            if (token.EndLine != null && token.StartLine != token.EndLine)
            {
                line = (int) token.EndLine;
                lineIndex = 1;
            }
            else
            {
                lineIndex = token.EndLineIndex + 1;
            }
            
            tokens.Add(token);
        }
        
        return tokens;
    }

    private bool HasNext(out char c)
    {
        if (current >= code.Length)
        {
            c = default;
            return false;
        }

        c = code[current];
        current++;
        return true;
    }

    
}