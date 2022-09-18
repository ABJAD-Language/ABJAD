using ABJAD.LexEngine;
using Xunit;

namespace ABJAD.LexEngine.Test;

public class StringUtilsTest
{
    private readonly StringUtils stringUtils;

    public StringUtilsTest()
    {
        stringUtils = new StringUtils();
    }

    [Fact]
    private void IgnoresLetterMadda()
    {
        Assert.Equal("ولد", stringUtils.IgnoreCaseSensitivity("ولـــد"));
    }

    [Fact]
    private void IgnoresAlefWithHamza()
    {
        Assert.Equal("اكل", stringUtils.IgnoreCaseSensitivity("أكل"));
    }

    [Fact]
    private void IgnoresAlefWithHamzaMaksoura()
    {
        Assert.Equal("اكل", stringUtils.IgnoreCaseSensitivity("إكل"));
    }

    [Fact]
    private void IgnoresAlefWithMadda()
    {
        Assert.Equal("اكل", stringUtils.IgnoreCaseSensitivity("آكل"));
    }
}