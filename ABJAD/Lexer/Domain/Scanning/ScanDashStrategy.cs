using ABJAD.Lexer.Domain.Tokens;

namespace ABJAD.Lexer.Domain.Scanning;

public class ScanDashStrategy : ScanningStrategy
{
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        if (code.Length > current && code[current] == '-')
        {
            return ScanDoubleDashToken(current, line, lineIndex);
        }

        if (code.Length > current && code[current] == '=')
        {
            return ScanDashEqualToken(current, line, lineIndex);
        }

        return ScanSingleDashToken(current, line, lineIndex);
    }

    private static Token ScanSingleDashToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current,
            EndIndex = current,
            StartLine = line,
            StartLineIndex = lineIndex,
            EndLineIndex = lineIndex,
            Type = TokenType.DASH,
            Label = "-"
        };
    }

    private static Token ScanDoubleDashToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current,
            EndIndex = current + 1,
            StartLine = line,
            StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + 1,
            Type = TokenType.DASH_DASH,
            Label = "--"
        };
    }

    private static Token ScanDashEqualToken(int current, int line, int lineIndex)
    {
        return new Token
        {
            StartIndex = current,
            EndIndex = current + 1,
            StartLine = line,
            StartLineIndex = lineIndex,
            EndLineIndex = lineIndex + 1,
            Type = TokenType.DASH_EQUAL,
            Label = "-="
        };
    }
}