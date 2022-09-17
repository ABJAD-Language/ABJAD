using System.Text.RegularExpressions;

namespace LexEngine.Tokens;

public class WordScanner
{
    public static string ScanNextWord(string code, int index, int line)
    {
        var words = Regex.Split(code[index..], Patterns.WordTerminalRegex);
        if (Regex.IsMatch(words[0], Patterns.LiteralRegex))
        {
            return words[0];
        }

        throw new InvalidWordException(index + 1, line, words[0]);
    }

    public static string ScanNextNumber(string code, int index, int line)
    {
        var numbers = Regex.Split(code[index..], Patterns.NumberTerminalRegex);
        if (Regex.IsMatch(numbers[0], Patterns.NumberRegex))
        {
            return numbers[0];
        }
        
        throw new InvalidWordException(index + 1, line, numbers[0]);
    }
}