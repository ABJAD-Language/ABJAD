using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions;

public class ExpressionEvaluatorTest
{
    private readonly IExpressionStrategyFactory expressionStrategyFactory = Substitute.For<IExpressionStrategyFactory>();
    private readonly ScopeFacade scopeFacade = Substitute.For<ScopeFacade>();

    [Fact(DisplayName = "applies assignment evaluation strategy on assignment expressions")]
    public void applies_assignment_evaluation_strategy_on_assignment_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade);
        var assignmentExpression = Substitute.For<AssignmentExpression>();
        var strategy = Substitute.For<ExpressionEvaluationStrategy>();
        expressionStrategyFactory.GetAssignmentEvaluationStrategy(assignmentExpression, expressionEvaluator, scopeFacade).Returns(strategy);
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);
        
        var result = expressionEvaluator.Evaluate(assignmentExpression);
        Assert.Equal(expectedResult, result);
    }

    [Fact(DisplayName = "applies binary expression evaluation strategy on binary expressions")]
    public void applies_binary_expression_evaluation_strategy_on_binary_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade);
        var binaryExpression = Substitute.For<BinaryExpression>();
        var strategy = Substitute.For<ExpressionEvaluationStrategy>();
        expressionStrategyFactory.GetBinaryExpressionEvaluationStrategy(binaryExpression, expressionEvaluator).Returns(strategy);
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);

        var result = expressionEvaluator.Evaluate(binaryExpression);
        Assert.Equal(expectedResult, result);
    }

    [Fact(DisplayName = "applies fixes expressions evaluation strategy on prefix and postfix expressions")]
    public void applies_fixes_expressions_evaluation_strategy_on_prefix_and_postfix_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade);
        var fixExpression = Substitute.For<FixExpression>();
        var strategy = Substitute.For<ExpressionEvaluationStrategy>();
        expressionStrategyFactory.GetFixesEvaluationStrategy(fixExpression, scopeFacade).Returns(strategy);
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);

        var result = expressionEvaluator.Evaluate(fixExpression);
        Assert.Equal(expectedResult, result);
    }

    [Fact(DisplayName = "applies primitives evaluation strategy on primitive expressions")]
    public void applies_primitives_evaluation_strategy_on_primitive_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade);
        var primitive = Substitute.For<Primitive>();
        var strategy = Substitute.For<ExpressionEvaluationStrategy>();
        expressionStrategyFactory.GetPrimitiveEvaluationStrategy(primitive, scopeFacade).Returns(strategy);
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);

        var result = expressionEvaluator.Evaluate(primitive);
        Assert.Equal(expectedResult, result);
    }

    [Fact(DisplayName = "applies unary expression evaluation strategy on unary expressions")]
    public void applies_unary_expression_evaluation_strategy_on_unary_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade);
        var unaryExpression = Substitute.For<UnaryExpression>();
        var strategy = Substitute.For<ExpressionEvaluationStrategy>();
        expressionStrategyFactory.GetUnaryExpressionEvaluationStrategy(unaryExpression, expressionEvaluator).Returns(strategy);
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);

        var result = expressionEvaluator.Evaluate(unaryExpression);
        Assert.Equal(expectedResult, result);
    }
}