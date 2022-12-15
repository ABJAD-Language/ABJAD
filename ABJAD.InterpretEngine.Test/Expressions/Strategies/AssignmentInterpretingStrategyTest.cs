using ABJAD.InterpretEngine.Expressions;
using ABJAD.InterpretEngine.Expressions.Strategies;
using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Assignments;
using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Expressions.Strategies;

public class AssignmentInterpretingStrategyTest
{
    private readonly IScope scope;
    private readonly Evaluater<Expression> expressionEvaluater;

    public AssignmentInterpretingStrategyTest()
    {
        scope = Substitute.For<IScope>();
        expressionEvaluater = Substitute.For<Evaluater<Expression>>();
    }

    [Fact(DisplayName = "throws error if the target reference did not exist")]
    public void throws_error_if_the_target_reference_did_not_exist()
    {
        scope.ReferenceExists("id").Returns(false);
        var strategy = new AssignmentInterpretingStrategy(new AdditionAssignment { Target = "id" }, scope, expressionEvaluater);
        Assert.Throws<ReferenceNameDoesNotExistException>(() => strategy.Apply());
    }

    public class AdditionAssignmentInterpretingTest
    {
        private readonly IScope scope;
        private readonly Evaluater<Expression> expressionEvaluater;

        public AdditionAssignmentInterpretingTest()
        {
            scope = Substitute.For<IScope>();
            expressionEvaluater = Substitute.For<Evaluater<Expression>>();
        }
        
        [Fact(DisplayName = "updates the value of target correctly and return the result")]
        public void updates_the_value_of_target_correctly_and_return_the_result()
        {
            scope.ReferenceExists("id").Returns(true);
            scope.Get("id").Returns(2.0);
            
            var offset = new NumberPrimitive { Value = 3.0 };

            expressionEvaluater.Evaluate(offset).Returns(3.0);

            var assignmentExpression = new AdditionAssignment { Target = "id", Value = offset };
            var strategy = new AssignmentInterpretingStrategy(assignmentExpression, scope, expressionEvaluater);
            var result = strategy.Apply();

            scope.Received(1).Set("id", 5.0);
            
            Assert.Equal(5.0, result);
        }
    }
}