using LexEngine.Tokens;

namespace LexEngine.Scanning;

public interface ScanningStrategy
{
    Token Scan(string code, int current, int line, int lineIndex);
}