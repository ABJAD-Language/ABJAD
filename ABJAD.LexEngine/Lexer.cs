using ABJAD.LexEngine.Characters;
using ABJAD.LexEngine.Tokens;
using Ardalis.GuardClauses;

namespace ABJAD.LexEngine;

public class Lexer : ILexer
{
    public readonly StringUtils stringUtils;
    private string code;

    private int line;
    private int lineIndex;
    private int current;
    private List<Token> tokens;

    public Lexer(StringUtils stringUtils)
    {
        this.stringUtils = stringUtils;
    }

    public Lexer()
    {
        // TODO create default implementation of pre-analysis code checker
        stringUtils = new StringUtils();
    }
    
    public List<Token> Lex(string _code)
    {
        Guard.Against.Null(_code);
        code = stringUtils.IgnoreCaseSensitivity(_code);
        
        line = 1;
        lineIndex = 1;
        current = 0;
        
        tokens = new List<Token>();
        char c;
        // TODO refactor
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