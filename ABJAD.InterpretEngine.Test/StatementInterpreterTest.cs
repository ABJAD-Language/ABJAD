﻿using ABJAD.InterpretEngine;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Statements;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test;

public class StatementInterpreterTest
{
    private readonly Interpreter<Expression> expressionInterpreter = Substitute.For<Interpreter<Expression>>();
    private readonly StatementInterpreter statementInterpreter;

    public StatementInterpreterTest()
    {
        statementInterpreter = new StatementInterpreter(expressionInterpreter);
    }

    [Fact(DisplayName = "delegates to the expression interpreter when the statement is an expression statement")]
    public void delegates_to_the_expression_interpreter_when_the_statement_is_an_expression_statement()
    {
        var expression = Substitute.For<Expression>();
        var statement = new ExpressionStatement { Target = expression };
        statementInterpreter.Interpret(statement);
        expressionInterpreter.Received(1).Interpret(expression);
    }
}