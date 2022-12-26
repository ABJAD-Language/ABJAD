using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Expressions.Fixes;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions.Strategies;

public class FixesEvaluationStrategyTest
{
    private readonly ScopeFacade scopeFacade = Substitute.For<ScopeFacade>();

    [Fact(DisplayName = "throws error if target did not exist in scope")]
    public void throws_error_if_target_did_not_exist_in_scope()
    {
        scopeFacade.ReferenceExists("id").Returns(false);
        var additionPostfix = new AdditionPostfix { Target = "id" };
        var strategy = new FixesEvaluationStrategy(additionPostfix, scopeFacade);
        Assert.Throws<ReferenceNameDoesNotExistException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if type of target was not number")]
    public void throws_error_if_type_of_target_was_not_number()
    {
        scopeFacade.ReferenceExists("id").Returns(true);
        var targetType = Substitute.For<DataType>();
        targetType.IsNumber().Returns(false);
        scopeFacade.GetReferenceType("id").Returns(targetType);
        var additionPostfix = new AdditionPostfix { Target = "id" };
        var strategy = new FixesEvaluationStrategy(additionPostfix, scopeFacade);
        Assert.Throws<InvalidTypeException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "throws error if value of target is still undefined")]
    public void throws_error_if_value_of_target_is_still_undefined()
    {
        var targetType = Substitute.For<DataType>();
        targetType.IsNumber().Returns(true);
        scopeFacade.ReferenceExists("id").Returns(true);
        scopeFacade.GetReferenceType("id").Returns(targetType);
        scopeFacade.GetReference("id").Returns(SpecialValues.UNDEFINED);
        var additionPostfix = new AdditionPostfix { Target = "id" };
        var strategy = new FixesEvaluationStrategy(additionPostfix, scopeFacade);
        Assert.Throws<OperationOnUndefinedValueException>(() => strategy.Apply());
    }

    public class AdditionPostfixInterpretationTest
    {
        private readonly ScopeFacade scopeFacade = Substitute.For<ScopeFacade>();
        private readonly DataType targetType = Substitute.For<DataType>();

        [Fact(DisplayName = "updates scope with the incremented value")]
        public void updates_scope_with_the_incremented_value()
        {
            targetType.IsNumber().Returns(true);
            scopeFacade.ReferenceExists("id").Returns(true);
            scopeFacade.GetReferenceType("id").Returns(targetType);
            scopeFacade.GetReference("id").Returns(3.2);

            var additionPostfix = new AdditionPostfix { Target = "id" };
            var strategy = new FixesEvaluationStrategy(additionPostfix, scopeFacade);
            strategy.Apply();
            scopeFacade.Received(1).UpdateReference("id", 4.2);
        }

        [Fact(DisplayName = "returns the old value of target")]
        public void returns_the_old_value_of_target()
        {
            targetType.IsNumber().Returns(true);
            scopeFacade.ReferenceExists("id").Returns(true);
            scopeFacade.GetReferenceType("id").Returns(targetType);
            scopeFacade.GetReference("id").Returns(3.2);

            var additionPostfix = new AdditionPostfix { Target = "id" };
            var strategy = new FixesEvaluationStrategy(additionPostfix, scopeFacade);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(3.2, result.Value);
        }
    }

    public class AdditionPrefixInterpretationTest
    {
        private readonly ScopeFacade scopeFacade = Substitute.For<ScopeFacade>();
        private readonly DataType targetType = Substitute.For<DataType>();

        [Fact(DisplayName = "updates scope with the incremented value")]
        public void updates_scope_with_the_incremented_value()
        {
            targetType.IsNumber().Returns(true);
            scopeFacade.ReferenceExists("id").Returns(true);
            scopeFacade.GetReferenceType("id").Returns(targetType);
            scopeFacade.GetReference("id").Returns(10.0);

            var additionPrefix = new AdditionPrefix { Target = "id" };
            var strategy = new FixesEvaluationStrategy(additionPrefix, scopeFacade);
            strategy.Apply();
            scopeFacade.Received(1).UpdateReference("id", 11.0);
        }

        [Fact(DisplayName = "return the incremented value")]
        public void return_the_incremented_value()
        {
            targetType.IsNumber().Returns(true);
            scopeFacade.ReferenceExists("id").Returns(true);
            scopeFacade.GetReferenceType("id").Returns(targetType);
            scopeFacade.GetReference("id").Returns(10.0);

            var additionPrefix = new AdditionPrefix { Target = "id" };
            var strategy = new FixesEvaluationStrategy(additionPrefix, scopeFacade);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(11.0, result.Value);
        }
    }

    public class SubtractionPostfixInterpretationTest
    {
        private readonly ScopeFacade scopeFacade = Substitute.For<ScopeFacade>();
        private readonly DataType targetType = Substitute.For<DataType>();

        [Fact(DisplayName = "updates scope with the incremented value")]
        public void updates_scope_with_the_incremented_value()
        {
            targetType.IsNumber().Returns(true);
            scopeFacade.ReferenceExists("id").Returns(true);
            scopeFacade.GetReferenceType("id").Returns(targetType);
            scopeFacade.GetReference("id").Returns(13.0);

            var subtractionPostfix = new SubtractionPostfix { Target = "id" };
            var strategy = new FixesEvaluationStrategy(subtractionPostfix, scopeFacade);
            strategy.Apply();
            scopeFacade.Received(1).UpdateReference("id", 12.0);
        }

        [Fact(DisplayName = "returns the old value of target")]
        public void returns_the_old_value_of_target()
        {
            targetType.IsNumber().Returns(true);
            scopeFacade.ReferenceExists("id").Returns(true);
            scopeFacade.GetReferenceType("id").Returns(targetType);
            scopeFacade.GetReference("id").Returns(13.0);
        
            var subtractionPostfix = new SubtractionPostfix { Target = "id" };
            var strategy = new FixesEvaluationStrategy(subtractionPostfix, scopeFacade);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(13.0, result.Value);
        }
    }

    public class SubtractionPrefixInterpretationTest
    {
        private readonly ScopeFacade scopeFacade = Substitute.For<ScopeFacade>();
        private readonly DataType targetType = Substitute.For<DataType>();

        [Fact(DisplayName = "updates scope with the incremented value")]
        public void updates_scope_with_the_incremented_value()
        {
            targetType.IsNumber().Returns(true);
            scopeFacade.ReferenceExists("id").Returns(true);
            scopeFacade.GetReferenceType("id").Returns(targetType);
            scopeFacade.GetReference("id").Returns(15.0);

            var subtractionPrefix = new SubtractionPrefix { Target = "id" };
            var strategy = new FixesEvaluationStrategy(subtractionPrefix, scopeFacade);
            strategy.Apply();
            scopeFacade.Received(1).UpdateReference("id", 14.0);
        }

        [Fact(DisplayName = "return the incremented value")]
        public void return_the_incremented_value()
        {
            targetType.IsNumber().Returns(true);
            scopeFacade.ReferenceExists("id").Returns(true);
            scopeFacade.GetReferenceType("id").Returns(targetType);
            scopeFacade.GetReference("id").Returns(15.0);
        
            var subtractionPrefix = new SubtractionPrefix { Target = "id" };
            var strategy = new FixesEvaluationStrategy(subtractionPrefix, scopeFacade);
            var result = strategy.Apply();
            Assert.True(result.Type.IsNumber());
            Assert.Equal(14.0, result.Value);
        }
    }
}