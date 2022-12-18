using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Unary;
using ABJAD.InterpretEngine.Types;
using _ToString =  ABJAD.InterpretEngine.Shared.Expressions.Unary.ToString;

namespace ABJAD.InterpretEngine.Expressions.Strategies;

public class UnaryExpressionInterpretingStrategy : ExpressionInterpretingStrategy
{
    private readonly UnaryExpression unaryExpression;
    private readonly Evaluator<Expression> expressionEvaluator;

    public UnaryExpressionInterpretingStrategy(UnaryExpression unaryExpression, Evaluator<Expression> expressionEvaluator)
    {
        this.unaryExpression = unaryExpression;
        this.expressionEvaluator = expressionEvaluator;
    }

    public EvaluatedResult Apply()
    {
        var target = expressionEvaluator.Evaluate(unaryExpression.Target);

        ValidateValueNotUndefined(target);

        return unaryExpression switch
        {
            Negation => HandleNegation(target),
            Negative => HandleNegative(target),
            ToBool => HandleToBool(target),
            ToNumber => HandleToNumber(target),
            _ToString => HandleToString(target),
            TypeOf => HandleTypeOf(target),
            _ => throw new ArgumentException()
        };
    }
    
    private static EvaluatedResult HandleTypeOf(EvaluatedResult target)
    {
        return new EvaluatedResult { Type = DataType.String(), Value = target.Type.GetValue() };
    }

    private static void ValidateValueNotUndefined(EvaluatedResult target)
    {
        if (target.Value.Equals(SpecialValues.UNDEFINED))
        {
            throw new OperationOnUndefinedValueException();
        }
    }

    private static EvaluatedResult HandleToString(EvaluatedResult target)
    {
        if (target.Value == null || target.Value.Equals(SpecialValues.NULL))
        {
            throw new NullPointerException();
        }

        string value;
        if (target.Type.IsNumber())
        {
            value = target.Value.ToString();
        }
        else if (target.Type.IsBool())
        {
            value = (bool)target.Value ? "صحيح" : "خطا";
        }
        else if (target.Type.IsUndefined())
        {
            throw new ArgumentException();
        }
        else
        {
            value = "$";
            // TODO
        }

        return new EvaluatedResult { Type = DataType.String(), Value = value };
    }

    private static EvaluatedResult HandleToNumber(EvaluatedResult target)
    {
        ValidateTargetIsString(target);
        ValidateTargetIsNotNull(target);

        var parsingSucceeded = double.TryParse((string)target.Value, out var result);

        if (!parsingSucceeded)
        {
            throw new InvalidTypeCastException(DataType.Number(), target.Value);
        }

        return new EvaluatedResult { Type = DataType.Number(), Value = result };
    }

    private static EvaluatedResult HandleToBool(EvaluatedResult target)
    {
        ValidateTargetIsString(target);
        ValidateTargetIsNotNull(target);

        switch ((string)target.Value)
        {
            case "صحيح":
                return new EvaluatedResult { Type = DataType.Bool(), Value = true };
            case "خطأ":
            case "خطا":
                return new EvaluatedResult { Type = DataType.Bool(), Value = false };
            default:
                throw new InvalidTypeCastException(DataType.Bool(), target.Value);
        }
    }

    private static void ValidateTargetIsNotNull(EvaluatedResult target)
    {
        if (target.Value.Equals(SpecialValues.NULL))
        {
            throw new NullPointerException();
        }
    }

    private static void ValidateTargetIsString(EvaluatedResult target)
    {
        if (!target.Type.IsString())
        {
            throw new InvalidTypeException(target.Type, DataType.String());
        }
    }

    private static EvaluatedResult HandleNegative(EvaluatedResult target)
    {
        ValidateTargetIsNumber(target);
        return new EvaluatedResult { Type = DataType.Number(), Value = -(double)target.Value };
    }

    private static void ValidateTargetIsNumber(EvaluatedResult target)
    {
        if (!target.Type.IsNumber())
        {
            throw new InvalidTypeException(target.Type, DataType.Number());
        }
    }

    private static EvaluatedResult HandleNegation(EvaluatedResult target)
    {
        ValidateTargetIsBool(target);
        return new EvaluatedResult { Type = DataType.Bool(), Value = !(bool)target.Value };
    }

    private static void ValidateTargetIsBool(EvaluatedResult target)
    {
        if (!target.Type.IsBool())
        {
            throw new InvalidTypeException(target.Type, DataType.Bool());
        }
    }
}