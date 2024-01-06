using ABJAD.Interpreter.Domain.Shared;
using ABJAD.Interpreter.Domain.Shared.Expressions.Binary;
using ABJAD.Interpreter.Domain.Shared.Expressions.Fixes;
using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;
using ABJAD.Interpreter.Domain.Shared.Statements;
using FluentAssertions;
using static ABJAD.Interpreter.Mappers.StatementMapper;

namespace ABJAD.Test.Mappers;

public class StatementMapperTest
{
    [Fact(DisplayName = "maps assignment statement")]
    public void maps_assignment_statement()
    {
        var jsonObject = new
        {
            _type = "statement.assignment",
            target = "id",
            value = new
            {
                _type = "expression.primitive.string",
                value = "hello world"
            }
        };

        var statement = Map(jsonObject);

        var expectedStatement = new Assignment()
        {
            Target = "id",
            Value = new StringPrimitive() { Value = "hello world" }
        };

        Assert.IsType<Assignment>(statement);
        statement.Should().BeEquivalentTo(expectedStatement, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps block statement")]
    public void maps_block_statement()
    {
        var jsonObject = new
        {
            _type = "statement.block",
            bindings = new List<object>
            {
                new
                {
                    _type = "statement.assignment",
                    target = "id",
                    value = new
                    {
                        _type = "expression.primitive.string",
                        value = "hello world"
                    }
                }
            }
        };

        var statement = Map(jsonObject);

        var block = new Block()
        {
            Bindings = new List<Binding>
            {
                new Assignment() { Target = "id", Value = new StringPrimitive() { Value = "hello world" }}
            }
        };

        Assert.IsType<Block>(statement);
        statement.Should().BeEquivalentTo(block, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "map expression statement")]
    public void map_expression_statement()
    {
        var jsonObject = new
        {
            _type = "statement.expression",
            expression = new
            {
                _type = "expression.primitive.number",
                value = 54.3
            }
        };

        var statement = Map(jsonObject);

        var expressionStatement = new ExpressionStatement()
        {
            Target = new NumberPrimitive() { Value = 54.3 }
        };

        Assert.IsType<ExpressionStatement>(statement);
        statement.Should().BeEquivalentTo(expressionStatement, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps for statement")]
    public void maps_for_statement()
    {
        var jsonObject = new
        {
            _type = "statement.for",
            target = new
            {
                _type = "statement.assignment",
                target = "counter",
                value = new
                {
                    _type = "expression.primitive.number",
                    value = 0.0
                }
            },
            condition = new
            {
                _type = "statement.expression",
                expression = new
                {
                    _type = "expression.binary.lessCheck",
                    firstOperand = new
                    {
                        _type = "expression.primitive.identifier",
                        value = "counter"
                    },
                    secondOperand = new
                    {
                        _type = "expression.primitive.number",
                        value = 10.0
                    }
                }
            },
            targetCallback = new
            {
                _type = "expression.unary.postfix.addition",
                target = new
                {
                    _type = "expression.primitive.identifier",
                    value = "counter"
                }
            },
            body = new
            {
                _type = "statement.assignment",
                target = "i",
                value = new
                {
                    _type = "expression.primitive.identifier",
                    value = "counter"
                }
            }
        };

        var statement = Map(jsonObject);

        var forLoop = new ForLoop()
        {
            TargetDefinition = new Assignment() { Target = "counter", Value = new NumberPrimitive() { Value = 0.0 } },
            Condition = new ExpressionStatement()
            {
                Target = new LessCheck()
                {
                    FirstOperand = new IdentifierPrimitive() { Value = "counter" },
                    SecondOperand = new NumberPrimitive() { Value = 10.0 }
                }
            },
            Callback = new AdditionPostfix() { Target = "counter" },
            Body = new Assignment() { Target = "i", Value = new IdentifierPrimitive() { Value = "counter" } }
        };

        Assert.IsType<ForLoop>(statement);
        statement.Should().BeEquivalentTo(forLoop, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps if else statement")]
    public void maps_if_else_statement()
    {
        var jsonObject = new
        {
            _type = "statement.ifElse",
            mainIfStatement = new
            {
                _type = "statement.if",
                condition = new
                {
                    _type = "expression.primitive.bool",
                    value = true
                },
                body = new
                {
                    _type = "statement.assignment",
                    target = "i",
                    value = new
                    {
                        _type = "expression.primitive.number",
                        value = 1.0
                    }
                }
            },
            otherIfStatements = new List<object>
            {
                new
                {
                    _type = "statement.if",
                    condition = new
                    {
                        _type = "expression.primitive.bool",
                        value = false
                    },
                    body = new
                    {
                        _type = "statement.assignment",
                        target = "i",
                        value = new
                        {
                            _type = "expression.primitive.number",
                            value = 2.0
                        }
                    }
                }
            },
            elseBody = new
            {
                _type = "statement.assignment",
                target = "i",
                value = new
                {
                    _type = "expression.primitive.number",
                    value = 3.0
                }
            }
        };

        var statement = Map(jsonObject);

        var ifElse = new IfElse()
        {
            MainConditional = new Conditional()
            {
                Condition = new BoolPrimitive() { Value = true },
                Body = new Assignment() { Target = "i", Value = new NumberPrimitive() { Value = 1.0 } }
            },
            OtherConditionals = new List<Conditional>()
            {
                new()
                {
                    Condition = new BoolPrimitive() { Value = false },
                    Body = new Assignment() { Target = "i", Value = new NumberPrimitive() { Value = 2.0 }}
                }
            },
            ElseBody = new Assignment() { Target = "i", Value = new NumberPrimitive() { Value = 3.0 } }
        };

        Assert.IsType<IfElse>(statement);
        statement.Should().BeEquivalentTo(ifElse, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps print statement")]
    public void maps_print_statement()
    {
        var jsonObject = new
        {
            _type = "statement.print",
            target = new
            {
                _type = "expression.primitive.string",
                value = "hello world"
            }
        };

        var statement = Map(jsonObject);

        var expectedStatement = new Print() { Target = new StringPrimitive() { Value = "hello world" } };

        Assert.IsType<Print>(statement);
        statement.Should().BeEquivalentTo(expectedStatement, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps return statement")]
    public void maps_return_statement()
    {
        var jsonObject = new
        {
            _type = "statement.return",
            target = new
            {
                _type = "expression.primitive.bool",
                value = true
            }
        };

        var statement = Map(jsonObject);

        var expectedStatement = new Return() { Target = new BoolPrimitive() { Value = true } };

        Assert.IsType<Return>(statement);
        statement.Should().BeEquivalentTo(expectedStatement, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps while statement")]
    public void maps_while_statement()
    {
        var jsonObject = new
        {
            _type = "statement.while",
            condition = new
            {
                _type = "expression.primitive.bool",
                value = true
            },
            body = new
            {
                _type = "statement.print",
                target = new
                {
                    _type = "expression.primitive.string",
                    value = "hello world"
                }
            }
        };

        var statement = Map(jsonObject);

        var expectedStatement = new WhileLoop()
        {
            Condition = new BoolPrimitive() { Value = true },
            Body = new Print() { Target = new StringPrimitive() { Value = "hello world" } }
        };

        Assert.IsType<WhileLoop>(statement);
        statement.Should().BeEquivalentTo(expectedStatement, options => options.RespectingRuntimeTypes());
    }
}