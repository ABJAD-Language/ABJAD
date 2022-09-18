namespace ABJAD.LexEngine.Characters;

public class CharacterAnalyzer
{
    public static CharacterType AnalyzeCharacterType(char c)
    {
        return c switch
        {
            ')'  => CharacterType.LEFT_PAREN,
            '('  => CharacterType.RIGHT_PAREN,
            '}'  => CharacterType.LEFT_BRACE,
            '{'  => CharacterType.RIGHT_BRACE,
            '.'  => CharacterType.DOT,
            '؛'  => CharacterType.SEMICOLON,
            '،'  => CharacterType.COMMA,
            '-'  => CharacterType.DASH,
            '+'  => CharacterType.PLUS,
            '='  => CharacterType.EQUAL,
            '*'  => CharacterType.STAR,
            '%'  => CharacterType.PERCENTAGE,
            '"'  => CharacterType.DOUBLE_QUOTE,
            '#'  => CharacterType.HASH,
            '!'  => CharacterType.EXCLAMATION_MARK,
            '>'  => CharacterType.LEFT_SINGLE_ANGLE,
            '<'  => CharacterType.RIGHT_SINGLE_ANGLE,
            '&'  => CharacterType.AMPERSAND,
            '|'  => CharacterType.VERTICAL_BAR,
            '\\' => CharacterType.SLASH,
            '\r' => CharacterType.WHITE_SPACE,
            '\n' => CharacterType.WHITE_SPACE,
            '\t' => CharacterType.WHITE_SPACE,
            ' '  => CharacterType.WHITE_SPACE,
            _    => CharacterType.LITERAL
        };
    }
}