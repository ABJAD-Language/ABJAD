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
        strategy.Apply();
        
        statementInterpreter.Received(1).Interpret(statement, true);
    }
}