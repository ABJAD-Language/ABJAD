using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions;

public class ExpressionEvaluatorTest
{
    private readonly IExpressionStrategyFactory expressionStrategyFactory = Substitute.For<IExpressionStrategyFactory>();
    private readonly ScopeFacade scopeFacade = Substitute.For<ScopeFacade>();
    private readonly TextWriter writer = Substitute.For<TextWriter>();

    [Fact(DisplayName = "applies assignment evaluation strategy on assignment expressions")]
    public void applies_assignment_evaluation_strategy_on_assignment_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade, writer);
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
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade, writer);
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
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade, writer);
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
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade, writer);
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
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade, writer);
        var unaryExpression = Substitute.For<UnaryExpression>();
        var strategy = Substitute.For<ExpressionEvaluationStrategy>();
        expressionStrategyFactory.GetUnaryExpressionEvaluationStrategy(unaryExpression, expressionEvaluator).Returns(strategy);
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);

        var result = expressionEvaluator.Evaluate(unaryExpression);
        Assert.Equal(expectedResult, result);
    }

    [Fact(DisplayName = "applies instantiation expression evaluation strategy on instantiation expressions")]
    public void applies_instantiation_expression_evaluation_strategy_on_instantiation_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade, writer);
        var instantiation = new Instantiation();
        var strategy = Substitute.For<ExpressionEvaluationStrategy>();
        expressionStrategyFactory.GetInstantiationEvaluationStrategy(instantiation, Arg.Any<ScopeFacade>(), 
            Arg.Any<ScopeFacade>(), expressionEvaluator, Arg.Any<Interpreter<Statement>>(), Arg.Any<Interpreter<Declaration>>()).Returns(strategy);
        
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);
        
        var result = expressionEvaluator.Evaluate(instantiation);
        Assert.Equal(expectedResult, result);
    }

    [Fact(DisplayName = "applies instance field access evaluation strategy on instance field access expressions")]
    public void applies_instance_field_access_evaluation_strategy_on_instance_field_access_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade, writer);
        var instanceFieldAccess = new InstanceFieldAccess();
        var strategy = Substitute.For<ExpressionEvaluationStrategy>();
        expressionStrategyFactory.GetInstanceFieldAccessEvaluationStrategy(instanceFieldAccess, Arg.Any<ScopeFacade>()).Returns(strategy);
        
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);
        
        var result = expressionEvaluator.Evaluate(instanceFieldAccess);
        Assert.Equal(expectedResult, result);
    }

    [Fact(DisplayName = "applies instance method call evaluation strategy on instance method call expressions")]
    public void applies_instance_method_call_evaluation_strategy_on_instance_method_call_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade, writer);
        var instanceMethodCall = new InstanceMethodCall();
        var strategy = Substitute.For<ExpressionEvaluationStrategy>();
        expressionStrategyFactory.GetInstanceFieldMethodCallEvaluationStrategy(instanceMethodCall,
            Arg.Any<ScopeFacade>(), Arg.Any<Evaluator<Expression>>(), Arg.Any<TextWriter>()).Returns(strategy);
        
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);
        
        var result = expressionEvaluator.Evaluate(instanceMethodCall);
        Assert.Equal(expectedResult, result);
    }

    [Fact(DisplayName = "applies method call evaluation strategy on method call expressions")]
    public void applies_method_call_evaluation_strategy_on_method_call_expressions()
    {
        var expressionEvaluator = new ExpressionEvaluator(expressionStrategyFactory, scopeFacade, writer);
        var methodCall = new MethodCall();
        var strategy = Substitute.For<ExpressionEvaluationStrategy>();
        expressionStrategyFactory.GetMethodCallEvaluationStrategy(methodCall,
            Arg.Any<ScopeFacade>(), Arg.Any<Evaluator<Expression>>(), Arg.Any<Interpreter<Statement>>(),
            Arg.Any<Interpreter<Declaration>>()).Returns(strategy);
        
        var expectedResult = new EvaluatedResult { Type = Substitute.For<DataType>(), Value = new object() };
        strategy.Apply().Returns(expectedResult);
        
        var result = expressionEvaluator.Evaluate(methodCall);
        Assert.Equal(expectedResult, result);
    }
}