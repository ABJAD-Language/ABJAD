using ABJAD.Interpreter.Domain.Shared;
using ABJAD.Interpreter.Domain.Shared.Declarations;
using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Types;
using FluentAssertions;
using static ABJAD.Interpreter.Mappers.DeclarationMapper;

namespace ABJAD.Test.Interpreter.Mappers;

public class DeclarationMapperTest
{
    [Fact(DisplayName = "maps variable declarations")]
    public void maps_variable_declarations()
    {
        var jsonObject = new
        {
            _type = "declaration.variable",
            type = "رقم",
            name = "id",
            value = new
            {
                _type = "expression.primitive.number",
                value = 13.0
            }
        };

        var declaration = Map(jsonObject);

        var variableDeclaration = new VariableDeclaration()
        {
            Type = DataType.Number(),
            Name = "id",
            Value = new NumberPrimitive() { Value = 13.0 }
        };

        Assert.IsType<VariableDeclaration>(declaration);
        declaration.Should().BeEquivalentTo(variableDeclaration, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps constant declaration")]
    public void maps_constant_declaration()
    {
        var jsonObject = new
        {
            _type = "declaration.constant",
            name = "id",
            type = "رقم",
            value = new
            {
                _type = "expression.primitive.number",
                value = 10.0
            }
        };

        var declaration = Map(jsonObject);

        var expectedDeclaration = new ConstantDeclaration()
        {
            Name = "id",
            Type = DataType.Number(),
            Value = new NumberPrimitive() { Value = 10.0 }
        };

        Assert.IsType<ConstantDeclaration>(declaration);
        declaration.Should().BeEquivalentTo(expectedDeclaration, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps function declaration")]
    public void maps_function_declaration()
    {
        var jsonObject = new
        {
            _type = "declaration.function",
            name = "func",
            returnType = "مقطع",
            parameters = new List<object>
            {
                new
                {
                    name = "param",
                    type = "رقم"
                }
            },
            body = new
            {
                _type = "statement.block",
                bindings = new List<object>
                {
                    new
                    {
                        _type = "statement.return",
                        target = new
                        {
                            _type = "expression.primitive.string",
                            value = "hello world"
                        }
                    }
                }
            }
        };

        var declaration = Map(jsonObject);

        var expectedDeclaration = new FunctionDeclaration()
        {
            Name = "func",
            ReturnType = DataType.String(),
            Parameters = new List<Parameter>
            {
                new() { Name = "param", Type = DataType.Number() }
            },
            Body = new Block()
            {
                Bindings = new List<Binding>
                {
                    new Return() { Target = new StringPrimitive() { Value = "hello world" }}
                }
            }
        };

        Assert.IsType<FunctionDeclaration>(declaration);
        declaration.Should().BeEquivalentTo(expectedDeclaration, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps function declaration correctly when it does not have a return type")]
    public void maps_function_declaration_correctly_when_it_does_not_have_a_return_type()
    {
        var jsonObject = new
        {
            _type = "declaration.function",
            name = "func",
            parameters = new List<object>(),
            body = new
            {
                _type = "statement.block",
                bindings = new List<object>()
            }
        };

        var declaration = Map(jsonObject) as FunctionDeclaration;

        Assert.NotNull(declaration);
        Assert.Null(declaration.ReturnType);
    }

    [Fact(DisplayName = "maps class declaration")]
    public void maps_class_declaration()
    {
        var jsonObject = new
        {
            _type = "declaration.class",
            name = "myClass",
            body = new
            {
                declarations = new List<object>
                {
                    new
                    {
                        _type = "declaration.variable",
                        name = "field",
                        type = "مقطع"
                    }
                }
            }
        };

        var declaration = Map(jsonObject);

        var expectedDeclaration = new ClassDeclaration()
        {
            Name = "myClass",
            Declarations = new List<Declaration>
            {
                new VariableDeclaration() { Name = "field", Type = DataType.String() }
            }
        };

        Assert.IsType<ClassDeclaration>(declaration);
        declaration.Should().BeEquivalentTo(expectedDeclaration, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps constructor declaration")]
    public void maps_constructor_declaration()
    {
        var jsonObject = new
        {
            _type = "declaration.constructor",
            parameters = new List<object>
            {
                new { name = "param", type = "منطق" }
            },
            body = new
            {
                _type = "statement.block",
                bindings = new List<object> {
                    new
                    {
                        _type = "statement.assignment",
                        target = "id",
                        value = new
                        {
                            _type = "expression.primitive.identifier",
                            value = "param"
                        }
                    }
                }
            }
        };

        var declaration = Map(jsonObject);

        var expectedDeclaration = new ConstructorDeclaration()
        {
            Parameters = new List<Parameter> { new() { Name = "param", Type = DataType.Bool() } },
            Body = new Block()
            {
                Bindings = new List<Binding>
                {
                    new Assignment() { Target = "id", Value = new IdentifierPrimitive() { Value = "param" }}
                }
            }
        };

        Assert.IsType<ConstructorDeclaration>(declaration);
        declaration.Should().BeEquivalentTo(expectedDeclaration, options => options.RespectingRuntimeTypes());
    }
}