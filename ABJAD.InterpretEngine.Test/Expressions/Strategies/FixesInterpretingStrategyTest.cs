using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions.Strategies;

public class FixesInterpretingStrategyTest
{
    private readonly IScope scope = Substitute.For<IScope>();

    [Fact(DisplayName = "throws error if target did not exist in scope")]
    public void throws_error_if_target_did_not_exist_in_scope()
    {
        var target = new IdentifierPrimitive { Value = "id" };
        scope.ReferenceExists("id").Returns(false);
        var additionPostfix = new AdditionPostfix { Target = target };
        var strategy = new FixesInterpretingStrategy(additionPostfix, scope);
        Assert.Throws<ReferenceNameDoesNotExistException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if type of target was not number")]
    public void throws_error_if_type_of_target_was_not_number()
    {
        var target = new IdentifierPrimitive { Value = "id" };
        scope.ReferenceExists("id").Returns(true);
        var targetType = Substitute.For<DataType>();
        targetType.IsNumber().Returns(false);
        scope.GetType("id").Returns(targetType);
        var additionPostfix = new AdditionPostfix { Target = target };
        var strategy = new FixesInterpretingStrategy(additionPostfix, scope);
        Assert.Throws<InvalidTypeException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if value of target is still undefined")]
    public void throws_error_if_value_of_target_is_still_undefined()
    {
        var target = new IdentifierPrimitive { Value = "id" };
        var targetType = Substitute.For<DataType>();
        targetType.IsNumber().Returns(true);
        scope.ReferenceExists("id").Returns(true);
        scope.GetType("id").Returns(targetType);
        scope.Get("id").Returns(SpecialValues.UNDEFINED);
        var additionPostfix = new AdditionPostfix { Target = target };
        var strategy = new FixesInterpretingStrategy(additionPostfix, scope);
        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }

    public class AdditionPostfixInterpretationTest
    {
        private readonly IScope scope = Substitute.For<IScope>();
        private readonly IdentifierPrimitive target = new IdentifierPrimitive { Value = "id" };
        private readonly DataType targetType = Substitute.For<DataType>();

        [Fact(DisplayName = "updates scope with the incremented value")]
        public void updates_scope_with_the_incremented_value()
        {
            targetType.IsNumber().Returns(true);
            scope.ReferenceExists("id").Returns(true);
            scope.GetType("id").Returns(targetType);
            scope.Get("id").Returns(3.2);

            var additionPostfix = new AdditionPostfix { Target = target };
            var strategy = new FixesInterpretingStrategy(additionPostfix, scope);
            strategy.Apply();
            scope.Received(1).Set("id", 4.2);
        }

        [Fact(DisplayName = "returns the old value of target")]
        public void returns_the_old_value_of_target()
        {
            targetType.IsNumber().Returns(true);
            scope.ReferenceExists("id").Returns(true);
            scope.GetType("id").Returns(targetType);
            scope.Get("id").Returns(3.2);

            var additionPostfix = new AdditionPostfix { Target = target };
            var strategy = new FixesInterpretingStrategy(additionPostfix, scope);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(3.2, result.Value);
        }
    }

    public class AdditionPrefixInterpretationTest
    {
        private readonly IScope scope = Substitute.For<IScope>();
        private readonly IdentifierPrimitive target = new IdentifierPrimitive { Value = "id" };
        private readonly DataType targetType = Substitute.For<DataType>();

        [Fact(DisplayName = "updates scope with the incremented value")]
        public void updates_scope_with_the_incremented_value()
        {
            targetType.IsNumber().Returns(true);
            scope.ReferenceExists("id").Returns(true);
            scope.GetType("id").Returns(targetType);
            scope.Get("id").Returns(10.0);

            var additionPrefix = new AdditionPrefix { Target = target };
            var strategy = new FixesInterpretingStrategy(additionPrefix, scope);
            strategy.Apply();
            scope.Received(1).Set("id", 11.0);
        }

        [Fact(DisplayName = "return the incremented value")]
        public void return_the_incremented_value()
        {
            targetType.IsNumber().Returns(true);
            scope.ReferenceExists("id").Returns(true);
            scope.GetType("id").Returns(targetType);
            scope.Get("id").Returns(10.0);

            var additionPrefix = new AdditionPrefix { Target = target };
            var strategy = new FixesInterpretingStrategy(additionPrefix, scope);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(11.0, result.Value);
        }
    }

    public class SubtractionPostfixInterpretationTest
    {
        private readonly IScope scope = Substitute.For<IScope>();
        private readonly IdentifierPrimitive target = new IdentifierPrimitive { Value = "id" };
        private readonly DataType targetType = Substitute.For<DataType>();

        [Fact(DisplayName = "updates scope with the incremented value")]
        public void updates_scope_with_the_incremented_value()
        {
            targetType.IsNumber().Returns(true);
            scope.ReferenceExists("id").Returns(true);
            scope.GetType("id").Returns(targetType);
            scope.Get("id").Returns(13.0);

            var subtractionPostfix = new SubtractionPostfix { Target = target };
            var strategy = new FixesInterpretingStrategy(subtractionPostfix, scope);
            strategy.Apply();
            scope.Received(1).Set("id", 12.0);
        }

        [Fact(DisplayName = "returns the old value of target")]
        public void returns_the_old_value_of_target()
        {
            targetType.IsNumber().Returns(true);
            scope.ReferenceExists("id").Returns(true);
            scope.GetType("id").Returns(targetType);
            scope.Get("id").Returns(13.0);
        
            var subtractionPostfix = new SubtractionPostfix { Target = target };
            var strategy = new FixesInterpretingStrategy(subtractionPostfix, scope);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(13.0, result.Value);
        }
    }

    public class SubtractionPrefixInterpretationTest
    {
        private readonly IScope scope = Substitute.For<IScope>();
        private readonly IdentifierPrimitive target = new IdentifierPrimitive { Value = "id" };
        private readonly DataType targetType = Substitute.For<DataType>();

        [Fact(DisplayName = "updates scope with the incremented value")]
        public void updates_scope_with_the_incremented_value()
        {
            targetType.IsNumber().Returns(true);
            scope.ReferenceExists("id").Returns(true);
            scope.GetType("id").Returns(targetType);
            scope.Get("id").Returns(15.0);

            var subtractionPrefix = new SubtractionPrefix { Target = target };
            var strategy = new FixesInterpretingStrategy(subtractionPrefix, scope);
            strategy.Apply();
            scope.Received(1).Set("id", 14.0);
        }

        [Fact(DisplayName = "return the incremented value")]
        public void return_the_incremented_value()
        {
            targetType.IsNumber().Returns(true);
            scope.ReferenceExists("id").Returns(true);
            scope.GetType("id").Returns(targetType);
            scope.Get("id").Returns(15.0);
        
            var subtractionPrefix = new SubtractionPrefix { Target = target };
            var strategy = new FixesInterpretingStrategy(subtractionPrefix, scope);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(14.0, result.Value);
        }
    }
}