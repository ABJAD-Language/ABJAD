using ABJAD.ParseEngine.Expressions;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Expressions;

public class ExpressionParserFactoryTest
{
    [Fact]
    private void ReturnsNonNullInstance()
    {
        Assert.NotNull(new ExpressionParserFactory().CreateInstance(new Mock<ITokenConsumer>().Object));
    }
}