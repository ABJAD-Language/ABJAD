using System.Text;
using ABJAD.LexEngine.Characters;
using ABJAD.LexEngine.Tokens;

namespace ABJAD.LexEngine.Scanning;

public class ScanWhiteSpaceStrategy : ScanningStrategy
{
    private int _line;
    private int _lineIndex;
    private int _current;
    private string _code;
    private StringBuilder _content;

    public Token Scan(string code, int current, int line, int lineIndex)
    {
        _line = line;
        _lineIndex = lineIndex;
        _current = current;
        _code = code;
        _content = new StringBuilder();

        if (_code[_current-1] == '\n')
        {
            HandleNewLine();
        }
        _content.Append(_code[_current-1]);
        
        ScanWhiteSpace();

        return new Token
        {
            StartIndex = current, EndIndex = _current, StartLine = line, StartLineIndex = lineIndex,
            EndLineIndex = _lineIndex, EndLine = _line, Label = _content.ToString(), Type = TokenType.WHITE_SPACE
        };
    }

    private void ScanWhiteSpace()
    {
        while (_code.Length > _current && CurrentCharacterIsWhiteSpace())
        {
            if (CurrentCharacterIsNewLine())
            {
                HandleNewLine();
            }
            else
            {
                _lineIndex++;
            }

            _content.Append(_code[_current]);
            _current++;
        }
    }

    private bool CurrentCharacterIsNewLine()
    {
        return _code[_current] == '\n';
    }

    private void HandleNewLine()
    {
        _line++;
        _lineIndex = 1;
    }

    private bool CurrentCharacterIsWhiteSpace()
    {
        return CharacterAnalyzer.AnalyzeCharacterType(_code[_current]) == CharacterType.WHITE_SPACE;
    }
}