using ABJAD.LexEngine.Characters;
using ABJAD.LexEngine.Scanning;

namespace ABJAD.LexEngine.Tokens;

public static class TokenScanner
{
    public static Token ScanToken(string code, int current, int startLineIndex, int line, CharacterType characterType)
    {
        var scanningStrategy = GenerateScanningStrategy(startLineIndex, line, characterType);
        return new ScanningContext(scanningStrategy).Execute(code, current, line, startLineIndex);
    }
    
    private static ScanningStrategy GenerateScanningStrategy(int startLineIndex, int line, CharacterType characterType)
    {
        return characterType switch
        {
            CharacterType.RIGHT_PAREN        => new ScanOpenParenthesisStrategy(),
            CharacterType.LEFT_PAREN         => new ScanCloseParenthesisStrategy(),
            CharacterType.RIGHT_BRACE        => new ScanOpenBraceStrategy(),
            CharacterType.LEFT_BRACE         => new ScanCloseBraceStrategy(),
            CharacterType.SEMICOLON          => new ScanSemicolonStrategy(),
            CharacterType.COLON              => new ScanColonStrategy(),
            CharacterType.COMMA              => new ScanCommaStrategy(),
            CharacterType.DOT                => new ScanDotStrategy(),
            CharacterType.PERCENTAGE         => new ScanModuloStrategy(),
            CharacterType.DASH               => new ScanDashStrategy(),
            CharacterType.PLUS               => new ScanPlusStrategy(),
            CharacterType.STAR               => new ScanStarStrategy(),
            CharacterType.SLASH              => new ScanSlashStrategy(),
            CharacterType.EQUAL              => new ScanEqualStrategy(),
            CharacterType.EXCLAMATION_MARK   => new ScanExclamationMarkStrategy(),
            CharacterType.RIGHT_SINGLE_ANGLE => new ScanRightAngleSignStrategy(),
            CharacterType.LEFT_SINGLE_ANGLE  => new ScanLeftAngleSignStrategy(),
            CharacterType.AMPERSAND          => new ScanAmpersandSignStrategy(),
            CharacterType.VERTICAL_BAR       => new ScanVerticalBarStrategy(),
            CharacterType.HASH               => new ScanCommentStrategy(),
            CharacterType.WHITE_SPACE        => new ScanWhiteSpaceStrategy(),
            CharacterType.DOUBLE_QUOTE       => new ScanStringStrategy(),
            CharacterType.LITERAL            => new ScanLiteralStrategy(),
            _                                => throw new InvalidTokenException(line, startLineIndex)
        };
    }
}