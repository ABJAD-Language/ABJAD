using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements;
using ABJAD.InterpretEngine.Statements.Strategies;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Statements.Strategies;

public class BlockInterpretationStrategyTest
{
    private readonly IStatementInterpreter statementInterpreter = Substitute.For<IStatementInterpreter>();
    private readonly IDeclarationInterpreter declarationInterpreter = Substitute.For<IDeclarationInterpreter>();

    [Fact(DisplayName = "calls the declaration interpreter on the declaration bindings")]
    public void calls_the_declaration_interpreter_on_the_declaration_bindings()
    {
        var declaration = Substitute.For<Declaration>();
        var statement = Substitute.For<Statement>();
        var block = new Block { Bindings = new List<Binding> { declaration, statement }};
        var strategy = new BlockInterpretationStrategy(block, false, statementInterpreter, declarationInterpreter);
        statementInterpreter.Interpret(statement).Returns(StatementInterpretationResult.GetNotReturned());
        strategy.Apply();

        declarationInterpreter.Received(1).Interpret(declaration);
    }

    [Fact(DisplayName = "calls the statement interpreter on the statement bindings")]
    public void calls_the_statement_interpreter_on_the_statement_bindings()
    {
        var declaration = Substitute.For<Declaration>();
        var statement = Substitute.For<Statement>();
        var block = new Block { Bindings = new List<Binding> { declaration, statement }};
        var strategy = new BlockInterpretationStrategy(block, false, statementInterpreter, declarationInterpreter);
        statementInterpreter.Interpret(statement).Returns(StatementInterpretationResult.GetNotReturned());
        strategy.Apply();
        
        statementInterpreter.Received(1).Interpret(statement);
    }

    [Fact(DisplayName = "interprets statements with function context set to true when it is the case")]
    public void interprets_statements_with_function_context_set_to_true_when_it_is_the_case()
    {
        var declaration = Substitute.For<Declaration>();
        var statement = Substitute.For<Statement>();
        var block = new Block { Bindings = new List<Binding> { declaration, statement }};
        var strategy = new BlockInterpretationStrategy(block, true, statementInterpreter, declarationInterpreter);
        statementInterpreter.Interpret(statement, true).Returns(StatementInterpretationResult.GetNotReturned());
        strategy.Apply();
        
        statementInterpreter.Received(1).Interpret(statement, true);
    }

    [Fact(DisplayName = "stops execution and return the result when a statement returns a breaking result")]
    public void stops_execution_and_return_the_result_when_a_statement_returns_a_breaking_result()
    {
        var declaration1 = Substitute.For<Declaration>();
        var declaration2 = Substitute.For<Declaration>();
        var statement1 = Substitute.For<Statement>();
        var statement2 = Substitute.For<Statement>();
        var block = new Block { Bindings = new List<Binding> { declaration1, statement1, statement2, declaration2 }};
        var strategy = new BlockInterpretationStrategy(block, true, statementInterpreter, declarationInterpreter);

        var evaluatedResult = new EvaluatedResult();
        statementInterpreter.Interpret(statement1, true).Returns(StatementInterpretationResult.GetReturned(evaluatedResult));

        var result = strategy.Apply();

        statementInterpreter.DidNotReceive().Interpret(statement2);
        declarationInterpreter.DidNotReceive().Interpret(declaration2);
        
        Assert.True(result.Returned);
        Assert.True(result.IsValueReturned);
        Assert.Equal(evaluatedResult, result.ReturnedValue);
    }

    [Fact(DisplayName = "returns a non breaking result when none was returned")]
    public void returns_a_non_breaking_result_when_none_was_returned()
    {
        var declaration1 = Substitute.For<Declaration>();
        var declaration2 = Substitute.For<Declaration>();
        var statement1 = Substitute.For<Statement>();
        var statement2 = Substitute.For<Statement>();
        var block = new Block { Bindings = new List<Binding> { declaration1, statement1, statement2, declaration2 }};
        var strategy = new BlockInterpretationStrategy(block, true, statementInterpreter, declarationInterpreter);

        statementInterpreter.Interpret(Arg.Any<Statement>(), true).Returns(StatementInterpretationResult.GetNotReturned());

        var result = strategy.Apply();

        Received.InOrder(() =>
        {
            declarationInterpreter.Interpret(declaration1);
            statementInterpreter.Interpret(statement1, true);
            statementInterpreter.Interpret(statement2, true);
            declarationInterpreter.Interpret(declaration2);
        });
        
        Assert.False(result.Returned);
    }
}