using System;
using System.Text.RegularExpressions;
using ABJAD.LexEngine;
using Xunit;

namespace ABJAD.LexEngine.Test;

public class PatternsTest
{
    public class DigitPatternTest
    {
        [Fact]
        public void ZeroMatchesPattern()
        {
            var isMatch = Regex.IsMatch("0", Patterns.DigitRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void OneMatchesPattern()
        {
            var isMatch = Regex.IsMatch("1", Patterns.DigitRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void TwoMatchesPattern()
        {
            var isMatch = Regex.IsMatch("2", Patterns.DigitRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void ThreeMatchesPattern()
        {
            var isMatch = Regex.IsMatch("3", Patterns.DigitRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void FourMatchesPattern()
        {
            var isMatch = Regex.IsMatch("4", Patterns.DigitRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void FiveMatchesPattern()
        {
            var isMatch = Regex.IsMatch("5", Patterns.DigitRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void SixMatchesPattern()
        {
            var isMatch = Regex.IsMatch("6", Patterns.DigitRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void SevenMatchesPattern()
        {
            var isMatch = Regex.IsMatch("7", Patterns.DigitRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void EightMatchesPattern()
        {
            var isMatch = Regex.IsMatch("8", Patterns.DigitRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void NineMatchesPattern()
        {
            var isMatch = Regex.IsMatch("9", Patterns.DigitRegex);
            Assert.True(isMatch);
        }
    }

    public class LetterPatternTest
    {
        [Fact]
        public void AlefWithHamzaMaftouhaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("أ", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void AlefWithHamzaMaksouraMatchesPattern()
        {
            var isMatch = Regex.IsMatch("إ", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void AlefWithMaddaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("آ", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void AlefWithoutHamzaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ا", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void AlefMaqsouraMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ى", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void HamzaAalaKurseyYaaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ئ", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void HamzaAalaWawMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ؤ", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void BaaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ب", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void TaaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ت", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void TaaMarboutaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ة", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void ThaaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ث", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void JeemMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ج", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void H7aaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ح", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void KhaaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("خ", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void DalMatchesPattern()
        {
            var isMatch = Regex.IsMatch("د", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void ThalMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ذ", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void RaaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ر", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void ZaynMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ز", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void SeenMatchesPattern()
        {
            var isMatch = Regex.IsMatch("س", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void SheemMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ش", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void SadMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ص", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void DdadMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ض", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void TdaaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ط", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void ThhaaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ظ", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void AaynMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ع", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void GhaynMatchesPattern()
        {
            var isMatch = Regex.IsMatch("غ", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void QafMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ق", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void KafMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ك", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void LamMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ل", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void MeemMatchesPattern()
        {
            var isMatch = Regex.IsMatch("م", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void NoonMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ن", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void HaaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ه", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void WawMatchesPattern()
        {
            var isMatch = Regex.IsMatch("و", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void YaaMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ي", Patterns.LetterRegex);
            Assert.True(isMatch);
        }
    }

    public class NumberPatternTest
    {
        [Fact]
        public void ZeroMatchesPattern()
        {
            var isMatch = Regex.IsMatch("0", Patterns.NumberRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void PositiveDigitMatchesPattern()
        {
            var isMatch = Regex.IsMatch("4", Patterns.NumberRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void TwoDigitsMatchesPattern()
        {
            var isMatch = Regex.IsMatch("29", Patterns.NumberRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void OneDigitStartingWithZeroDoesNotMatchePattern()
        {
            var isMatch = Regex.IsMatch("07", Patterns.NumberRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void NegativeDigitDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("-3", Patterns.NumberRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void PositiveDigitDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("+8", Patterns.NumberRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void NegativeZeroDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("-0", Patterns.NumberRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void PositiveZeroDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("+0", Patterns.NumberRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void DecimalWithoutZeroDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch(".1", Patterns.NumberRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void DecimalWithZeroMatchesPattern()
        {
            var isMatch = Regex.IsMatch("0.1", Patterns.NumberRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void DecimalWithDigitMatchesPattern()
        {
            var isMatch = Regex.IsMatch("4.9", Patterns.NumberRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void DecimalWithTwoDigitsMatchesPattern()
        {
            var isMatch = Regex.IsMatch("61.3", Patterns.NumberRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void DecimalWithTenthDigitIsZeroMatchesPattern()
        {
            var isMatch = Regex.IsMatch("4.0", Patterns.NumberRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void DecimalWithMoreThanOneZeroAndOnlyZerosAfterDecimalDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("11.00", Patterns.NumberRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void DecimalWithManyDigitsAfterDecimalMatchesPattern()
        {
            var isMatch = Regex.IsMatch("2.0004", Patterns.NumberRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void DecimalWithTwoDigitsAfterDecimalStartingWithZeroMatchesPattern()
        {
            var isMatch = Regex.IsMatch("2.01", Patterns.NumberRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void DecimalWithTwoDigitsAfterDecimalWithoutZerosMatchesPattern()
        {
            var isMatch = Regex.IsMatch("2.13", Patterns.NumberRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void MoreThanOneDecimalDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("2.1.1", Patterns.NumberRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void MoreThanOneDecimalEndingWithZeroDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("2.1.0", Patterns.NumberRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void MoreThanOneDecimalWithZeroInMiddleDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("2.0.1", Patterns.NumberRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void MoreThanOneDecimalWithZeroInMiddleAndEndDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("2.0.0", Patterns.NumberRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void OneDigitRepeatedMatchesPattern()
        {
            var isMatch = Regex.IsMatch("11111", Patterns.NumberRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void OneDigitRepeatedWithDecimalMatchesPattern()
        {
            var isMatch = Regex.IsMatch("11111.1111", Patterns.NumberRegex);
            Assert.True(isMatch);
        }
    }
    
    public class LiteralPatternTest
    {
        [Fact]
        public void DigitDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("0", Patterns.LiteralRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void IntegerDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("19", Patterns.LiteralRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void UnderscoreDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("_", Patterns.LiteralRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void UnderscoresDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("__", Patterns.LiteralRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void LetterMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ا", Patterns.LiteralRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void LettersMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ولد", Patterns.LiteralRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void LetterFollowedByDigitMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ب1", Patterns.LiteralRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void LetterFollowedByDigitsMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ب123", Patterns.LiteralRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void LetterFollowedByUnderscoreMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ب_", Patterns.LiteralRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void LetterFollowedByUnderscoresMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ب___", Patterns.LiteralRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void LettersFollowedByDigitMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ولد1", Patterns.LiteralRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void LettersFollowedByDigitsMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ولد123", Patterns.LiteralRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void LettersFollowedByUnderscoreMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ولد_", Patterns.LiteralRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void LettersFollowedByUnderscoresMatchesPattern()
        {
            var isMatch = Regex.IsMatch("ولد__", Patterns.LiteralRegex);
            Assert.True(isMatch);
        }
        
        [Fact]
        public void LettersProceededByDigitDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("1ولد", Patterns.LiteralRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void LettersProceededByDigitsDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("123ولد", Patterns.LiteralRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void LettersProceededByUnderscoreDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("_ولد", Patterns.LiteralRegex);
            Assert.False(isMatch);
        }
        
        [Fact]
        public void LettersProceededByUnderscoresDoesNotMatchPattern()
        {
            var isMatch = Regex.IsMatch("___ولد", Patterns.LiteralRegex);
            Assert.False(isMatch);
        }
        
    }

    public class WordTerminalPatternTest
    {
        [Fact]
        void SplitOneWordByPatternResultInOneWord()
        {
            var text = "ولد";
            Assert.Equal(1, GetNumberOfWords(text));
        }
        
        [Theory]
        [InlineData("ولد=ولد")]
        [InlineData("ولد+ولد")]
        [InlineData("ولد-ولد")]
        [InlineData("ولد ولد")]
        [InlineData("ولد*ولد")]
        [InlineData("ولد&ولد")]
        [InlineData("ولد^ولد")]
        [InlineData("ولد%ولد")]
        [InlineData("ولد$ولد")]
        [InlineData("ولد#ولد")]
        [InlineData("ولد@ولد")]
        [InlineData("ولد!ولد")]
        [InlineData("ولد|ولد")]
        [InlineData("ولد\\ولد")]
        [InlineData("ولد\'ولد")]
        [InlineData("ولد\"ولد")]
        [InlineData("ولد/ولد")]
        [InlineData("ولد؟ولد")]
        [InlineData("ولد؛ولد")]
        [InlineData("ولد،ولد")]
        [InlineData("ولد.ولد")]
        [InlineData("ولد÷ولد")]
        [InlineData("ولد×ولد")]
        [InlineData("ولد<ولد")]
        [InlineData("ولد>ولد")]
        [InlineData("ولد{ولد")]
        [InlineData("ولد}ولد")]
        [InlineData("ولد[ولد")]
        [InlineData("ولد]ولد")]
        [InlineData("ولد(ولد")]
        [InlineData("ولد)ولد")]
        [InlineData("ولد~ولد")]
        [InlineData("ولد‘ولد")]
        private void SplitTwoWordsByPatternResultInOneWord(String text)
        {
            Assert.Equal(2, GetNumberOfWords(text));
        }

        private static int GetNumberOfWords(string text)
        {
            return Regex.Split(text, Patterns.WordTerminalRegex).Length;
        }
    }

    public class NumberTerminalPatternTest
    {
        [Fact]
        private void SplitOneNumberByPattern()
        {
            Assert.Equal(1, GetNumberOfWords("124"));
        }
        
        [Theory]
        [InlineData("123+2")]
        [InlineData("123.2")]
        [InlineData("123/2")]
        [InlineData("123\\2")]
        [InlineData("123|2")]
        [InlineData("123-2")]
        [InlineData("123=2")]
        [InlineData("123,2")]
        [InlineData("123:2")]
        [InlineData("123(2")]
        [InlineData("123)2")]
        [InlineData("123*2")]
        [InlineData("123&2")]
        [InlineData("123^2")]
        [InlineData("123%2")]
        [InlineData("123$2")]
        [InlineData("123#2")]
        [InlineData("123@2")]
        [InlineData("123!2")]
        [InlineData("123'2")]
        [InlineData("123\"2")]
        private void SplitTwoNumbersByPattern(string text)
        {
            Assert.Equal(2, GetNumberOfWords(text));
        }
        
        private static int GetNumberOfWords(string text)
        {
            return Regex.Split(text, Patterns.WordTerminalRegex).Length;
        }
    }
}