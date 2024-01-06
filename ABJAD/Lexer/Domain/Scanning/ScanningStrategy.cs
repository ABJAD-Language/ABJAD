using ABJAD.Lexer.Domain.Tokens;

namespace ABJAD.Lexer.Domain.Scanning;

public interface ScanningStrategy
{
    Token Scan(string code, int current, int line, int lineIndex);
}