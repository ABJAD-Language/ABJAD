using Ardalis.GuardClauses;
using LexEngine.Characters;
using LexEngine.Tokens;

namespace LexEngine;

public class Lexer : ILexer
{
    private readonly string code;
    private readonly StringUtils stringUtils;

    private int line = 1;
    private int lineIndex = 0;
    private int current = 0;
    private List<Token> tokens;

    public Lexer(string code, StringUtils stringUtils)
    {
        Guard.Against.Null(code);
        this.stringUtils = stringUtils;
        this.code = stringUtils.IgnoreCaseSensitivity(code);
    }
    
    public List<Token> Lex()
    {
        tokens = new List<Token>();
        char c;
        while (HasNext(out c))
        {
            var characterType = CharacterAnalyzer.AnalyzeCharacterType(c);
            var token = TokenScanner.ScanToken(code, current, line, characterType);
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