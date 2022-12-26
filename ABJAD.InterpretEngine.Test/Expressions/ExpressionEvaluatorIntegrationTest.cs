using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;
using Environment = ABJAD.InterpretEngine.ScopeManagement.Environment;

namespace ABJAD.InterpretEngine.Test.Expressions;

public class ExpressionEvaluatorIntegrationTest
{

    [Fact(DisplayName = "evaluate complex binary expression")]
    public void evaluate_complex_binary_expression()
    {
        var environment = new Environment(new List<Scope>() { ScopeFactory.NewScope() });
        var expressionEvaluator = new ExpressionEvaluator(environment, new StringWriter());
        
        var multiplication = new Multiplication() { FirstOperand = new NumberPrimitive() { Value = 3.0 }, SecondOperand = new NumberPrimitive() { Value = 14.0 }};
        var division = new Division() { FirstOperand = new NumberPrimitive() { Value = 10.0 }, SecondOperand = new NumberPrimitive() { Value = 2.0 }};
        var addition = new Addition() { FirstOperand = multiplication, SecondOperand = division };

        var evaluatedResult = expressionEvaluator.Evaluate(addition);
        
        Assert.True(evaluatedResult.Type.IsNumber());
        Assert.Equal(47.0, evaluatedResult.Value);
    }

    [Fact(DisplayName = "evaluate complex unary expression")]
    public void evaluate_complex_unary_expression()
    {
        var environment = new Environment(new List<Scope>() { ScopeFactory.NewScope() });
        var expressionEvaluator = new ExpressionEvaluator(environment, new StringWriter());

        var negative = new Negative() { Target = new NumberPrimitive() { Value = 10.0 }};
        var greaterCheck = new GreaterCheck() { FirstOperand = negative, SecondOperand = new NumberPrimitive() { Value = 1.0 }};
        var negation = new Negation() { Target = greaterCheck };

        var evaluatedResult = expressionEvaluator.Evaluate(negation);
        
        Assert.True(evaluatedResult.Type.IsBool());
        Assert.Equal(true, evaluatedResult.Value);
    }

    [Fact(DisplayName = "evaluate assignment expression")]
    public void evaluate_assignment_expression()
    {
        var environment = new Environment(new List<Scope>() { ScopeFactory.NewScope() });
        environment.DefineVariable("id", DataType.Number(), 3.0);
        var expressionEvaluator = new ExpressionEvaluator(environment, new StringWriter());

        var value = new Addition() { FirstOperand = new NumberPrimitive() { Value = 2 }, SecondOperand = new NumberPrimitive() { Value = 10 }};
        var additionAssignment = new AdditionAssignment() { Target = "id", Value = value};

        var evaluatedResult = expressionEvaluator.Evaluate(additionAssignment);
        
        Assert.True(evaluatedResult.Type.IsNumber());
        Assert.Equal(15.0, evaluatedResult.Value);
        Assert.Equal(15.0, environment.GetReference("id"));
    }

    [Fact(DisplayName = "evaluate postfix expression")]
    public void evaluate_postfix_expression()
    {
        var environment = new Environment(new List<Scope>() { ScopeFactory.NewScope() });
        environment.DefineVariable("id", DataType.Number(), 3.0);
        var expressionEvaluator = new ExpressionEvaluator(environment, new StringWriter());

        var additionPostfix = new AdditionPostfix() { Target = "id" };
        var evaluatedResult = expressionEvaluator.Evaluate(additionPostfix);
        
        Assert.True(evaluatedResult.Type.IsNumber());
        Assert.Equal(3.0, evaluatedResult.Value);
        Assert.Equal(4.0, environment.GetReference("id"));
    }
    
    [Fact(DisplayName = "evaluate prefix expression")]
    public void evaluate_prefix_expression()
    {
        var environment = new Environment(new List<Scope>() { ScopeFactory.NewScope() });
        environment.DefineVariable("id", DataType.Number(), 3.0);
        var expressionEvaluator = new ExpressionEvaluator(environment, new StringWriter());

        var subtractionPrefix = new SubtractionPrefix() { Target = "id" };
        var evaluatedResult = expressionEvaluator.Evaluate(subtractionPrefix);
        
        Assert.True(evaluatedResult.Type.IsNumber());
        Assert.Equal(2.0, evaluatedResult.Value);
        Assert.Equal(2.0, environment.GetReference("id"));
    }

    [Fact(DisplayName = "evaluate method call")]
    public void evaluate_method_call()
    {
        var environment = new Environment(new List<Scope>() { ScopeFactory.NewScope() });
        environment.DefineVariable("id", DataType.Number(), 3.0);
        var expressionEvaluator = new ExpressionEvaluator(environment, new StringWriter());

        var functionElement = new FunctionElement()
        {
            Parameters = new List<FunctionParameter>()
            {
                new() { Type = DataType.String(), Name = "arg1" },
                new() { Type = DataType.String(), Name = "arg2" },
            },
            ReturnType = DataType.String(),
            Body = new Block()
            {
                Bindings = new List<Binding>
                {
                    new Return()
                    {
                        Target = new Addition()
                        {
                            FirstOperand = new Addition()
                            {
                                FirstOperand = new IdentifierPrimitive() { Value = "arg1" },
                                SecondOperand = new StringPrimitive() { Value = " "}
                            },
                            SecondOperand = new IdentifierPrimitive() { Value = "arg2" }
                        }
                    }
                }
            }
        };
        
        environment.DefineFunction("func", functionElement);

        var methodCall = new MethodCall()
        {
            MethodName = "func", 
            Arguments = new List<Expression>
            {
                new StringPrimitive() { Value = "hello" },
                new StringPrimitive() { Value = "world" }
            }
        };

        var evaluatedResult = expressionEvaluator.Evaluate(methodCall);
        Assert.True(evaluatedResult.Type.IsString());
        Assert.Equal("hello world", evaluatedResult.Value);
    }

    [Fact(DisplayName = "evaluate instantiation expression")]
    public void evaluate_instantiation_expression()
    {
        var environment = new Environment(new List<Scope>() { ScopeFactory.NewScope() });
        var expressionEvaluator = new ExpressionEvaluator(environment, new StringWriter());

        var classElement = new ClassElement()
        {
            Declarations = new List<Declaration>()
            {
                new VariableDeclaration() { Name = "name", Type = DataType.String() },
                new FunctionDeclaration()
                {
                    Name = "getName",
                    ReturnType = DataType.String(),
                    Parameters = new List<Parameter>(),
                    Body = new Block()
                    {
                        Bindings = new List<Binding>
                        {
                            new Return() { Target = new IdentifierPrimitive() { Value = "name" }}
                        }
                    }
                }
            },
            Constructors =
            {
                new ConstructorElement()
                {
                    Parameters = new List<FunctionParameter>()
                    {
                        new() { Name = "_name", Type = DataType.String() }
                    },
                    Body = new Block()
                    {
                        Bindings = new List<Binding>()
                        {
                            new Assignment() { Target = "name", Value = new IdentifierPrimitive() { Value = "_name" }}
                        }
                    }
                }
            }
        };
        
        environment.DefineType("type", classElement);
        var instantiation = new Instantiation()
        {
            ClassName = "type", 
            Arguments = new List<Expression>
            {
                new StringPrimitive() { Value = "the name" }
            }
        };

        var instantiationResult = expressionEvaluator.Evaluate(instantiation);
        Assert.True(instantiationResult.Type.Is("type"));
        
        environment.DefineVariable("instance", DataType.Custom("type"), instantiationResult.Value);

        var instanceFieldAccess = new InstanceFieldAccess() { Instance = "instance", NestedFields = new List<string> { "name" }};
        var instanceFieldAccessResult = expressionEvaluator.Evaluate(instanceFieldAccess);
        Assert.True(instanceFieldAccessResult.Type.IsString());
        Assert.Equal("the name", instanceFieldAccessResult.Value);

        var instanceMethodCall = new InstanceMethodCall()
        {
            Instances = new List<string> { "instance" }, 
            Arguments = new List<Expression>(), 
            MethodName = "getName"
        };
        var instanceMethodCallResult = expressionEvaluator.Evaluate(instanceMethodCall);
        Assert.True(instanceMethodCallResult.Type.IsString());
        Assert.Equal("the name", instanceMethodCallResult.Value);
    }
}