using System.Text;
using LexEngine.Tokens;

namespace LexEngine.Scanning;

public class ScanStringStrategy : ScanningStrategy
{
    private int _line;
    private int _lineIndex;
    private int _current;
    private string _code;
    private StringBuilder _content;

    private readonly List<char> escapedCharacters = new() {'"', '\\'};
    
    public Token Scan(string code, int current, int line, int lineIndex)
    {
        _line = line;
        _lineIndex = lineIndex;
        _current = current;
        _code = code;
        _content = new StringBuilder();

        while (NotEndOfCode() && !EndOfString())
        {
            GuardAgainstNewLineCharacter();
            if (_code[_current] == '\\')
            {
                HandleEscapedCharacter();
            }

            _content.Append(_code[_current]);
            MoveForwardCharacter();
        }

        if (NotEndOfCode() && EndOfString())
        {
            MoveForwardCharacter();
        }
        else
        {
            throw new MissingTokenException(_lineIndex, _line, "\"");
        }

        return new Token
        {
            StartIndex = current, EndIndex = _current, Label = _content.ToString(), StartLine = _line,
            StartLineIndex = lineIndex, EndLineIndex = _lineIndex, Type = TokenType.STRING_CONST
        };
    }

    private void HandleEscapedCharacter()
    {
        if (_code.Length > _current + 1 && escapedCharacters.Contains(_code[_current + 1]))
        {
            MoveForwardCharacter();
        }
        else
        {
            throw new InvalidTokenException(_line, _lineIndex + 1);
        }
    }

    private bool EndOfString()
    {
        return _code[_current] == '\"';
    }

    private bool NotEndOfCode()
    {
        return _code.Length > _current;
    }

    private void MoveForwardCharacter()
    {
        _current++;
        _lineIndex++;
    }

    private void GuardAgainstNewLineCharacter()
    {
        if (_code[_current] == '\n')
        {
            throw new MissingTokenException(_lineIndex + 1, _line, "\"");
        }
    }
}