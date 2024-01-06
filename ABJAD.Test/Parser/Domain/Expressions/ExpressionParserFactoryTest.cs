using ABJAD.Parser.Domain;
using ABJAD.Parser.Domain.Expressions;
using Moq;

namespace ABJAD.Test.Parser.Domain.Expressions;

public class ExpressionParserFactoryTest
{
    [Fact]
    private void ReturnsInstanceOfAbstractSyntaxTreeExpressionParser()
    {
        var tokenConsumer = new Mock<ITokenConsumer>();
        Assert.IsType<AbstractSyntaxTreeExpressionParser>(ExpressionParserFactory.Get(tokenConsumer.Object));
    }
}