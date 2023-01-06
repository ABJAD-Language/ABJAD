using ABJAD.InterpretEngine.Declarations;
using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Statements;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test;

public class BindingInterpreterTest
{
    private readonly IStatementInterpreter statementInterpreter = Substitute.For<IStatementInterpreter>();
    private readonly IDeclarationInterpreter declarationInterpreter = Substitute.For<IDeclarationInterpreter>();
    private readonly BindingInterpreter interpreter;

    public BindingInterpreterTest()
    {
        interpreter = new BindingInterpreter(statementInterpreter, declarationInterpreter);
    }

    [Fact(DisplayName = "interpreting a statement calls the statement interpreter")]
    public void interpreting_a_statement_calls_the_statement_interpreter()
    {
        var statement = Substitute.For<Statement>();
        interpreter.Interpret(new List<Binding> { statement });
        
        statementInterpreter.Received(1).Interpret(statement);
    }

    [Fact(DisplayName = "interpreting a declaration calls the declaration interpreter")]
    public void interpreting_a_declaration_calls_the_declaration_interpreter()
    {
        var declaration = Substitute.For<Declaration>();
        interpreter.Interpret(new List<Binding> { declaration });
        
        declarationInterpreter.Received(1).Interpret(declaration);
    }
}