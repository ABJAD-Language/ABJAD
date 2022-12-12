using ABJAD.InterpretEngine;
using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Shared.Statements;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test;

public class MainInterpreterTest
{
    private readonly Interpreter<Statement> statementInterpreter = Substitute.For<Interpreter<Statement>>();
    private readonly MainInterpreter interpreter;

    public MainInterpreterTest()
    {
        interpreter = new MainInterpreter(statementInterpreter);
    }

    [Fact(DisplayName = "interpreting a statement calls the statement interpreter")]
    public void interpreting_a_statement_calls_the_statement_interpreter()
    {
        var statement = Substitute.For<Statement>();
        interpreter.Interpret(new List<Binding> { statement });
        
        statementInterpreter.Received(1).Interpret(statement);
    }
}