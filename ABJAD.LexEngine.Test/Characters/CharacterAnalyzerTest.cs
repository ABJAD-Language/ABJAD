using ABJAD.LexEngine.Characters;
using Xunit;

namespace ABJAD.LexEngine.Test.Characters;

public class CharacterAnalyzerTest
{
    [Theory]
    [InlineData(')', CharacterType.LEFT_PAREN)]
    [InlineData('(', CharacterType.RIGHT_PAREN)]
    [InlineData('}', CharacterType.LEFT_BRACE)]
    [InlineData('{', CharacterType.RIGHT_BRACE)]
    [InlineData('.', CharacterType.DOT)]
    [InlineData('؛', CharacterType.SEMICOLON)]
    [InlineData('،', CharacterType.COMMA)]
    [InlineData('-', CharacterType.DASH)]
    [InlineData('+', CharacterType.PLUS)]
    [InlineData('=', CharacterType.EQUAL)]
    [InlineData('*', CharacterType.STAR)]
    [InlineData('%', CharacterType.PERCENTAGE)]
    [InlineData('"', CharacterType.DOUBLE_QUOTE)]
    [InlineData('#', CharacterType.HASH)]
    [InlineData('!', CharacterType.EXCLAMATION_MARK)]
    [InlineData('>', CharacterType.LEFT_SINGLE_ANGLE)]
    [InlineData('<', CharacterType.RIGHT_SINGLE_ANGLE)]
    [InlineData('&', CharacterType.AMPERSAND)]
    [InlineData('|', CharacterType.VERTICAL_BAR)]
    [InlineData('\\', CharacterType.SLASH)]
    [InlineData('\r', CharacterType.WHITE_SPACE)]
    [InlineData('\t', CharacterType.WHITE_SPACE)]
    [InlineData('\n', CharacterType.WHITE_SPACE)]
    [InlineData(' ', CharacterType.WHITE_SPACE)]
    [InlineData('1', CharacterType.LITERAL)]
    [InlineData('_', CharacterType.LITERAL)]
    [InlineData('d', CharacterType.LITERAL)]
    private void ReturnCorrectTokenTypes(char c, CharacterType type)
    {
        Assert.Equal(type, CharacterAnalyzer.AnalyzeCharacterType(c));
    }
}