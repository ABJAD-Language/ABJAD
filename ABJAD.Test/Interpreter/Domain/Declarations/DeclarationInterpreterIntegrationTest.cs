using ABJAD.Interpreter.Domain;
using ABJAD.Interpreter.Domain.Declarations;
using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared;
using ABJAD.Interpreter.Domain.Shared.Declarations;
using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Types;
using Environment = ABJAD.Interpreter.Domain.ScopeManagement.Environment;

namespace ABJAD.Test.Interpreter.Domain.Declarations;

public class DeclarationInterpreterIntegrationTest
{
    [Fact(DisplayName = "interpret variable declaration without a value")]
    public void interpret_variable_declaration_without_a_value()
    {
        var environment = new Environment(new List<Scope> { ScopeFactory.NewScope() });
        var interpreter = new DeclarationInterpreter(environment, new StringWriter());

        var declaration = new VariableDeclaration()
        {
            Name = "id",
            Type = DataType.String()
        };

        interpreter.Interpret(declaration);

        Assert.True(environment.GetReferenceType("id").IsString());
        Assert.Equal(SpecialValues.UNDEFINED, environment.GetReference("id"));
        Assert.False(environment.IsReferenceConstant("id"));
    }

    [Fact(DisplayName = "interpret constant declaration")]
    public void interpret_constant_declaration()
    {
        var environment = new Environment(new List<Scope> { ScopeFactory.NewScope() });
        var interpreter = new DeclarationInterpreter(environment, new StringWriter());

        var declaration = new ConstantDeclaration()
        {
            Name = "id",
            Type = DataType.Number(),
            Value = new NumberPrimitive() { Value = 12.0 }
        };

        interpreter.Interpret(declaration);

        Assert.True(environment.GetReferenceType("id").IsNumber());
        Assert.Equal(12.0, environment.GetReference("id"));
        Assert.True(environment.IsReferenceConstant("id"));
    }

    [Fact(DisplayName = "interpret function declaration")]
    public void interpret_function_declaration()
    {
        var environment = new Environment(new List<Scope> { ScopeFactory.NewScope() });
        var interpreter = new DeclarationInterpreter(environment, new StringWriter());

        var declaration = new FunctionDeclaration()
        {
            Name = "func",
            ReturnType = DataType.Number(),
            Parameters = new List<Parameter>()
            {
                new() { Name = "param", Type = DataType.Custom("type") }
            },
            Body = new Block()
            {
                Bindings = new List<Binding>()
                {
                    new Return() { Target = new NumberPrimitive() { Value = 1.0 }}
                }
            }
        };

        interpreter.Interpret(declaration);

        Assert.True(environment.FunctionExists("func", DataType.Custom("type")));
        Assert.True(environment.GetFunctionReturnType("func", DataType.Custom("type")).IsNumber());

        var functionElement = environment.GetFunction("func", DataType.Custom("type"));
        Assert.Equal(declaration.Body, functionElement.Body);
    }

    [Fact(DisplayName = "interpret class declaration")]
    public void interpret_class_declaration()
    {
        var environment = new Environment(new List<Scope> { ScopeFactory.NewScope() });
        var interpreter = new DeclarationInterpreter(environment, new StringWriter());

        var assignment = new Assignment() { Target = "field", Value = new IdentifierPrimitive() { Value = "_field" } };
        var declaration = new ClassDeclaration()
        {
            Name = "type",
            Declarations = new List<Declaration>()
            {
                new VariableDeclaration() { Name = "field", Type = DataType.String() },
                new ConstructorDeclaration()
                {
                    Parameters = new List<Parameter>()
                    {
                        new() { Name = "_field", Type = DataType.String() }
                    },
                    Body = new Block()
                    {
                        Bindings = new List<Binding>()
                        {
                            assignment
                        }
                    }
                }
            }
        };

        interpreter.Interpret(declaration);

        Assert.True(environment.TypeExists("type"));
        Assert.True(environment.TypeHasConstructor("type", DataType.String()));

        var classElement = environment.GetType("type");
        Assert.Equal(assignment, classElement.Constructors[0].Body.Bindings[0]);
    }
}