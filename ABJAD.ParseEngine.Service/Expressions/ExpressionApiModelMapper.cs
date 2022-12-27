using ABJAD.ParseEngine.Expressions;
using ABJAD.ParseEngine.Expressions.Assignments;
using ABJAD.ParseEngine.Expressions.Binary;
using ABJAD.ParseEngine.Expressions.Unary;
using ABJAD.ParseEngine.Service.Expressions.Assignments;
using ABJAD.ParseEngine.Service.Expressions.Binary;
using ABJAD.ParseEngine.Service.Expressions.Unary;
using ABJAD.ParseEngine.Service.Primitives;

namespace ABJAD.ParseEngine.Service.Expressions;

public static class ExpressionApiModelMapper
{
    public static ExpressionApiModel Map(Expression expression)
    {
        return expression switch
        {
            InstanceMethodCallExpression instanceMethodCallExpression => Map(instanceMethodCallExpression),
            CallExpression callExpression => Map(callExpression),
            InstanceFieldExpression instanceFieldExpression => Map(instanceFieldExpression),
            InstantiationExpression instantiationExpression => Map(instantiationExpression),
            PrimitiveExpression primitiveExpression => PrimitiveApiModelMapper.Map(primitiveExpression.Primitive),
            AdditionAssignmentExpression additionAssignmentExpression => Map(additionAssignmentExpression),
            SubtractionAssignmentExpression subtractionAssignmentExpression => Map(subtractionAssignmentExpression),
            MultiplicationAssignmentExpression multiplicationAssignmentExpression => Map(multiplicationAssignmentExpression),
            DivisionAssignmentExpression divisionAssignmentExpression => Map(divisionAssignmentExpression),
            NegationExpression negationExpression => Map(negationExpression),
            NegativeExpression negativeExpression => Map(negativeExpression),
            PostfixAdditionExpression postfixAdditionExpression => Map(postfixAdditionExpression),
            PostfixSubtractionExpression postfixSubtractionExpression => Map(postfixSubtractionExpression),
            PrefixAdditionExpression prefixAdditionExpression => Map(prefixAdditionExpression),
            PrefixSubtractionExpression prefixSubtractionExpression => Map(prefixSubtractionExpression),
            ToBoolExpression toBoolExpression => Map(toBoolExpression),
            ToNumberExpression toNumberExpression => Map(toNumberExpression),
            ToStringExpression toStringExpression => Map(toStringExpression),
            TypeOfExpression typeOfExpression => Map(typeOfExpression),
            AdditionExpression additionExpression => Map(additionExpression),
            SubtractionExpression subtractionExpression => Map(subtractionExpression),
            MultiplicationExpression multiplicationExpression => Map(multiplicationExpression),
            DivisionExpression divisionExpression => Map(divisionExpression),
            ModuloExpression moduloExpression => Map(moduloExpression),
            AndOperationExpression andOperationExpression => Map(andOperationExpression),
            OrOperationExpression orOperationExpression => Map(orOperationExpression),
            EqualityCheckExpression equalityCheckExpression => Map(equalityCheckExpression),
            InequalityCheckExpression inequalityCheckExpression => Map(inequalityCheckExpression),
            GreaterCheckExpression greaterCheckExpression => Map(greaterCheckExpression),
            GreaterOrEqualCheckExpression greaterOrEqualCheckExpression => Map(greaterOrEqualCheckExpression),
            LessCheckExpression lessCheckExpression => Map(lessCheckExpression),
            LessOrEqualCheckExpression lessOrEqualCheckExpression => Map(lessOrEqualCheckExpression),
            GroupExpression groupExpression => Map(groupExpression.Target)
        };
    }

    private static ModuloExpressionApiModel Map(ModuloExpression moduloExpression)
    {
        return new ModuloExpressionApiModel(Map(moduloExpression.FirstOperand), Map(moduloExpression.SecondOperand));
    }

    private static LessOrEqualCheckExpressionApiModel Map(LessOrEqualCheckExpression lessOrEqualCheckExpression)
    {
        return new LessOrEqualCheckExpressionApiModel(Map(lessOrEqualCheckExpression.FirstOperand), Map(lessOrEqualCheckExpression.SecondOperand));
    }

    private static LessCheckExpressionApiModel Map(LessCheckExpression lessCheckExpression)
    {
        return new LessCheckExpressionApiModel(Map(lessCheckExpression.FirstOperand), Map(lessCheckExpression.SecondOperand));
    }

    private static GreaterOrEqualCheckExpressionApiModel Map(GreaterOrEqualCheckExpression greaterOrEqualCheckExpression)
    {
        return new GreaterOrEqualCheckExpressionApiModel(Map(greaterOrEqualCheckExpression.FirstOperand), Map(greaterOrEqualCheckExpression.SecondOperand));
    }

    private static GreaterCheckExpressionApiModel Map(GreaterCheckExpression greaterCheckExpression)
    {
        return new GreaterCheckExpressionApiModel(Map(greaterCheckExpression.FirstOperand), Map(greaterCheckExpression.SecondOperand));
    }

    private static InequalityCheckExpressionApiModel Map(InequalityCheckExpression inequalityCheckExpression)
    {
        return new InequalityCheckExpressionApiModel(Map(inequalityCheckExpression.FirstOperand), Map(inequalityCheckExpression.SecondOperand));
    }

    private static EqualityCheckExpressionApiModel Map(EqualityCheckExpression equalityCheckExpression)
    {
        return new EqualityCheckExpressionApiModel(Map(equalityCheckExpression.FirstOperand), Map(equalityCheckExpression.SecondOperand));
    }

    private static OrOperationExpressionApiModel Map(OrOperationExpression orOperationExpression)
    {
        return new OrOperationExpressionApiModel(Map(orOperationExpression.FirstOperand), Map(orOperationExpression.SecondOperand));
    }

    private static AndOperationExpressionApiModel Map(AndOperationExpression andOperationExpression)
    {
        return new AndOperationExpressionApiModel(Map(andOperationExpression.FirstOperand), Map(andOperationExpression.SecondOperand));
    }

    private static DivisionExpressionApiModel Map(DivisionExpression divisionExpression)
    {
        return new DivisionExpressionApiModel(Map(divisionExpression.FirstOperand), Map(divisionExpression.SecondOperand));
    }

    private static MultiplicationExpressionApiModel Map(MultiplicationExpression multiplicationExpression)
    {
        return new MultiplicationExpressionApiModel(Map(multiplicationExpression.FirstOperand), Map(multiplicationExpression.SecondOperand));
    }

    private static SubtractionExpressionApiModel Map(SubtractionExpression subtractionExpression)
    {
        return new SubtractionExpressionApiModel(Map(subtractionExpression.FirstOperand), Map(subtractionExpression.SecondOperand));
    }

    private static AdditionExpressionApiModel Map(AdditionExpression additionExpression)
    {
        return new AdditionExpressionApiModel(Map(additionExpression.FirstOperand), Map(additionExpression.SecondOperand));
    }

    private static TypeOfExpressionApiModel Map(TypeOfExpression typeOfExpression)
    {
        return new TypeOfExpressionApiModel(Map(typeOfExpression.Target));
    }

    private static ToStringExpressionApiModel Map(ToStringExpression toStringExpression)
    {
        return new ToStringExpressionApiModel(Map(toStringExpression.Target));
    }

    private static ToNumberExpressionApiModel Map(ToNumberExpression toNumberExpression)
    {
        return new ToNumberExpressionApiModel(Map(toNumberExpression.Target));
    }

    private static ToBoolExpressionApiModel Map(ToBoolExpression toBoolExpression)
    {
        return new ToBoolExpressionApiModel(Map(toBoolExpression.Target));
    }

    private static PrefixSubtractionExpressionApiModel Map(PrefixSubtractionExpression prefixSubtractionExpression)
    {
        return new PrefixSubtractionExpressionApiModel(Map(prefixSubtractionExpression.Target));
    }

    private static PrefixAdditionExpressionApiModel Map(PrefixAdditionExpression prefixAdditionExpression)
    {
        return new PrefixAdditionExpressionApiModel(Map(prefixAdditionExpression.Target));
    }

    private static PostfixSubtractionExpressionApiModel Map(PostfixSubtractionExpression postfixSubtractionExpression)
    {
        return new PostfixSubtractionExpressionApiModel(Map(postfixSubtractionExpression.Target));
    }

    private static PostfixAdditionExpressionApiModel Map(PostfixAdditionExpression postfixAdditionExpression)
    {
        return new PostfixAdditionExpressionApiModel(Map(postfixAdditionExpression.Target));
    }

    private static NegativeExpressionApiModel Map(NegativeExpression negativeExpression)
    {
        return new NegativeExpressionApiModel(Map(negativeExpression.Target));
    }

    private static NegationExpressionApiModel Map(NegationExpression negationExpression)
    {
        return new NegationExpressionApiModel(Map(negationExpression.Target));
    }

    private static DivisionAssignmentExpressionApiModel Map(DivisionAssignmentExpression divisionAssignmentExpression)
    {
        return new DivisionAssignmentExpressionApiModel(divisionAssignmentExpression.Target.Value, Map(divisionAssignmentExpression.Value));
    }

    private static MultiplicationAssignmentExpressionApiModel Map(MultiplicationAssignmentExpression multiplicationAssignmentExpression)
    {
        return new MultiplicationAssignmentExpressionApiModel(multiplicationAssignmentExpression.Target.Value, Map(multiplicationAssignmentExpression.Value));
    }

    private static SubtractionAssignmentExpressionApiModel Map(SubtractionAssignmentExpression subtractionAssignmentExpression)
    {
        return new SubtractionAssignmentExpressionApiModel(subtractionAssignmentExpression.Target.Value, Map(subtractionAssignmentExpression.Value));
    }

    private static AdditionAssignmentExpressionApiModel Map(AdditionAssignmentExpression additionAssignmentExpression)
    {
        return new AdditionAssignmentExpressionApiModel(additionAssignmentExpression.Target.Value, Map(additionAssignmentExpression.Value));
    }

    private static InstantiationExpressionApiModel Map(InstantiationExpression instantiationExpression)
    {
        return new InstantiationExpressionApiModel(PrimitiveApiModelMapper.Map(instantiationExpression.Class.Primitive), instantiationExpression.Arguments.Select(Map).ToList());
    }

    private static InstanceMethodCallExpressionApiModel Map(InstanceMethodCallExpression instanceMethodCallExpression)
    {
        return new InstanceMethodCallExpressionApiModel(instanceMethodCallExpression.Instances.Select(primitiveExpression => PrimitiveApiModelMapper.Map(primitiveExpression.Primitive)).ToList(),
            PrimitiveApiModelMapper.Map(instanceMethodCallExpression.Method.Primitive), instanceMethodCallExpression.Arguments.Select(Map).ToList());
    }

    private static InstanceFieldExpressionApiModel Map(InstanceFieldExpression instanceFieldExpression)
    {
        return new InstanceFieldExpressionApiModel(PrimitiveApiModelMapper.Map(instanceFieldExpression.Instance.Primitive), instanceFieldExpression.Fields.Select(primitiveExpression => PrimitiveApiModelMapper.Map(primitiveExpression.Primitive)).ToList());
    }

    private static CallExpressionApiModel Map(CallExpression callExpression)
    {
        return new CallExpressionApiModel(PrimitiveApiModelMapper.Map(callExpression.Method.Primitive), callExpression.Arguments.Select(Map).ToList());
    }
}