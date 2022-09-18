using ABJAD.LexEngine.Tokens;

namespace ABJAD.LexEngine.Scanning;

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