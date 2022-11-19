﻿using ABJAD.ParseEngine.Service.Expressions;
using ABJAD.ParseEngine.Service.Statements;
using Moq;

namespace ABJAD.ParseEngine.Service.Test.Statements;

public class StatementApiModelTest
{
    [Fact(DisplayName = "assignment statement returns correct type")]
    public void assignment_statement_returns_correct_type()
    {
        Assert.Equal("statement.assignment", new AssignmentStatementApiModel("", new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "expression statement returns correct type")]
    public void expression_statement_returns_correct_type()
    {
        Assert.Equal("statement.expression", new ExpressionStatementApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "if statement returns correct type")]
    public void if_statement_returns_correct_type()
    {
        Assert.Equal("statement.if", new IfStatementApiModel(new Mock<ExpressionApiModel>().Object, new Mock<StatementApiModel>().Object).Type);
    }

    [Fact(DisplayName = "if else statement returns correct type")]
    public void if_else_statement_returns_correct_type()
    {
        var ifStatementApiModel = new IfStatementApiModel(new Mock<ExpressionApiModel>().Object, new Mock<StatementApiModel>().Object);
        Assert.Equal("statement.ifElse", new IfElseStatementApiModel(ifStatementApiModel, new List<IfStatementApiModel>(), new Mock<StatementApiModel>().Object).Type);
    }

    [Fact(DisplayName = "print statement returns correct type")]
    public void print_statement_returns_correct_type()
    {
        Assert.Equal("statement.print", new PrintStatementApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "return statement returns correct type")]
    public void return_statement_returns_correct_type()
    {
        Assert.Equal("statement.return", new ReturnStatementApiModel(new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "while statement returns correct type")]
    public void while_statement_returns_correct_type()
    {
        Assert.Equal("statement.while", new WhileStatementApiModel(new Mock<ExpressionApiModel>().Object, new Mock<StatementApiModel>().Object).Type);
    }
}