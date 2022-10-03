using ABJAD.ParseEngine.Expressions;
using Moq;
using Xunit;

namespace ABJAD.ParseEngine.Test.Expressions;

public class ExpressionParserFactoryTest
{
    [Fact]
    private void ReturnsInstanceOfAbstractSyntaxTreeExpressionParser()
    {
        var tokenConsumer = new Mock<ITokenConsumer>();
        Assert.IsType<AbstractSyntaxTreeExpressionParser>(ExpressionParserFactory.Get(tokenConsumer.Object));
    }
}