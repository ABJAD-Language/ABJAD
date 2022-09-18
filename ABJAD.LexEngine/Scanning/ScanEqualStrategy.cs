using ABJAD.LexEngine.Tokens;

namespace ABJAD.LexEngine.Scanning;

public class ScanEqualStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '=')
        {
            return ScanDoubleEqualToken(current, line, lineIndex);
        }

        return ScanSingleEqualToken(current, line, lineIndex);
    }

    private static Token ScanSingleEqualToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex, Label = "=", Type = TokenType.EQUAL
        };
    }

    private static Token ScanDoubleEqualToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + 1, Label = "==", Type = TokenType.EQUAL_EQUAL
        };
    }
}