using ABJAD.LexEngine.Tokens;

namespace ABJAD.LexEngine.Scanning;

public class ScanPlusStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '+')
        {
            return ScanDoublePlusToken(current, line, lineIndex);
        }

        if (code.Length > current && code[current] == '=')
        {
            return ScanPlusEqualToken(current, line, lineIndex);
        }

        return ScanSinglePlusToken(current, line, lineIndex);
    }

    private static Token ScanSinglePlusToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex, Type = TokenType.PLUS, Label = "+"
        };
    }

    private static Token ScanDoublePlusToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + 1, Type = TokenType.PLUS_PLUS, Label = "++"
        };
    }

    private static Token ScanPlusEqualToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current, EndIndex = current + 1, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + 1, Type = TokenType.PLUS_EQUAL, Label = "+="
        };
    }
}