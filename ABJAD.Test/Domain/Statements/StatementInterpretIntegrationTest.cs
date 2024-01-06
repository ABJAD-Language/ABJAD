using ABJAD.Interpreter.Domain;
using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Shared;
using ABJAD.Interpreter.Domain.Shared.Declarations;
using ABJAD.Interpreter.Domain.Shared.Expressions.Binary;
using ABJAD.Interpreter.Domain.Shared.Expressions.Fixes;
using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;
using ABJAD.Interpreter.Domain.Shared.Statements;
using ABJAD.Interpreter.Domain.Statements;
using ABJAD.Interpreter.Domain.Types;
using Environment = ABJAD.Interpreter.Domain.ScopeManagement.Environment;

namespace ABJAD.Test.Domain.Statements;

public class StatementInterpretIntegrationTest
{
    [Fact(DisplayName = "interpret assignment statement")]
    public void interpret_assignment_statement()
    {
        var environment = new Environment(new List<Scope> { ScopeFactory.NewScope() });
        var interpreter = new StatementInterpreter(environment, new StringWriter());

        environment.DefineVariable("id", DataType.Number(), SpecialValues.UNDEFINED);
        var assignment = new Assignment() { Target = "id", Value = new NumberPrimitive() { Value = 13.0 } };
        interpreter.Interpret(assignment);

        Assert.Equal(13.0, environment.GetReference("id"));
    }

    [Fact(DisplayName = "interpret block statement")]
    public void interpret_block_statement()
    {
        var environment = new Environment(new List<Scope> { ScopeFactory.NewScope() });
        var writer = new StringWriter();
        var interpreter = new StatementInterpreter(environment, writer);

        environment.DefineVariable("globalId", DataType.Bool(), SpecialValues.UNDEFINED);

        var block = new Block()
        {
            Bindings = new List<Binding>()
            {
                new ConstantDeclaration() { Name = "id", Type = DataType.String(), Value = new StringPrimitive() { Value = "hello world" }},
                new Assignment() { Target = "globalId", Value = new BoolPrimitive() { Value = true }},
                new Print() { Target = new IdentifierPrimitive() { Value = "id" }},
                new Print() { Target = new IdentifierPrimitive() { Value = "globalId" }},
            }
        };

        interpreter.Interpret(block);

        Assert.Equal($"hello world{writer.NewLine}صحيح{writer.NewLine}", writer.ToString());
        Assert.Equal(true, environment.GetReference("globalId"));
        Assert.False(environment.ReferenceExists("id"));
    }

    [Fact(DisplayName = "interpret if else statement")]
    public void interpret_if_else_statement()
    {
        var environment = new Environment(new List<Scope> { ScopeFactory.NewScope() });
        var writer = new StringWriter();
        var interpreter = new StatementInterpreter(environment, writer);

        environment.DefineVariable("conditional1", DataType.Bool(), false);
        environment.DefineVariable("conditional2", DataType.Bool(), false);
        environment.DefineVariable("conditional3", DataType.Bool(), true);

        var ifElse = new IfElse()
        {
            MainConditional = new Conditional()
            {
                Condition = new IdentifierPrimitive() { Value = "conditional1" },
                Body = new Print() { Target = new StringPrimitive() { Value = "block 1 executed" } }
            },
            OtherConditionals = new List<Conditional>()
            {
                new()
                {
                    Condition = new IdentifierPrimitive() { Value = "conditional2" },
                    Body = new Print() { Target = new StringPrimitive() { Value = "block 2 executed"}}
                },
                new()
                {
                    Condition = new IdentifierPrimitive() { Value = "conditional3" },
                    Body = new Print() { Target = new StringPrimitive() { Value = "block 3 executed"}}
                }
            },
            ElseBody = new Print() { Target = new StringPrimitive() { Value = "else block executed" } }
        };

        interpreter.Interpret(ifElse);

        Assert.Equal($"block 3 executed{writer.NewLine}", writer.ToString());
    }

    [Fact(DisplayName = "interprets while loop")]
    public void interprets_while_loop()
    {
        var environment = new Environment(new List<Scope> { ScopeFactory.NewScope() });
        var writer = new StringWriter();
        var interpreter = new StatementInterpreter(environment, writer);

        environment.DefineVariable("counter", DataType.Number(), 0.0);

        var whileLoop = new WhileLoop()
        {
            Condition = new LessCheck()
            {
                FirstOperand = new IdentifierPrimitive() { Value = "counter" },
                SecondOperand = new NumberPrimitive() { Value = 3.0 }
            },
            Body = new Block()
            {
                Bindings = new List<Binding>
                {
                    new ExpressionStatement() { Target = new AdditionPostfix() { Target = "counter" }},
                    new Print() { Target = new IdentifierPrimitive() { Value = "counter" }}
                }
            }
        };

        interpreter.Interpret(whileLoop);

        Assert.Equal($"1{writer.NewLine}2{writer.NewLine}3{writer.NewLine}", writer.ToString());
        Assert.Equal(3.0, environment.GetReference("counter"));
    }

    [Fact(DisplayName = "interpret for loop")]
    public void interpret_for_loop()
    {
        var environment = new Environment(new List<Scope> { ScopeFactory.NewScope() });
        var writer = new StringWriter();
        var interpreter = new StatementInterpreter(environment, writer);

        var forLoop = new ForLoop()
        {
            TargetDefinition = new VariableDeclaration()
            {
                Name = "counter",
                Type = DataType.Number(),
                Value = new NumberPrimitive() { Value = 0.0 }
            },
            Condition = new ExpressionStatement()
            {
                Target = new LessCheck()
                {
                    FirstOperand = new IdentifierPrimitive() { Value = "counter" },
                    SecondOperand = new NumberPrimitive() { Value = 4.0 }
                }
            },
            Callback = new AdditionPostfix() { Target = "counter" },
            Body = new Print() { Target = new StringPrimitive() { Value = "hello" } }
        };

        interpreter.Interpret(forLoop);

        Assert.Equal($"hello{writer.NewLine}hello{writer.NewLine}hello{writer.NewLine}hello{writer.NewLine}", writer.ToString());
        Assert.False(environment.ReferenceExists("counter"));
    }
}