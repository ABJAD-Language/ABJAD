using ABJAD.Lexer.Domain.Tokens;

namespace ABJAD.Lexer.Domain.Scanning;

public class ScanOpenParenthesisStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current,
            EndIndex = current,
            StartLine = line,
            StartLineIndex = lineIndex,
            EndLineIndex = lineIndex,
            Type = TokenType.OPEN_PAREN,
            Label = "("
        };
    }
}