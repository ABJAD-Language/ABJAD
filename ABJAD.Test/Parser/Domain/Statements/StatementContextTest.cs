using ABJAD.Parser.Domain.Statements;
using Moq;

namespace ABJAD.Test.Parser.Domain.Statements;

public class StatementContextTest
{
    private readonly Mock<ParseStatementStrategy> strategy = new();

    [Fact]
    private void ThrowsExceptionIfStrategyIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new StatementContext(null));
    }

    [Fact]
    private void ExecutesStrategy()
    {
        new StatementContext(strategy.Object).ParseStatement();
        strategy.Verify(s => s.Parse());
    }

    [Fact]
    private void ReturnsTheResultReturnedFromExecutingStrategy()
    {
        var statementMock = new Mock<Statement>();
        strategy.Setup(s => s.Parse()).Returns(statementMock.Object);
        var statement = new StatementContext(strategy.Object).ParseStatement();
        Assert.Equal(statementMock.Object, statement);
    }
}