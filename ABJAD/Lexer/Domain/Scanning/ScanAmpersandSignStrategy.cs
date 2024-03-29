using ABJAD.Lexer.Domain.Tokens;

namespace ABJAD.Lexer.Domain.Scanning;

public class ScanAmpersandSignStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        if (code.Length <= current || code[current] != '&')
        {
            throw new MissingTokenException(lineIndex, line, "&");
        }

        return new Token
        {
            StartIndex = current,
            EndIndex = current + 1,
            StartLine = line,
            StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + 1,
            Label = "&&",
            Type = TokenType.AND
        };
    }
}