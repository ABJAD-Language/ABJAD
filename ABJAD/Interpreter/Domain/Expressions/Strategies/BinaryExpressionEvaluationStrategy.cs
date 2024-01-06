using ABJAD.Interpreter.Domain.Shared.Expressions.Binary;
using ABJAD.Interpreter.Domain.Types;

namespace ABJAD.Interpreter.Domain.Expressions.Strategies;

public class BinaryExpressionEvaluationStrategy : ExpressionEvaluationStrategy
{
    private readonly BinaryExpression expression;
    private readonly IExpressionEvaluator expressionEvaluator;
    private static readonly double NumberComparisonTolerance = 10e-100;

    public BinaryExpressionEvaluationStrategy(BinaryExpression expression, IExpressionEvaluator expressionEvaluator)
    {
        this.expression = expression;
        this.expressionEvaluator = expressionEvaluator;
    }

    public EvaluatedResult Apply()
    {
        var firstOperandEvaluationResult = expressionEvaluator.Evaluate(expression.FirstOperand);
        var secondOperandEvaluationResult = expressionEvaluator.Evaluate(expression.SecondOperand);

        ValidateOperandValueIsNotUndefined(firstOperandEvaluationResult);
        ValidateOperandValueIsNotUndefined(secondOperandEvaluationResult);

        return expression switch
        {
            Addition => HandleAddition(firstOperandEvaluationResult, secondOperandEvaluationResult),
            Subtraction => HandleSubtraction(firstOperandEvaluationResult, secondOperandEvaluationResult),
            Multiplication => HandleMultiplication(firstOperandEvaluationResult, secondOperandEvaluationResult),
            Division => HandleDivision(firstOperandEvaluationResult, secondOperandEvaluationResult),
            Modulo => HandleModulo(firstOperandEvaluationResult, secondOperandEvaluationResult),
            LogicalAnd => HandleLogicalAnd(firstOperandEvaluationResult, secondOperandEvaluationResult),
            LogicalOr => HandleLogicalOr(firstOperandEvaluationResult, secondOperandEvaluationResult),
            GreaterCheck => HandleGreaterCheck(firstOperandEvaluationResult, secondOperandEvaluationResult),
            GreaterOrEqualCheck => HandleGreaterOrEqualCheck(firstOperandEvaluationResult, secondOperandEvaluationResult),
            LessCheck => HandleLessCheck(firstOperandEvaluationResult, secondOperandEvaluationResult),
            LessOrEqualCheck => HandleLessOrEqualCheck(firstOperandEvaluationResult, secondOperandEvaluationResult),
            EqualityCheck => HandleEqualityCheck(firstOperandEvaluationResult, secondOperandEvaluationResult),
            InequalityCheck => HandleInequalityCheck(firstOperandEvaluationResult, secondOperandEvaluationResult),
            _ => throw new ArgumentException()
        };
    }

    private static void ValidateOperandValueIsNotUndefined(EvaluatedResult firstOperandEvaluationResult)
    {
        if (firstOperandEvaluationResult.Value.Equals(SpecialValues.UNDEFINED))
        {
            throw new OperationOnUndefinedValueException();
        }
    }

    private static EvaluatedResult HandleInequalityCheck(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        if (!firstOperand.Type.Is(secondOperand.Type))
        {
            throw new IncompatibleTypesException(firstOperand.Type, secondOperand.Type);
        }

        bool result;
        if (firstOperand.Type.IsNumber())
        {
            result = Math.Abs((double)firstOperand.Value - (double)secondOperand.Value) > NumberComparisonTolerance;
        }
        else if (firstOperand.Type.IsString())
        {
            result = (string)firstOperand.Value != (string)secondOperand.Value;
        }
        else if (firstOperand.Type.IsBool())
        {
            result = (bool)firstOperand.Value != (bool)secondOperand.Value;
        }
        else
        {
            result = true;
        }

        return new EvaluatedResult { Type = DataType.Bool(), Value = result };
    }

    private static EvaluatedResult HandleEqualityCheck(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        if (!firstOperand.Type.Is(secondOperand.Type))
        {
            throw new IncompatibleTypesException(firstOperand.Type, secondOperand.Type);
        }

        bool result;
        if (firstOperand.Type.IsNumber())
        {
            result = Math.Abs((double)firstOperand.Value - (double)secondOperand.Value) < NumberComparisonTolerance;
        }
        else if (firstOperand.Type.IsString())
        {
            result = (string)firstOperand.Value == (string)secondOperand.Value;
        }
        else if (firstOperand.Type.IsBool())
        {
            result = (bool)firstOperand.Value == (bool)secondOperand.Value;
        }
        else
        {
            result = false;
        }

        return new EvaluatedResult { Type = DataType.Bool(), Value = result };
    }

    private static EvaluatedResult HandleLessOrEqualCheck(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsNumber(firstOperand);
        ValidateOperandIsNumber(secondOperand);

        var value = (double)firstOperand.Value <= (double)secondOperand.Value;
        return new EvaluatedResult { Type = DataType.Bool(), Value = value };
    }

    private static EvaluatedResult HandleLessCheck(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsNumber(firstOperand);
        ValidateOperandIsNumber(secondOperand);

        var value = (double)firstOperand.Value < (double)secondOperand.Value;
        return new EvaluatedResult { Type = DataType.Bool(), Value = value };
    }

    private static EvaluatedResult HandleGreaterOrEqualCheck(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsNumber(firstOperand);
        ValidateOperandIsNumber(secondOperand);

        var value = (double)firstOperand.Value >= (double)secondOperand.Value;
        return new EvaluatedResult { Type = DataType.Bool(), Value = value };
    }

    private static EvaluatedResult HandleGreaterCheck(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsNumber(firstOperand);
        ValidateOperandIsNumber(secondOperand);

        var value = (double)firstOperand.Value > (double)secondOperand.Value;
        return new EvaluatedResult { Type = DataType.Bool(), Value = value };
    }

    private static EvaluatedResult HandleLogicalOr(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsBool(firstOperand);
        ValidateOperandIsBool(secondOperand);

        var value = (bool)firstOperand.Value || (bool)secondOperand.Value;
        return new EvaluatedResult { Type = DataType.Bool(), Value = value };
    }

    private static EvaluatedResult HandleLogicalAnd(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsBool(firstOperand);
        ValidateOperandIsBool(secondOperand);

        var value = (bool)firstOperand.Value && (bool)secondOperand.Value;
        return new EvaluatedResult { Type = DataType.Bool(), Value = value };
    }

    private static void ValidateOperandIsBool(EvaluatedResult operand)
    {
        if (!operand.Type.IsBool())
        {
            throw new InvalidTypeException(operand.Type, DataType.Bool());
        }
    }

    private static EvaluatedResult HandleModulo(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsNumber(firstOperand);
        ValidateOperandIsNumber(secondOperand);

        var value = (double)firstOperand.Value % (double)secondOperand.Value;
        return new EvaluatedResult { Type = DataType.Number(), Value = value };
    }

    private static EvaluatedResult HandleDivision(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsNumber(firstOperand);
        ValidateOperandIsNumber(secondOperand);

        if ((double)secondOperand.Value == 0.0)
        {
            throw new DivisionByZeroException();
        }

        var value = (double)firstOperand.Value / (double)secondOperand.Value;
        return new EvaluatedResult { Type = DataType.Number(), Value = value };
    }

    private static EvaluatedResult HandleMultiplication(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsNumber(firstOperand);
        ValidateOperandIsNumber(secondOperand);

        var value = (double)firstOperand.Value * (double)secondOperand.Value;
        return new EvaluatedResult { Type = DataType.Number(), Value = value };
    }

    private static EvaluatedResult HandleSubtraction(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsNumber(firstOperand);
        ValidateOperandIsNumber(secondOperand);

        var value = (double)firstOperand.Value - (double)secondOperand.Value;
        return new EvaluatedResult { Type = DataType.Number(), Value = value };
    }

    private static void ValidateOperandIsNumber(EvaluatedResult operand)
    {
        if (!operand.Type.IsNumber())
        {
            throw new InvalidTypeException(operand.Type, DataType.Number());
        }
    }

    private static EvaluatedResult HandleAddition(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        if (firstOperand.Type.IsNumber())
        {
            if (secondOperand.Type.IsNumber())
            {
                var value = (double)firstOperand.Value + (double)secondOperand.Value;
                return new EvaluatedResult { Type = DataType.Number(), Value = value };
            }

            if (secondOperand.Type.IsString())
            {
                ValidateOperandIsNotNull(secondOperand);
                var value = (double)firstOperand.Value + (string)secondOperand.Value;
                return new EvaluatedResult { Type = DataType.String(), Value = value };
            }

            throw new InvalidTypeException(secondOperand.Type, DataType.Number(), DataType.String());
        }

        if (firstOperand.Type.IsString())
        {
            ValidateOperandIsNotNull(firstOperand);

            if (secondOperand.Type.IsString())
            {
                ValidateOperandIsNotNull(secondOperand);
                var value = (string)firstOperand.Value + (string)secondOperand.Value;
                return new EvaluatedResult { Type = DataType.String(), Value = value };
            }

            if (secondOperand.Type.IsNumber())
            {
                var value = (string)firstOperand.Value + (double)secondOperand.Value;
                return new EvaluatedResult { Type = DataType.String(), Value = value };
            }

            throw new InvalidTypeException(secondOperand.Type, DataType.Number(), DataType.String());
        }

        throw new InvalidTypeException(firstOperand.Type, DataType.Number(), DataType.String());
    }

    private static void ValidateOperandIsNotNull(EvaluatedResult firstOperand)
    {
        if (firstOperand.Value.Equals(SpecialValues.NULL))
        {
            throw new NullPointerException();
        }
    }
}