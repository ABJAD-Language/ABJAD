using ABJAD.Lexer.Domain.Tokens;

namespace ABJAD.Lexer.Domain.Scanning;

public class ScanningContext
{
    private readonly ScanningStrategy strategy;

    public ScanningContext(ScanningStrategy strategy)
    {
        this.strategy = strategy;
    }

    public Token Execute(string code, int current, int line, int lineIndex)
    {
        return strategy.Scan(code, current, line, lineIndex);
    }
}