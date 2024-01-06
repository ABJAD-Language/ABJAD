using ABJAD.Interpreter.Domain.Shared.Expressions;
using ABJAD.Interpreter.Domain.Shared.Expressions.Assignments;
using ABJAD.Interpreter.Domain.Shared.Expressions.Binary;
using ABJAD.Interpreter.Domain.Shared.Expressions.Fixes;
using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;
using ABJAD.Interpreter.Domain.Shared.Expressions.Unary;
using FluentAssertions;
using static ABJAD.Interpreter.Mappers.ExpressionMapper;

namespace ABJAD.Test.Mappers;

public class ExpressionMapperTest
{
    [Fact(DisplayName = "maps call expression")]
    public void maps_call_expression()
    {
        var jsonObject = new
        {
            _type = "expression.call",
            method = new
            {
                _type = "expression.primitive.identifier",
                value = "methodName"
            },
            arguments = new List<object>
            {
                new
                {
                    _type = "expression.primitive.string",
                    value = "hello"
                }
            }
        };

        var expression = Map(jsonObject);

        var expectedExpression = new MethodCall()
        {
            MethodName = "methodName",
            Arguments = new List<Expression>
            {
                new StringPrimitive() { Value = "hello" }
            }
        };

        Assert.IsType<MethodCall>(expression);
        expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps instance field access expression")]
    public void maps_instance_field_access_expression()
    {
        var jsonObject = new
        {
            _type = "expression.instanceField",
            instance = new
            {
                _type = "expression.primitive.identifier",
                value = "instance"
            },
            fields = new List<object>
            {
                new
                {
                    _type = "expression.primitive.identifier",
                    value = "field1"
                },
                new
                {
                    _type = "expression.primitive.identifier",
                    value = "field2"
                },
            }
        };

        var expression = Map(jsonObject);

        var expectedExpression = new InstanceFieldAccess()
        {
            Instance = "instance",
            NestedFields = new List<string> { "field1", "field2" }
        };

        Assert.IsType<InstanceFieldAccess>(expression);
        expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps instance method call expression")]
    public void maps_instance_method_call_expression()
    {
        var jsonObject = new
        {
            _type = "expression.instanceMethodCall",
            instances = new List<object>
            {
                new
                {
                    _type = "expression.primitive.identifier",
                    value = "instance1"
                },
                new
                {
                    _type = "expression.primitive.identifier",
                    value = "instance2"
                }
            },
            method = new
            {
                _type = "expression.primitive.identifier",
                value = "methodName"
            },
            arguments = new List<object>
            {
                new
                {
                    _type = "expression.primitive.string",
                    value = "hello"
                }
            }
        };

        var expression = Map(jsonObject);

        var expectedExpression = new InstanceMethodCall()
        {
            Instances = new List<string>() { "instance1", "instance2" },
            MethodName = "methodName",
            Arguments = new List<Expression> { new StringPrimitive() { Value = "hello" } }
        };

        Assert.IsType<InstanceMethodCall>(expression);
        expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps instantiation expression")]
    public void maps_instantiation_expression()
    {
        var jsonObject = new
        {
            _type = "expression.instantiation",
            @class = new
            {
                _type = "expression.primitive.identifier",
                value = "className"
            },
            arguments = new List<object>
            {
                new
                {
                    _type = "expression.primitive.number",
                    value = 32.0
                }
            }
        };

        var expression = Map(jsonObject);

        var expectedExpression = new Instantiation()
        {
            ClassName = "className",
            Arguments = new List<Expression>
            {
                new NumberPrimitive() { Value = 32.0 }
            }
        };

        Assert.IsType<Instantiation>(expression);
        expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
    }

    public class PrimitiveMappingTest
    {
        [Fact(DisplayName = "maps number primitive")]
        public void maps_number_primitive()
        {
            var jsonObject = new { _type = "expression.primitive.number", value = 3.0 };
            var expression = Map(jsonObject);

            var expectedExpression = new NumberPrimitive() { Value = 3.0 };
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps bool primitive")]
        public void maps_bool_primitive()
        {
            var jsonObject = new { _type = "expression.primitive.bool", value = true };
            var expression = Map(jsonObject);

            var expectedExpression = new BoolPrimitive() { Value = true };
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps string primitive")]
        public void maps_string_primitive()
        {
            var jsonObject = new { _type = "expression.primitive.string", value = "hello world" };
            var expression = Map(jsonObject);

            var expectedExpression = new StringPrimitive() { Value = "hello world" };
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps identifier primitive")]
        public void maps_identifier_primitive()
        {
            var jsonObject = new { _type = "expression.primitive.identifier", value = "id" };
            var expression = Map(jsonObject);

            var expectedExpression = new IdentifierPrimitive() { Value = "id" };
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps null primitive")]
        public void maps_null_primitive()
        {
            var jsonObject = new { _type = "expression.primitive.null" };
            var expression = Map(jsonObject);

            Assert.IsType<NullPrimitive>(expression);
        }
    }

    public class AssignmentMappingTest
    {
        [Fact(DisplayName = "maps addition assigment expression")]
        public void maps_addition_assigment_expression()
        {
            var jsonObject = new
            {
                _type = "expression.assignment.addition",
                target = "id",
                value = new { _type = "expression.primitive.number", value = 2.0 }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new AdditionAssignment() { Target = "id", Value = new NumberPrimitive() { Value = 2.0 } };
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps subtraction assignment expression")]
        public void maps_subtraction_assignment_expression()
        {
            var jsonObject = new
            {
                _type = "expression.assignment.subtraction",
                target = "id",
                value = new { _type = "expression.primitive.number", value = -10.0 }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new SubtractionAssignment() { Target = "id", Value = new NumberPrimitive() { Value = -10.0 } };
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps multiplication assignment expression")]
        public void maps_multiplication_assignment_expression()
        {
            var jsonObject = new
            {
                _type = "expression.assignment.multiplication",
                target = "id",
                value = new { _type = "expression.primitive.number", value = 6.0 }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new MultiplicationAssignment() { Target = "id", Value = new NumberPrimitive() { Value = 6.0 } };
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps division assignment expression")]
        public void maps_division_assignment_expression()
        {
            var jsonObject = new
            {
                _type = "expression.assignment.division",
                target = "id",
                value = new { _type = "expression.primitive.number", value = 3.0 }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new DivisionAssignment() { Target = "id", Value = new NumberPrimitive() { Value = 3.0 } };
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }
    }

    public class UnaryExpressionMappingTest
    {
        [Fact(DisplayName = "maps negation expression")]
        public void maps_negation_expression()
        {
            var jsonObject = new
            {
                _type = "expression.unary.negation",
                target = new
                {
                    _type = "expression.primitive.bool",
                    value = true
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new Negation() { Target = new BoolPrimitive() { Value = true } };
            Assert.IsType<Negation>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps negative expression")]
        public void maps_negative_expression()
        {
            var jsonObject = new
            {
                _type = "expression.unary.negative",
                target = new
                {
                    _type = "expression.primitive.number",
                    value = 30.0
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new Negative() { Target = new NumberPrimitive() { Value = 30.0 } };
            Assert.IsType<Negative>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps addition postfix expression")]
        public void maps_addition_postfix_expression()
        {
            var jsonObject = new
            {
                _type = "expression.unary.postfix.addition",
                target = new
                {
                    _type = "expression.primitive.identifier",
                    value = "id"
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new AdditionPostfix() { Target = "id" };
            Assert.IsType<AdditionPostfix>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps subtraction postfix expression")]
        public void maps_subtraction_postfix_expression()
        {
            var jsonObject = new
            {
                _type = "expression.unary.postfix.subtraction",
                target = new
                {
                    _type = "expression.primitive.identifier",
                    value = "id"
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new SubtractionPostfix() { Target = "id" };
            Assert.IsType<SubtractionPostfix>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps addition prefix expression")]
        public void maps_addition_prefix_expression()
        {
            var jsonObject = new
            {
                _type = "expression.unary.prefix.addition",
                target = new
                {
                    _type = "expression.primitive.identifier",
                    value = "id"
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new AdditionPrefix() { Target = "id" };
            Assert.IsType<AdditionPrefix>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps subtraction prefix expression")]
        public void maps_subtraction_prefix_expression()
        {
            var jsonObject = new
            {
                _type = "expression.unary.prefix.subtraction",
                target = new
                {
                    _type = "expression.primitive.identifier",
                    value = "id"
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new SubtractionPrefix() { Target = "id" };
            Assert.IsType<SubtractionPrefix>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps to bool expression")]
        public void maps_to_bool_expression()
        {
            var jsonObject = new
            {
                _type = "expression.unary.toBool",
                target = new
                {
                    _type = "expression.primitive.identifier",
                    value = "id"
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new ToBool() { Target = new IdentifierPrimitive() { Value = "id" } };
            Assert.IsType<ToBool>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps to number expression")]
        public void maps_to_number_expression()
        {
            var jsonObject = new
            {
                _type = "expression.unary.toNumber",
                target = new
                {
                    _type = "expression.primitive.identifier",
                    value = "id"
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new ToNumber() { Target = new IdentifierPrimitive() { Value = "id" } };
            Assert.IsType<ToNumber>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps to string expression")]
        public void maps_to_string_expression()
        {
            var jsonObject = new
            {
                _type = "expression.unary.toString",
                target = new
                {
                    _type = "expression.primitive.identifier",
                    value = "id"
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new ToString() { Target = new IdentifierPrimitive() { Value = "id" } };
            Assert.IsType<ToString>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps type of expression")]
        public void maps_type_of_expression()
        {
            var jsonObject = new
            {
                _type = "expression.unary.typeOf",
                target = new
                {
                    _type = "expression.primitive.identifier",
                    value = "id"
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new TypeOf() { Target = new IdentifierPrimitive() { Value = "id" } };
            Assert.IsType<TypeOf>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }
    }

    public class BinaryExpressionMappingTest
    {
        [Fact(DisplayName = "maps addition expression")]
        public void maps_addition_expression()
        {
            var jsonObject = new
            {
                _type = "expression.binary.addition",
                firstOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 2.0
                },
                secondOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 4.0
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new Addition()
            {
                FirstOperand = new NumberPrimitive() { Value = 2.0 },
                SecondOperand = new NumberPrimitive() { Value = 4.0 }
            };

            Assert.IsType<Addition>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps subtraction expression")]
        public void maps_subtraction_expression()
        {
            var jsonObject = new
            {
                _type = "expression.binary.subtraction",
                firstOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 2.0
                },
                secondOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 4.0
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new Subtraction()
            {
                FirstOperand = new NumberPrimitive() { Value = 2.0 },
                SecondOperand = new NumberPrimitive() { Value = 4.0 }
            };

            Assert.IsType<Subtraction>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps multiplication expression")]
        public void maps_multiplication_expression()
        {
            var jsonObject = new
            {
                _type = "expression.binary.multiplication",
                firstOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 2.0
                },
                secondOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 4.0
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new Multiplication()
            {
                FirstOperand = new NumberPrimitive() { Value = 2.0 },
                SecondOperand = new NumberPrimitive() { Value = 4.0 }
            };

            Assert.IsType<Multiplication>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps division expression")]
        public void maps_division_expression()
        {
            var jsonObject = new
            {
                _type = "expression.binary.division",
                firstOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 2.0
                },
                secondOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 4.0
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new Division()
            {
                FirstOperand = new NumberPrimitive() { Value = 2.0 },
                SecondOperand = new NumberPrimitive() { Value = 4.0 }
            };

            Assert.IsType<Division>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps and operation expression")]
        public void maps_and_operation_expression()
        {
            var jsonObject = new
            {
                _type = "expression.binary.and",
                firstOperand = new
                {
                    _type = "expression.primitive.bool",
                    value = true
                },
                secondOperand = new
                {
                    _type = "expression.primitive.bool",
                    value = false
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new LogicalAnd()
            {
                FirstOperand = new BoolPrimitive() { Value = true },
                SecondOperand = new BoolPrimitive() { Value = false }
            };

            Assert.IsType<LogicalAnd>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps or operation expression")]
        public void maps_or_operation_expression()
        {
            var jsonObject = new
            {
                _type = "expression.binary.or",
                firstOperand = new
                {
                    _type = "expression.primitive.bool",
                    value = true
                },
                secondOperand = new
                {
                    _type = "expression.primitive.bool",
                    value = false
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new LogicalOr()
            {
                FirstOperand = new BoolPrimitive() { Value = true },
                SecondOperand = new BoolPrimitive() { Value = false }
            };

            Assert.IsType<LogicalOr>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps equality check expression")]
        public void maps_equality_check_expression()
        {
            var jsonObject = new
            {
                _type = "expression.binary.equalityCheck",
                firstOperand = new
                {
                    _type = "expression.primitive.bool",
                    value = true
                },
                secondOperand = new
                {
                    _type = "expression.primitive.bool",
                    value = false
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new EqualityCheck()
            {
                FirstOperand = new BoolPrimitive() { Value = true },
                SecondOperand = new BoolPrimitive() { Value = false }
            };

            Assert.IsType<EqualityCheck>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps greater check expression")]
        public void maps_greater_check_expression()
        {
            var jsonObject = new
            {
                _type = "expression.binary.greaterCheck",
                firstOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 2.0
                },
                secondOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 4.0
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new GreaterCheck()
            {
                FirstOperand = new NumberPrimitive() { Value = 2.0 },
                SecondOperand = new NumberPrimitive() { Value = 4.0 }
            };

            Assert.IsType<GreaterCheck>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps greater or equal check expression")]
        public void maps_greater_or_equal_check_expression()
        {
            var jsonObject = new
            {
                _type = "expression.binary.greaterOrEqualCheck",
                firstOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 2.0
                },
                secondOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 4.0
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new GreaterOrEqualCheck()
            {
                FirstOperand = new NumberPrimitive() { Value = 2.0 },
                SecondOperand = new NumberPrimitive() { Value = 4.0 }
            };

            Assert.IsType<GreaterOrEqualCheck>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps inequality check expression")]
        public void maps_inequality_check_expression()
        {
            var jsonObject = new
            {
                _type = "expression.binary.inequalityCheck",
                firstOperand = new
                {
                    _type = "expression.primitive.bool",
                    value = true
                },
                secondOperand = new
                {
                    _type = "expression.primitive.bool",
                    value = false
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new InequalityCheck()
            {
                FirstOperand = new BoolPrimitive() { Value = true },
                SecondOperand = new BoolPrimitive() { Value = false }
            };

            Assert.IsType<InequalityCheck>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }

        [Fact(DisplayName = "maps less check expression")]
        public void maps_less_check_expression()
        {
            var jsonObject = new
            {
                _type = "expression.binary.lessCheck",
                firstOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 2.0
                },
                secondOperand = new
                {
                    _type = "expression.primitive.number",
                    value = 4.0
                }
            };
            var expression = Map(jsonObject);

            var expectedExpression = new LessCheck()
            {
                FirstOperand = new NumberPrimitive() { Value = 2.0 },
                SecondOperand = new NumberPrimitive() { Value = 4.0 }
            };

            Assert.IsType<LessCheck>(expression);
            expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
        }
    }

    [Fact(DisplayName = "maps less or equal check expression")]
    public void maps_less_or_equal_check_expression()
    {
        var jsonObject = new
        {
            _type = "expression.binary.lessOrEqualCheck",
            firstOperand = new
            {
                _type = "expression.primitive.number",
                value = 2.0
            },
            secondOperand = new
            {
                _type = "expression.primitive.number",
                value = 4.0
            }
        };
        var expression = Map(jsonObject);

        var expectedExpression = new LessOrEqualCheck()
        {
            FirstOperand = new NumberPrimitive() { Value = 2.0 },
            SecondOperand = new NumberPrimitive() { Value = 4.0 }
        };

        Assert.IsType<LessOrEqualCheck>(expression);
        expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
    }

    [Fact(DisplayName = "maps modulo expression")]
    public void maps_modulo_expression()
    {
        var jsonObject = new
        {
            _type = "expression.binary.modulo",
            firstOperand = new
            {
                _type = "expression.primitive.number",
                value = 2.0
            },
            secondOperand = new
            {
                _type = "expression.primitive.number",
                value = 4.0
            }
        };
        var expression = Map(jsonObject);

        var expectedExpression = new Modulo()
        {
            FirstOperand = new NumberPrimitive() { Value = 2.0 },
            SecondOperand = new NumberPrimitive() { Value = 4.0 }
        };

        Assert.IsType<Modulo>(expression);
        expression.Should().BeEquivalentTo(expectedExpression, options => options.RespectingRuntimeTypes());
    }
}