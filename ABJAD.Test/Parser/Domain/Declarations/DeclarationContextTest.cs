using ABJAD.Parser.Domain.Declarations;
using Moq;

namespace ABJAD.Test.Parser.Domain.Declarations;

public class DeclarationContextTest
{
    private readonly Mock<ParseDeclarationStrategy> strategy = new();

    [Fact]
    private void ThrowsExceptionIfStrategyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new DeclarationContext(null));
    }

    [Fact]
    private void ExecutesStrategy()
    {
        new DeclarationContext(strategy.Object).ParseDeclaration();
        strategy.Verify(s => s.Parse());
    }

    [Fact]
    private void ReturnsResultReturnedFromExecutingStrategy()
    {
        var declarationMock = new Mock<Declaration>();
        strategy.Setup(s => s.Parse()).Returns(declarationMock.Object);
        var declaration = new DeclarationContext(strategy.Object).ParseDeclaration();
        Assert.Equal(declarationMock.Object, declaration);
    }
}