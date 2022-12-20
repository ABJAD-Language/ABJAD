using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions.Strategies;

public class BinaryExpressionEvaluationStrategyTest
{
    private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
    private readonly Expression firstOperand = Substitute.For<Expression>();
    private readonly Expression secondOperand = Substitute.For<Expression>();
    
    [Fact(DisplayName = "throws error if the value of the first operand is undefined")]
    public void throws_error_if_the_value_of_the_first_operand_is_undefined()
    {
        expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Value = SpecialValues.UNDEFINED });
        expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Value = new object() });
            
        var addition = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };
        var strategy = new BinaryExpressionEvaluationStrategy(addition, expressionEvaluator);

        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }
    
    [Fact(DisplayName = "throws error if the value of the second operand is undefined")]
    public void throws_error_if_the_value_of_the_second_operand_is_undefined()
    {
        expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Value = new object() });
        expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Value = SpecialValues.UNDEFINED });
            
        var addition = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };
        var strategy = new BinaryExpressionEvaluationStrategy(addition, expressionEvaluator);

        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }
    
    public class AdditionInterpretingTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error if the first operand is neither a number nor a string")]
        public void throws_error_if_the_first_operand_is_neither_a_number_nor_a_string()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });

            firstOperandDataType.IsNumber().Returns(false);
            firstOperandDataType.IsString().Returns(false);
            secondOperandDataType.IsString().Returns(true);
            
            var binaryExpression = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };

            var strategy = new BinaryExpressionEvaluationStrategy(binaryExpression, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the second operand is neither a number nor a string")]
        public void throws_error_if_the_second_operand_is_neither_a_number_nor_a_string()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });

            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsString().Returns(false);

            var binaryExpression = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };

            var strategy = new BinaryExpressionEvaluationStrategy(binaryExpression, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the first operand is null")]
        public void throws_error_if_the_first_operand_is_null()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = SpecialValues.NULL });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = "hello" });
            
            firstOperandDataType.IsString().Returns(true);
            secondOperandDataType.IsString().Returns(true);

            var addition = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };
            var strategy = new BinaryExpressionEvaluationStrategy(addition, expressionEvaluator);

            Assert.Throws<NullPointerException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the first operand is string and the second operand is null")]
        public void throws_error_if_the_first_operand_is_string_and_the_second_operand_is_null()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = "hey" });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = SpecialValues.NULL });
            
            firstOperandDataType.IsString().Returns(true);
            secondOperandDataType.IsString().Returns(true);

            var addition = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };
            var strategy = new BinaryExpressionEvaluationStrategy(addition, expressionEvaluator);

            Assert.Throws<NullPointerException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the first operand is number and the second operand is null")]
        public void throws_error_if_the_first_operand_is_number_and_the_second_operand_is_null()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 3.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = SpecialValues.NULL });
            
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsString().Returns(true);

            var addition = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };
            var strategy = new BinaryExpressionEvaluationStrategy(addition, expressionEvaluator);

            Assert.Throws<NullPointerException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns the sum of the two numbers when both operands are numbers")]
        public void returns_the_sum_of_the_two_numbers_when_both_operands_are_numbers()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 3.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 2.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
            
            var binaryExpression = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };

            var strategy = new BinaryExpressionEvaluationStrategy(binaryExpression, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(5.0, result.Value);
        }

        [Fact(DisplayName = "returns the concatenation of the two strings when both operands are strings")]
        public void returns_the_concatenation_of_the_two_strings_when_both_operands_are_strings()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = "hello " });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = "world" });
            firstOperandDataType.IsString().Returns(true);
            secondOperandDataType.IsString().Returns(true);
            
            var binaryExpression = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };

            var strategy = new BinaryExpressionEvaluationStrategy(binaryExpression, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsString());
            Assert.Equal("hello world", result.Value);
        }

        [Fact(DisplayName = "returns the string concatenation of the operands when the first one is number and the second is string")]
        public void returns_the_string_concatenation_of_the_operands_when_the_first_one_is_number_and_the_second_is_string()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 1.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = " is the answer" });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsString().Returns(true);

            var binaryExpression = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };

            var strategy = new BinaryExpressionEvaluationStrategy(binaryExpression, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsString());
            Assert.Equal("1 is the answer", result.Value);
        }

        [Fact(DisplayName = "returns the string concatenation of the operands when the first one is string and the second is number")]
        public void returns_the_string_concatenation_of_the_operands_when_the_first_one_is_string_and_the_second_is_number()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = "the answer is " });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 7.0 });
            firstOperandDataType.IsString().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);

            var binaryExpression = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };

            var strategy = new BinaryExpressionEvaluationStrategy(binaryExpression, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsString());
            Assert.Equal("the answer is 7", result.Value);
        }
    }

    public class SubtractionInterpretingTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error if the first operand is not a number")]
        public void throws_error_if_the_first_operand_is_not_a_number()
        {
            var subtraction = new Subtraction { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionEvaluationStrategy(subtraction, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the second operand is not a number")]
        public void throws_error_if_the_second_operand_is_not_a_number()
        {
            var subtraction = new Subtraction { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
            var strategy = new BinaryExpressionEvaluationStrategy(subtraction, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns the difference of the two operands")]
        public void returns_the_difference_of_the_two_operands()
        {
            var subtraction = new Subtraction { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 3.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 8.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionEvaluationStrategy(subtraction, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(-5.0, result.Value);
        }
    }
    
    public class MultiplicationInterpretingTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error if the first operand is not a number")]
        public void throws_error_if_the_first_operand_is_not_a_number()
        {
            var multiplication = new Multiplication { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionEvaluationStrategy(multiplication, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the second operand is not a number")]
        public void throws_error_if_the_second_operand_is_not_a_number()
        {
            var multiplication = new Multiplication { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
            var strategy = new BinaryExpressionEvaluationStrategy(multiplication, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns the product of the two operands")]
        public void returns_the_product_of_the_two_operands()
        {
            var multiplication = new Multiplication { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = -4.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 2.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionEvaluationStrategy(multiplication, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(-8.0, result.Value);
        }
    }
    
    public class DivisionInterpretingTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error if the first operand is not a number")]
        public void throws_error_if_the_first_operand_is_not_a_number()
        {
            var division = new Division { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionEvaluationStrategy(division, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the second operand is not a number")]
        public void throws_error_if_the_second_operand_is_not_a_number()
        {
            var division = new Division { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
            var strategy = new BinaryExpressionEvaluationStrategy(division, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if trying to divide by zero")]
        public void throws_error_if_trying_to_divide_by_zero()
        {
            var division = new Division() { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 9.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 0.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionEvaluationStrategy(division, expressionEvaluator);
            Assert.Throws<DivisionByZeroException>(() => strategy.Apply());
        }
        
        [Fact(DisplayName = "returns the product of the two operands")]
        public void returns_the_product_of_the_two_operands()
        {
            var division = new Division { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 12.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 3.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionEvaluationStrategy(division, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(4.0, result.Value);
        }
    }
    
    public class ModuloInterpretingTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error if the first operand is not a number")]
        public void throws_error_if_the_first_operand_is_not_a_number()
        {
            var modulo = new Modulo { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionEvaluationStrategy(modulo, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the second operand is not a number")]
        public void throws_error_if_the_second_operand_is_not_a_number()
        {
            var modulo = new Modulo { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
            var strategy = new BinaryExpressionEvaluationStrategy(modulo, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }
        
        [Fact(DisplayName = "returns the modulo of the two operands")]
        public void returns_the_modulo_of_the_two_operands()
        {
            var modulo = new Modulo { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 10.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 3.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionEvaluationStrategy(modulo, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(1.0, result.Value);
        }
    }

    public class LogicalAndInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error if the first operand is not a bool")]
        public void throws_error_if_the_first_operand_is_not_a_bool()
        {
            var logicalAnd = new LogicalAnd { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsBool().Returns(false);
            secondOperandDataType.IsBool().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(logicalAnd, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the second operand is not a bool")]
        public void throws_error_if_the_second_operand_is_not_a_bool()
        {
            var logicalAnd = new LogicalAnd { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsBool().Returns(true);
            secondOperandDataType.IsBool().Returns(false);

            var strategy = new BinaryExpressionEvaluationStrategy(logicalAnd, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns true when both operands evaluate to true")]
        public void returns_true_when_both_operands_evaluate_to_true()
        {
            var logicalAnd = new LogicalAnd { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = true});
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = true});
            firstOperandDataType.IsBool().Returns(true);
            secondOperandDataType.IsBool().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(logicalAnd, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }

        [Fact(DisplayName = "returns false when first operand evaluates to false")]
        public void returns_false_when_first_operand_evaluates_to_false()
        {
            var logicalAnd = new LogicalAnd { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = false});
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = true});
            firstOperandDataType.IsBool().Returns(true);
            secondOperandDataType.IsBool().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(logicalAnd, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }

        [Fact(DisplayName = "returns false when second operand evaluates to false")]
        public void returns_false_when_second_operand_evaluates_to_false()
        {
            var logicalAnd = new LogicalAnd { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = true});
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = false});
            firstOperandDataType.IsBool().Returns(true);
            secondOperandDataType.IsBool().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(logicalAnd, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }

        [Fact(DisplayName = "returns false when both operands evaluate to false")]
        public void returns_false_when_both_operands_evaluate_to_false()
        {
            var logicalAnd = new LogicalAnd { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = false});
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = false});
            firstOperandDataType.IsBool().Returns(true);
            secondOperandDataType.IsBool().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(logicalAnd, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }
    }

    public class LogicalOrInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error if the first operand is not a bool")]
        public void throws_error_if_the_first_operand_is_not_a_bool()
        {
            var logicalOr = new LogicalOr { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsBool().Returns(false);
            secondOperandDataType.IsBool().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(logicalOr, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the second operand is not a bool")]
        public void throws_error_if_the_second_operand_is_not_a_bool()
        {
            var logicalOr = new LogicalOr { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsBool().Returns(true);
            secondOperandDataType.IsBool().Returns(false);
        
            var strategy = new BinaryExpressionEvaluationStrategy(logicalOr, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }
        
        [Fact(DisplayName = "returns true when both operands evaluate to true")]
        public void returns_true_when_both_operands_evaluate_to_true()
        {
            var logicalOr = new LogicalOr { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = true});
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = true});
            firstOperandDataType.IsBool().Returns(true);
            secondOperandDataType.IsBool().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(logicalOr, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }
        
        [Fact(DisplayName = "returns true when first operand evaluates to true")]
        public void returns_true_when_first_operand_evaluates_to_false()
        {
            var logicalOr = new LogicalOr { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = true});
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = false});
            firstOperandDataType.IsBool().Returns(true);
            secondOperandDataType.IsBool().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(logicalOr, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }
        
        [Fact(DisplayName = "returns true when second operand evaluates to true")]
        public void returns_false_when_second_operand_evaluates_to_false()
        {
            var logicalOr = new LogicalOr { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = false});
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = true});
            firstOperandDataType.IsBool().Returns(true);
            secondOperandDataType.IsBool().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(logicalOr, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }
        
        [Fact(DisplayName = "returns false when both operands evaluate to false")]
        public void returns_false_when_both_operands_evaluate_to_false()
        {
            var logicalOr = new LogicalOr { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = false});
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = false});
            firstOperandDataType.IsBool().Returns(true);
            secondOperandDataType.IsBool().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(logicalOr, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }
    }

    public class GreaterCheckInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error when the first operand is not a number")]
        public void throws_error_when_the_first_operand_is_not_a_number()
        {
            var greaterCheck = new GreaterCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsNumber().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(greaterCheck, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error when the second operand is not a number")]
        public void throws_error_when_the_second_operand_is_not_a_number()
        {
            var greaterCheck = new GreaterCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);

            var strategy = new BinaryExpressionEvaluationStrategy(greaterCheck, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns true when the operands are numbers and the first one is bigger")]
        public void returns_true_when_the_operands_are_numbers_and_the_first_one_is_bigger()
        {
            var greaterCheck = new GreaterCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 10.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 6.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(greaterCheck, expressionEvaluator);
            var result = strategy.Apply();
            
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }

        [Fact(DisplayName = "returns false when the operands are numbers and equal")]
        public void returns_false_when_the_operands_are_numbers_and_equal()
        {
            var greaterCheck = new GreaterCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 10.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 10.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(greaterCheck, expressionEvaluator);
            var result = strategy.Apply();
            
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }

        [Fact(DisplayName = "returns false when the operands are numbers and the first is smaller")]
        public void returns_false_when_the_operands_are_numbers_and_the_first_is_smaller()
        {
            var greaterCheck = new GreaterCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 3.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 10.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(greaterCheck, expressionEvaluator);
            var result = strategy.Apply();
            
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }
    }

    public class GreaterOrEqualCheckInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error when the first operand is not a number")]
        public void throws_error_when_the_first_operand_is_not_a_number()
        {
            var greaterOrEqualCheck = new GreaterOrEqualCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsNumber().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(greaterOrEqualCheck, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }
        
        [Fact(DisplayName = "throws error when the second operand is not a number")]
        public void throws_error_when_the_second_operand_is_not_a_number()
        {
            var greaterOrEqualCheck = new GreaterOrEqualCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
        
            var strategy = new BinaryExpressionEvaluationStrategy(greaterOrEqualCheck, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }
        
        [Fact(DisplayName = "returns true when the operands are numbers and the first one is bigger")]
        public void returns_true_when_the_operands_are_numbers_and_the_first_one_is_bigger()
        {
            var greaterOrEqualCheck = new GreaterOrEqualCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 10.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 6.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(greaterOrEqualCheck, expressionEvaluator);
            var result = strategy.Apply();
            
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }
        
        [Fact(DisplayName = "returns true when the operands are numbers and equal")]
        public void returns_true_when_the_operands_are_numbers_and_equal()
        {
            var greaterOrEqualCheck = new GreaterOrEqualCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 10.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 10.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(greaterOrEqualCheck, expressionEvaluator);
            var result = strategy.Apply();
            
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }
        
        [Fact(DisplayName = "returns false when the operands are numbers and the first is smaller")]
        public void returns_false_when_the_operands_are_numbers_and_the_first_is_smaller()
        {
            var greaterOrEqualCheck = new GreaterOrEqualCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 3.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 10.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(greaterOrEqualCheck, expressionEvaluator);
            var result = strategy.Apply();
            
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }
    }

    public class LessCheckInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error when the first operand is not a number")]
        public void throws_error_when_the_first_operand_is_not_a_number()
        {
            var lessCheck = new LessCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsNumber().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(lessCheck, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }
        
        [Fact(DisplayName = "throws error when the second operand is not a number")]
        public void throws_error_when_the_second_operand_is_not_a_number()
        {
            var lessCheck = new LessCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
        
            var strategy = new BinaryExpressionEvaluationStrategy(lessCheck, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }
        
        [Fact(DisplayName = "returns false when the operands are numbers and the first one is bigger")]
        public void returns_false_when_the_operands_are_numbers_and_the_first_one_is_bigger()
        {
            var lessCheck = new LessCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 10.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 6.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(lessCheck, expressionEvaluator);
            var result = strategy.Apply();
            
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }
        
        [Fact(DisplayName = "returns false when the operands are numbers and equal")]
        public void returns_false_when_the_operands_are_numbers_and_equal()
        {
            var lessCheck = new LessCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 10.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 10.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(lessCheck, expressionEvaluator);
            var result = strategy.Apply();
            
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }
        
        [Fact(DisplayName = "returns true when the operands are numbers and the first is smaller")]
        public void returns_true_when_the_operands_are_numbers_and_the_first_is_smaller()
        {
            var lessCheck = new LessCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 3.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 10.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(lessCheck, expressionEvaluator);
            var result = strategy.Apply();
            
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }
    }

    public class LessOrEqualCheckInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error when the first operand is not a number")]
        public void throws_error_when_the_first_operand_is_not_a_number()
        {
            var lessOrEqualCheck = new LessOrEqualCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsNumber().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(lessOrEqualCheck, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }
        
        [Fact(DisplayName = "throws error when the second operand is not a number")]
        public void throws_error_when_the_second_operand_is_not_a_number()
        {
            var lessOrEqualCheck = new LessOrEqualCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
        
            var strategy = new BinaryExpressionEvaluationStrategy(lessOrEqualCheck, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }
        
        [Fact(DisplayName = "returns false when the operands are numbers and the first one is bigger")]
        public void returns_false_when_the_operands_are_numbers_and_the_first_one_is_bigger()
        {
            var lessOrEqualCheck = new LessOrEqualCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 10.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 6.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(lessOrEqualCheck, expressionEvaluator);
            var result = strategy.Apply();
            
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }
        
        [Fact(DisplayName = "returns true when the operands are numbers and equal")]
        public void returns_true_when_the_operands_are_numbers_and_equal()
        {
            var lessOrEqualCheck = new LessOrEqualCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 10.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 10.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(lessOrEqualCheck, expressionEvaluator);
            var result = strategy.Apply();
            
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }
        
        [Fact(DisplayName = "returns true when the operands are numbers and the first is smaller")]
        public void returns_true_when_the_operands_are_numbers_and_the_first_is_smaller()
        {
            var lessOrEqualCheck = new LessOrEqualCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 3.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 10.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(lessOrEqualCheck, expressionEvaluator);
            var result = strategy.Apply();
            
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }
    }

    public class EqualityCheckInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error if the operands are not of the same type")]
        public void throws_error_if_the_operands_are_not_of_the_same_type()
        {
            var equalityCheck = new EqualityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.Is(secondOperandDataType).Returns(false);

            var strategy = new BinaryExpressionEvaluationStrategy(equalityCheck, expressionEvaluator);
            Assert.Throws<IncompatibleTypesException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns true when both operands are number and equal")]
        public void returns_true_when_both_operands_are_number_and_equal()
        {
            var equalityCheck = new EqualityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 1.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 1.0 });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
            firstOperandDataType.IsNumber().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(equalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }

        [Fact(DisplayName = "returns false when both operands are number and not equal")]
        public void returns_false_when_both_operands_are_number_and_not_equal()
        {
            var equalityCheck = new EqualityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = -8.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = -3.0 });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
            firstOperandDataType.IsNumber().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(equalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }

        [Fact(DisplayName = "returns true when both operands are strings and equal")]
        public void returns_true_when_both_operands_are_string_and_equal()
        {
            var equalityCheck = new EqualityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = "hello" });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = "hello" });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
            firstOperandDataType.IsString().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(equalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }

        [Fact(DisplayName = "returns false when both operands are strings and not equal")]
        public void returns_false_when_both_operands_are_string_and_not_equal()
        {
            var equalityCheck = new EqualityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = "hello " });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = "hello" });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
            firstOperandDataType.IsString().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(equalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }

        [Fact(DisplayName = "returns true when both operands are bool and equal")]
        public void returns_true_when_both_operands_are_bool_and_equal()
        {
            var equalityCheck = new EqualityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = true });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = true });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
            firstOperandDataType.IsBool().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(equalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }

        [Fact(DisplayName = "returns false when both operands are bool and not equal")]
        public void returns_false_when_both_operands_are_bool_and_not_equal()
        {
            var equalityCheck = new EqualityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = true });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = false });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
            firstOperandDataType.IsBool().Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(equalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }

        [Fact(DisplayName = "returns false when both operands are of the same custom type")]
        public void returns_false_when_both_operands_are_of_the_same_custom_type()
        {
            // TODO revisit
            var equalityCheck = new EqualityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);

            var strategy = new BinaryExpressionEvaluationStrategy(equalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }
    }

    public class InequalityCheckInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression firstOperand = Substitute.For<Expression>();
        private readonly Expression secondOperand = Substitute.For<Expression>();
        private readonly DataType firstOperandDataType = Substitute.For<DataType>();
        private readonly DataType secondOperandDataType = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error if the operands are not of the same type")]
        public void throws_error_if_the_operands_are_not_of_the_same_type()
        {
            var inequalityCheck = new InequalityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.Is(secondOperandDataType).Returns(false);

            var strategy = new BinaryExpressionEvaluationStrategy(inequalityCheck, expressionEvaluator);
            Assert.Throws<IncompatibleTypesException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns false when both operands are number and equal")]
        public void returns_true_when_both_operands_are_number_and_equal()
        {
            var inequalityCheck = new InequalityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 1.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 1.0 });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
            firstOperandDataType.IsNumber().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(inequalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }
        
        [Fact(DisplayName = "returns true when both operands are number and not equal")]
        public void returns_false_when_both_operands_are_number_and_not_equal()
        {
            var inequalityCheck = new InequalityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = -8.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = -3.0 });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
            firstOperandDataType.IsNumber().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(inequalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }
        
        [Fact(DisplayName = "returns false when both operands are strings and equal")]
        public void returns_false_when_both_operands_are_string_and_equal()
        {
            var inequalityCheck = new InequalityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = "hello" });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = "hello" });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
            firstOperandDataType.IsString().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(inequalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }
        
        [Fact(DisplayName = "returns true when both operands are strings and not equal")]
        public void returns_true_when_both_operands_are_string_and_not_equal()
        {
            var inequalityCheck = new InequalityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = "hello " });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = "hello" });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
            firstOperandDataType.IsString().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(inequalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }
        
        [Fact(DisplayName = "returns false when both operands are bool and equal")]
        public void returns_false_when_both_operands_are_bool_and_equal()
        {
            var inequalityCheck = new InequalityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = true });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = true });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
            firstOperandDataType.IsBool().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(inequalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }
        
        [Fact(DisplayName = "returns true when both operands are bool and not equal")]
        public void returns_true_when_both_operands_are_bool_and_not_equal()
        {
            var inequalityCheck = new InequalityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = true });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = false });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
            firstOperandDataType.IsBool().Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(inequalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }
        
        [Fact(DisplayName = "returns true when both operands are of the same custom type")]
        public void returns_true_when_both_operands_are_of_the_same_custom_type()
        {
            // TODO revisit
            var inequalityCheck = new InequalityCheck { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = new object() });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = new object() });
            firstOperandDataType.Is(secondOperandDataType).Returns(true);
        
            var strategy = new BinaryExpressionEvaluationStrategy(inequalityCheck, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }
    }
}