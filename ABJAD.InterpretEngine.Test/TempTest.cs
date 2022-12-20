using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Statements;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test;

public class TempTest
{
    [Fact(DisplayName = "temp")]
    public void temp()
    {
        var scope = Substitute.For<IScope>();
        var expressionStrategyFactory = new ExpressionStrategyFactory(scope);
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory);
        var statementInterpreter = new StatementInterpreter(expressionEvaluator);
        var mainInterpreter = new MainInterpreter(statementInterpreter);
    }
}