using ABJAD.LexEngine.Tokens;
using Xunit;

namespace ABJAD.LexEngine.Test.Tokens;

public class WordScannerTest
{
    public class ScanNextWordTest
    {
        [Fact]
        private void ReturnsCompleteWordWhenOneWordInSentenceExists()
        {
            Assert.Equal("متغير", WordScanner.ScanNextWord("متغير", 0, 1));
        }
        [Fact]
        private void ReturnsCompleteWordWhenOneWordInSentenceExistsFollowedByNewLine()
        {
            Assert.Equal("متغير", WordScanner.ScanNextWord("متغير\n", 0, 1));
        }

        [Fact]
        private void ReturnsCompleteWordWhenOneWordInSentenceExistsFollowedByTerminator()
        {
            Assert.Equal("متغير", WordScanner.ScanNextWord("متغير؛", 0, 1));
        }

        [Fact]
        private void ReturnsCompleteWordWhenTwoWordInSentenceExistAndIndexPointsToFirst()
        {
            Assert.Equal("متغير", WordScanner.ScanNextWord("متغير ر", 0, 1));
        }

        [Fact]
        private void ReturnsCompleteWordWhenTwoWordInSentenceExistAndIndexPointsToSecond()
        {
            Assert.Equal("ر", WordScanner.ScanNextWord("متغير ر", 6, 1));
        }

        [Fact]
        private void ReturnsCompleteWordWhenThreeWordInSentenceExistAndIndexPointsToSecond()
        {
            Assert.Equal("ر", WordScanner.ScanNextWord("متغير ر =", 6, 1));
        }

        [Fact]
        private void ThrowsExceptionWhenWordStartsWithNumber()
        {
            var exception = Assert.Throws<InvalidWordException>(() => WordScanner.ScanNextWord("متغير 1ولد؛", 6, 1));
            Assert.Equal(7, exception.Index);
            Assert.Equal(1, exception.Line);
            Assert.Equal("1ولد", exception.Label);
            Assert.Equal("Invalid word: '1ولد' at line 1 : 7", exception.EnglishMessage);
            Assert.Equal("كلمة غير صالحة: '1ولد' على السطر 1 : 7", exception.ArabicMessage);
        }
    }
    
    public class ScanNextNumberTest
    {
        [Fact]
        private void ReturnsCompleteNumberWhenOneNumberInSentenceExists()
        {
            Assert.Equal("123", WordScanner.ScanNextNumber("123", 0, 1));
        }
        
        [Fact]
        private void ReturnsCompleteNumberWhenOneNumberWithDecimalInSentenceExists()
        {
            Assert.Equal("123.3", WordScanner.ScanNextNumber("123.3", 0, 1));
        }
        
        [Fact]
        private void ReturnsCompleteNumberWhenOneNumberWithDecimalWithCodeBeforeAndAfterInSentenceExists()
        {
            Assert.Equal("123.3", WordScanner.ScanNextNumber("متغير ا = 123.3؛", 10, 1));
        }
        
        [Fact]
        private void ThrowsExceptionWhenNumberHasMoreThanOneDecimal()
        {
            var exception = Assert.Throws<InvalidWordException>(() => WordScanner.ScanNextNumber("متغير ا = 123.3.1؛", 10, 1));
            Assert.Equal(11, exception.Index);
            Assert.Equal(1, exception.Line);
            Assert.Equal("123.3.1", exception.Label);
            Assert.Equal("Invalid word: '123.3.1' at line 1 : 11", exception.EnglishMessage);
            Assert.Equal("كلمة غير صالحة: '123.3.1' على السطر 1 : 11", exception.ArabicMessage);
        }

        [Fact]
        private void ThrowsExceptionWhenNumberIsEmpty()
        {
            var exception = Assert.Throws<InvalidWordException>(() => WordScanner.ScanNextNumber("متغير ا = ؛", 10, 1));
            Assert.Equal(11, exception.Index);
            Assert.Equal(1, exception.Line);
            Assert.Equal("", exception.Label);
            Assert.Equal("Invalid word: '' at line 1 : 11", exception.EnglishMessage);
            Assert.Equal("كلمة غير صالحة: '' على السطر 1 : 11", exception.ArabicMessage);
        }
    }
}