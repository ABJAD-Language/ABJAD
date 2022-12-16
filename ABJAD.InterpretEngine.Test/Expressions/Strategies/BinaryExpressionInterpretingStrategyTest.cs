using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions.Strategies;

public class BinaryExpressionInterpretingStrategyTest
{

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
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType });

            firstOperandDataType.IsNumber().Returns(false);
            firstOperandDataType.IsString().Returns(false);
            secondOperandDataType.IsString().Returns(true);
            
            var binaryExpression = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };

            var strategy = new BinaryExpressionInterpretingStrategy(binaryExpression, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the second operand is neither a number nor a string")]
        public void throws_error_if_the_second_operand_is_neither_a_number_nor_a_string()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType });

            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsString().Returns(false);

            var binaryExpression = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };

            var strategy = new BinaryExpressionInterpretingStrategy(binaryExpression, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns the sum of the two numbers when both operands are numbers")]
        public void returns_the_sum_of_the_two_numbers_when_both_operands_are_numbers()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 3.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 2.0 });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);
            
            var binaryExpression = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };

            var strategy = new BinaryExpressionInterpretingStrategy(binaryExpression, expressionEvaluator);
            Assert.Equal(5.0, strategy.Apply());
        }

        [Fact(DisplayName = "returns the concatenation of the two strings when both operands are strings")]
        public void returns_the_concatenation_of_the_two_strings_when_both_operands_are_strings()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = "hello " });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = "world" });
            firstOperandDataType.IsString().Returns(true);
            secondOperandDataType.IsString().Returns(true);
            
            var binaryExpression = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };

            var strategy = new BinaryExpressionInterpretingStrategy(binaryExpression, expressionEvaluator);
            Assert.Equal("hello world", strategy.Apply());
        }

        [Fact(DisplayName = "returns the string concatenation of the operands when the first one is number and the second is string")]
        public void returns_the_string_concatenation_of_the_operands_when_the_first_one_is_number_and_the_second_is_string()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = 1.0 });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = " is the answer" });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsString().Returns(true);

            var binaryExpression = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };

            var strategy = new BinaryExpressionInterpretingStrategy(binaryExpression, expressionEvaluator);
            Assert.Equal("1 is the answer", strategy.Apply());
        }

        [Fact(DisplayName = "returns the string concatenation of the operands when the first one is string and the second is number")]
        public void returns_the_string_concatenation_of_the_operands_when_the_first_one_is_string_and_the_second_is_number()
        {
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType, Value = "the answer is " });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType, Value = 7.0 });
            firstOperandDataType.IsString().Returns(true);
            secondOperandDataType.IsNumber().Returns(true);

            var binaryExpression = new Addition { FirstOperand = firstOperand, SecondOperand = secondOperand };

            var strategy = new BinaryExpressionInterpretingStrategy(binaryExpression, expressionEvaluator);
            Assert.Equal("the answer is 7", strategy.Apply());
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
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType });
            firstOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionInterpretingStrategy(subtraction, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the second operand is not a number")]
        public void throws_error_if_the_second_operand_is_not_a_number()
        {
            var subtraction = new Subtraction { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
            var strategy = new BinaryExpressionInterpretingStrategy(subtraction, expressionEvaluator);
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
            var strategy = new BinaryExpressionInterpretingStrategy(subtraction, expressionEvaluator);
            Assert.Equal(-5.0, strategy.Apply());
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
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType });
            firstOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionInterpretingStrategy(multiplication, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the second operand is not a number")]
        public void throws_error_if_the_second_operand_is_not_a_number()
        {
            var multiplication = new Multiplication { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
            var strategy = new BinaryExpressionInterpretingStrategy(multiplication, expressionEvaluator);
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
            var strategy = new BinaryExpressionInterpretingStrategy(multiplication, expressionEvaluator);
            Assert.Equal(-8.0, strategy.Apply());
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
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType });
            firstOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionInterpretingStrategy(division, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the second operand is not a number")]
        public void throws_error_if_the_second_operand_is_not_a_number()
        {
            var division = new Division { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
            var strategy = new BinaryExpressionInterpretingStrategy(division, expressionEvaluator);
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
            var strategy = new BinaryExpressionInterpretingStrategy(division, expressionEvaluator);
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
            var strategy = new BinaryExpressionInterpretingStrategy(division, expressionEvaluator);
            Assert.Equal(4.0, strategy.Apply());
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
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType });
            firstOperandDataType.IsNumber().Returns(false);
            secondOperandDataType.IsNumber().Returns(true);
            var strategy = new BinaryExpressionInterpretingStrategy(modulo, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the second operand is not a number")]
        public void throws_error_if_the_second_operand_is_not_a_number()
        {
            var modulo = new Modulo { FirstOperand = firstOperand, SecondOperand = secondOperand };
            expressionEvaluator.Evaluate(firstOperand).Returns(new EvaluatedResult { Type = firstOperandDataType });
            expressionEvaluator.Evaluate(secondOperand).Returns(new EvaluatedResult { Type = secondOperandDataType });
            firstOperandDataType.IsNumber().Returns(true);
            secondOperandDataType.IsNumber().Returns(false);
            var strategy = new BinaryExpressionInterpretingStrategy(modulo, expressionEvaluator);
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
            var strategy = new BinaryExpressionInterpretingStrategy(modulo, expressionEvaluator);
            Assert.Equal(1.0, strategy.Apply());
        }
    }
}