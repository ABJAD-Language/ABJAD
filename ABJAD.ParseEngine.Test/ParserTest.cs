using System.Collections.Generic;
using ABJAD.ParseEngine.Bindings;
using ABJAD.ParseEngine.Declarations;
using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Expressions.Binary;
using ABJAD.ParseEngine.Expressions.Unary;
using ABJAD.ParseEngine.Primitives;
using ABJAD.ParseEngine.Shared;
using ABJAD.ParseEngine.Statements;
using FluentAssertions;
using Xunit;

namespace ABJAD.ParseEngine.Test;

public class ParserTest
{
    [Fact]
    private void ParsePrintHelloWorld()
    {
        var tokens = new List<Token>
        {
            new() { Type = TokenType.PRINT },
            new() { Type = TokenType.OPEN_PAREN },
            new() { Type = TokenType.STRING_CONST, Content = "hello, world" },
            new() { Type = TokenType.CLOSE_PAREN },
            new() { Type = TokenType.SEMICOLON }
        };

        var expectedBinding = new StatementBinding(
            new PrintStatement(
                new PrimitiveExpression(
                    StringPrimitive.From("hello, world")
                )
            )
        );

        var bindings = new Parser(tokens).Parse();
        Assert.Single(bindings);
        bindings[0].Should().BeEquivalentTo(expectedBinding, options => options.RespectingRuntimeTypes());
    }

    [Fact]
    private void ParseTwoStatementsPrintHelloWorld()
    {
        var tokens = new List<Token>
        {
            new() { Type = TokenType.PRINT },
            new() { Type = TokenType.OPEN_PAREN },
            new() { Type = TokenType.STRING_CONST, Content = "hello, world" },
            new() { Type = TokenType.CLOSE_PAREN },
            new() { Type = TokenType.SEMICOLON },
            new() { Type = TokenType.PRINT },
            new() { Type = TokenType.OPEN_PAREN },
            new() { Type = TokenType.STRING_CONST, Content = "hello, world" },
            new() { Type = TokenType.CLOSE_PAREN },
            new() { Type = TokenType.SEMICOLON },
        };

        var expectedBinding = new StatementBinding(
            new PrintStatement(
                new PrimitiveExpression(
                    StringPrimitive.From("hello, world")
                )
            )
        );

        var bindings = new Parser(tokens).Parse();
        Assert.Equal(2, bindings.Count);
        bindings[0].Should().BeEquivalentTo(expectedBinding, options => options.RespectingRuntimeTypes());
        bindings[1].Should().BeEquivalentTo(expectedBinding, options => options.RespectingRuntimeTypes());
    }

    [Fact]
    private void ParseClassDeclaration()
    {
        var tokens = new List<Token>
        {
            new() { Type = TokenType.CLASS },
            new() { Type = TokenType.WHITE_SPACE },
            new() { Type = TokenType.ID, Content = "className" },
            new() { Type = TokenType.WHITE_SPACE },
            new() { Type = TokenType.OPEN_BRACE },
            new() { Type = TokenType.VAR },
            new() { Type = TokenType.NUMBER },
            new() { Type = TokenType.ID, Content = "field" },
            new() { Type = TokenType.SEMICOLON },
            new() { Type = TokenType.CONSTRUCTOR },
            new() { Type = TokenType.OPEN_PAREN },
            new() { Type = TokenType.STRING },
            new() { Type = TokenType.ID, Content = "param" },
            new() { Type = TokenType.CLOSE_PAREN },
            new() { Type = TokenType.OPEN_BRACE },
            new() { Type = TokenType.ID, Content = "field" },
            new() { Type = TokenType.EQUAL },
            new() { Type = TokenType.ID, Content = "param" },
            new() { Type = TokenType.SEMICOLON },
            new() { Type = TokenType.CLOSE_BRACE },
            new() { Type = TokenType.FUNC },
            new() { Type = TokenType.ID, Content = "getField" },
            new() { Type = TokenType.OPEN_PAREN },
            new() { Type = TokenType.CLOSE_PAREN },
            new() { Type = TokenType.COLON },
            new() { Type = TokenType.STRING },
            new() { Type = TokenType.OPEN_BRACE },
            new() { Type = TokenType.RETURN },
            new() { Type = TokenType.ID, Content = "field" },
            new() { Type = TokenType.SEMICOLON },
            new() { Type = TokenType.CLOSE_BRACE },
            new() { Type = TokenType.CLOSE_BRACE },
        };

        var expectedBinding = new DeclarationBinding(
            new ClassDeclaration(
                "className",
                new BlockDeclaration(
                    new List<DeclarationBinding>
                    {
                        new(
                            new VariableDeclaration("رقم", "field", null)
                        ),
                        new(
                            new ConstructorDeclaration(
                                new List<FunctionParameter>
                                {
                                    new FunctionParameter("مقطع", "param")
                                },
                                new BlockStatement(
                                    new List<Binding>
                                    {
                                        new StatementBinding(
                                            new AssignmentStatement("field",
                                                new PrimitiveExpression(IdentifierPrimitive.From("param")))
                                        )
                                    }
                                )
                            )
                        ),
                        new(
                            new FunctionDeclaration(
                                "getField", "مقطع", new List<FunctionParameter>(),
                                new BlockStatement(
                                    new List<Binding>
                                    {
                                        new StatementBinding(
                                            new ReturnStatement(
                                                new PrimitiveExpression(IdentifierPrimitive.From("field"))))
                                    }
                                )
                            )
                        )
                    }
                )
            )
        );

        var bindings = new Parser(tokens).Parse();
        Assert.Single(bindings);
        bindings[0].Should().BeEquivalentTo(expectedBinding, options => options.RespectingRuntimeTypes());
    }

    [Fact]
    private void ParsingExpressionStatement()
    {
        var tokens = new List<Token>
        {
            new() { Type = TokenType.ID, Content = "a" },
            new() { Type = TokenType.PLUS_PLUS },
            new() { Type = TokenType.SEMICOLON },
        };

        var expectedBinding = new StatementBinding(
            new ExpressionStatement(
                new PostfixAdditionExpression(
                    new PrimitiveExpression(IdentifierPrimitive.From("a"))
                )
            )
        );

        var bindings = new Parser(tokens).Parse();
        Assert.Single(bindings);
        bindings[0].Should().BeEquivalentTo(expectedBinding, options => options.RespectingRuntimeTypes());
    }

    [Fact]
    private void ParsingBinaryExpressionStatement()
    {
        var tokens = new List<Token>
        {
            new() { Type = TokenType.ID, Content = "a" },
            new() { Type = TokenType.PLUS_PLUS },
            new() { Type = TokenType.PLUS },
            new() { Type = TokenType.ID, Content = "b" },
            new() { Type = TokenType.PLUS_PLUS },
            new() { Type = TokenType.SEMICOLON },
        };

        var expectedBinding = new StatementBinding(
            new ExpressionStatement(
                new AdditionExpression(
                    new PostfixAdditionExpression(
                        new PrimitiveExpression(IdentifierPrimitive.From("a"))
                    ),
                    new PostfixAdditionExpression(
                        new PrimitiveExpression(IdentifierPrimitive.From("b"))
                    )
                )
            )
        );

        var bindings = new Parser(tokens).Parse();
        Assert.Single(bindings);
        bindings[0].Should().BeEquivalentTo(expectedBinding, options => options.RespectingRuntimeTypes());
    }

    [Fact]
    private void FailsIfMissingSemicolonAfterExpression()
    {
        var tokens = new List<Token>
        {
            new() { Type = TokenType.ID, Content = "a" },
            new() { Type = TokenType.PLUS_PLUS },
        };

        Assert.NotNull(Record.Exception(() => new Parser(tokens).Parse()));
    }

    [Fact(DisplayName = "skips comments when found")]
    public void skips_comments_when_found()
    {
        var tokens = new List<Token>
        {
            new() { Type = TokenType.IF },
            new() { Type = TokenType.OPEN_PAREN },
            new() { Type = TokenType.TRUE },
            new() { Type = TokenType.CLOSE_PAREN },
            new() { Type = TokenType.WHITE_SPACE },
            new() { Type = TokenType.OPEN_BRACE },
            new() { Type = TokenType.WHITE_SPACE },
            new() { Type = TokenType.COMMENT },
            new() { Type = TokenType.WHITE_SPACE },
            new() { Type = TokenType.PRINT },
            new() { Type = TokenType.OPEN_PAREN },
            new() { Type = TokenType.STRING_CONST, Content = "hello"},
            new() { Type = TokenType.CLOSE_PAREN },
            new() { Type = TokenType.SEMICOLON },
            new() { Type = TokenType.WHITE_SPACE },
            new() { Type = TokenType.CLOSE_BRACE }
        };

        var expectedBinding = new StatementBinding(new IfStatement(
            new PrimitiveExpression(BoolPrimitive.True()),
            new BlockStatement(new List<Binding>
            {
                new StatementBinding(new PrintStatement(new PrimitiveExpression(StringPrimitive.From("hello"))))
            })
        ));
        
        var bindings = new Parser(tokens).Parse();
        Assert.Single(bindings);
        bindings[0].Should().BeEquivalentTo(expectedBinding, options => options.RespectingRuntimeTypes());
    }
}