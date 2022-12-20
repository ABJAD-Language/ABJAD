using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions.Strategies;

public class UnaryExpressionEvaluationStrategyTest
{
    private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
    private readonly Expression target = Substitute.For<Expression>();

    [Fact(DisplayName = "throws error if the value of target is undefined")]
    public void throws_error_if_the_value_of_target_is_undefined()
    {
        var negation = new Negation { Target = target };
        expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Value = SpecialValues.UNDEFINED });
        var strategy = new UnaryExpressionEvaluationStrategy(negation, expressionEvaluator);
        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }
    
    public class NegationInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression target = Substitute.For<Expression>();
        private readonly DataType type = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error if target is not a bool")]
        public void throws_error_if_target_is_not_a_bool()
        {
            var negation = new Negation { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = new object() });
            type.IsBool().Returns(false);
            var strategy = new UnaryExpressionEvaluationStrategy(negation, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns true when the target evaluates to false")]
        public void returns_true_when_the_target_evaluates_to_false()
        {
            var negation = new Negation { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = false });
            type.IsBool().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(negation, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }

        [Fact(DisplayName = "returns false when the target evaluates to true")]
        public void returns_false_when_the_target_evaluates_to_true()
        {
            var negation = new Negation { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = true });
            type.IsBool().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(negation, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }
    }

    public class NegativeInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression target = Substitute.For<Expression>();
        private readonly DataType type = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error when the target is not a number")]
        public void throws_error_when_the_target_is_not_a_number()
        {
            var negative = new Negative { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = new object() });
            type.IsNumber().Returns(false);
            var strategy = new UnaryExpressionEvaluationStrategy(negative, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns the positive value of the number when target is negative")]
        public void returns_the_positive_value_of_the_number_when_target_is_negative()
        {
            var negative = new Negative { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = -3.0});
            type.IsNumber().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(negative, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(3.0, result.Value);
        }

        [Fact(DisplayName = "returns the negative value of the number when target is positive")]
        public void returns_the_negative_value_of_the_number_when_target_is_positive()
        {
            var negative = new Negative { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = 11.0});
            type.IsNumber().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(negative, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(-11.0, result.Value);
        }
    }

    public class ToBoolInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression target = Substitute.For<Expression>();
        private readonly DataType type = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error if target is not a string")]
        public void throws_error_if_target_is_not_a_string()
        {
            var toBool = new ToBool { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = new object() });
            type.IsString().Returns(false);
            var strategy = new UnaryExpressionEvaluationStrategy(toBool, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if target value is null")]
        public void throws_error_if_target_value_is_null()
        {
            var toBool = new ToBool { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = SpecialValues.NULL });
            type.IsString().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(toBool, expressionEvaluator);
            Assert.Throws<NullPointerException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns true when target evaluates to the true string literal")]
        public void returns_true_when_target_evaluates_to_the_true_string_literal()
        {
            var toBool = new ToBool { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = "صحيح"});
            type.IsString().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(toBool, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(true, result.Value);
        }

        [Fact(DisplayName = "returns false when target evaluates to the false string literal")]
        public void returns_false_when_target_evaluates_to_the_false_string_literal()
        {
            var toBool = new ToBool { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = "خطا"});
            type.IsString().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(toBool, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsBool());
            Assert.Equal(false, result.Value);
        }

        [Fact(DisplayName = "throws error when the string value does not match the true nor the false literals")]
        public void throws_error_when_the_string_value_does_not_match_the_true_nor_the_false_literals()
        {
            var toBool = new ToBool { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = "صح"});
            type.IsString().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(toBool, expressionEvaluator);
            Assert.Throws<InvalidTypeCastException>(() => strategy.Apply());
        }
    }

    public class ToNumberInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression target = Substitute.For<Expression>();
        private readonly DataType type = Substitute.For<DataType>();

        [Fact(DisplayName = "throws error if the target is not a string")]
        public void throws_error_if_the_target_is_not_a_string()
        {
            var toNumber = new ToNumber { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = new object() });
            type.IsString().Returns(false);
            var strategy = new UnaryExpressionEvaluationStrategy(toNumber, expressionEvaluator);
            Assert.Throws<InvalidTypeException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "throws error if the value of target is null")]
        public void throws_error_if_the_value_of_target_is_null()
        {
            var toNumber = new ToNumber { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = SpecialValues.NULL });
            type.IsString().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(toNumber, expressionEvaluator);
            Assert.Throws<NullPointerException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns the number value of target when it is a string representation of a positive number")]
        public void returns_the_number_value_of_target_when_it_is_a_string_representation_of_a_positive_number()
        {
            var toNumber = new ToNumber { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = "90.0" });
            type.IsString().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(toNumber, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(90.0, result.Value);
        }

        [Fact(DisplayName = "returns the number value of target when it is a string representation of a number preceded by a plus")]
        public void returns_the_number_value_of_target_when_it_is_a_string_representation_of_a_number_preceded_by_a_plus()
        {
            var toNumber = new ToNumber { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = "+13.0" });
            type.IsString().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(toNumber, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(13.0, result.Value);
        }

        [Fact(DisplayName = "return the number value of target when it is a string representation of a negative number")]
        public void return_the_number_value_of_target_when_it_is_a_string_representation_of_a_negative_number()
        {
            var toNumber = new ToNumber { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = "-33.0" });
            type.IsString().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(toNumber, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(-33.0, result.Value);
        }

        [Fact(DisplayName = "throws an error when target cannot be parsed to number")]
        public void throws_an_error_when_target_cannot_be_parsed_to_number()
        {
            var toNumber = new ToNumber { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = "10.0.3" });
            type.IsString().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(toNumber, expressionEvaluator);
            Assert.Throws<InvalidTypeCastException>(() => strategy.Apply());
        }
    }

    public class ToStringInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression target = Substitute.For<Expression>();
        private readonly DataType type = Substitute.For<DataType>();

        [Fact(DisplayName = "throws null pointer error if target value is null")]
        public void throws_null_pointer_error_if_target_value_is_null()
        {
            var toString = new ToString { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Value = SpecialValues.NULL });
            var strategy = new UnaryExpressionEvaluationStrategy(toString, expressionEvaluator);
            Assert.Throws<NullPointerException>(() => strategy.Apply());
        }

        [Fact(DisplayName = "returns the string representation of target when it is a number")]
        public void returns_the_string_representation_of_target_when_it_is_a_number()
        {
            var toString = new ToString { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = -12.3 });
            type.IsNumber().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(toString, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsString());
            Assert.Equal("-12.3", result.Value);
        }

        [Fact(DisplayName = "returns the string representation of target when it is a bool and its value is false")]
        public void returns_the_string_representation_of_target_when_it_is_a_bool_and_its_value_is_false()
        {
            var toString = new ToString { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = false });
            type.IsBool().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(toString, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsString());
            Assert.Equal("خطا", result.Value);
        }

        [Fact(DisplayName = "returns the string representation of target when it is a bool and its value is true")]
        public void returns_the_string_representation_of_target_when_it_is_a_bool_and_its_value_is_true()
        {
            var toString = new ToString { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = type, Value = true });
            type.IsBool().Returns(true);
            var strategy = new UnaryExpressionEvaluationStrategy(toString, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsString());
            Assert.Equal("صحيح", result.Value);
        }
    }

    public class TypeOfInterpretationTest
    {
        private readonly Evaluator<Expression> expressionEvaluator = Substitute.For<Evaluator<Expression>>();
        private readonly Expression target = Substitute.For<Expression>();

        [Fact(DisplayName = "returns undefined when the type of target is unknown yet")]
        public void returns_undefined_when_the_type_of_target_is_unknown_yet()
        {
            var typeOf = new TypeOf { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = DataType.Undefined(), Value = new object() });
            var strategy = new UnaryExpressionEvaluationStrategy(typeOf, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsString());
            Assert.Equal("غير_معرف", result.Value);
        }

        [Fact(DisplayName = "returns number when the type of target is number")]
        public void returns_number_when_the_type_of_target_is_number()
        {
            var typeOf = new TypeOf { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = DataType.Number(), Value = new object() });
            var strategy = new UnaryExpressionEvaluationStrategy(typeOf, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsString());
            Assert.Equal("رقم", result.Value);
        }

        [Fact(DisplayName = "returns string when the type of target is string")]
        public void returns_string_when_the_type_of_target_is_string()
        {
            var typeOf = new TypeOf { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = DataType.String(), Value = new object() });
            var strategy = new UnaryExpressionEvaluationStrategy(typeOf, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsString());
            Assert.Equal("مقطع", result.Value);
        }

        [Fact(DisplayName = "returns bool when the type of target is bool")]
        public void returns_bool_when_the_type_of_target_is_bool()
        {
            var typeOf = new TypeOf { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = DataType.Bool(), Value = new object() });
            var strategy = new UnaryExpressionEvaluationStrategy(typeOf, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsString());
            Assert.Equal("منطق", result.Value);
        }

        [Fact(DisplayName = "returns the custom type name when the type of target is custom")]
        public void returns_the_custom_type_name_when_the_type_of_target_is_custom()
        {
            var typeOf = new TypeOf { Target = target };
            expressionEvaluator.Evaluate(target).Returns(new EvaluatedResult { Type = DataType.Custom("خاص"), Value = new object() });
            var strategy = new UnaryExpressionEvaluationStrategy(typeOf, expressionEvaluator);
            var result = strategy.Apply();
            Assert.True(result.Type.IsString());
            Assert.Equal("خاص", result.Value);
        }
    }
}