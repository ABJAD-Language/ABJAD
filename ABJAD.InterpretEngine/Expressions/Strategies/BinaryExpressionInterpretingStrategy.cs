using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Shared.Expressions.Binary;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Expressions.Strategies;

public class BinaryExpressionInterpretingStrategy : ExpressionInterpretingStrategy
{
    private readonly BinaryExpression expression;
    private readonly Evaluator<Expression> expressionEvaluator;

    public BinaryExpressionInterpretingStrategy(BinaryExpression expression, Evaluator<Expression> expressionEvaluator)
    {
        this.expression = expression;
        this.expressionEvaluator = expressionEvaluator;
    }

    public EvaluatedResult Apply()
    {
        var firstOperandEvaluationResult = expressionEvaluator.Evaluate(expression.FirstOperand);
        var secondOperandEvaluationResult = expressionEvaluator.Evaluate(expression.SecondOperand);

        return expression switch
        {
            Addition => HandleAddition(firstOperandEvaluationResult, secondOperandEvaluationResult),
            Subtraction => HandleSubtraction(firstOperandEvaluationResult, secondOperandEvaluationResult),
            Multiplication => HandleMultiplication(firstOperandEvaluationResult, secondOperandEvaluationResult),
            Division => HandleDivision(firstOperandEvaluationResult, secondOperandEvaluationResult),
            Modulo => HandleModulo(firstOperandEvaluationResult, secondOperandEvaluationResult),
            _ => throw new ArgumentException()
        };
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
                var value = (double)firstOperand.Value + (string)secondOperand.Value;
                return new EvaluatedResult { Type = DataType.String(), Value = value };
            }
            
            throw new InvalidTypeException(secondOperand.Type, DataType.Number(), DataType.String());
        } 
        
        if (firstOperand.Type.IsString())
        {
            if (secondOperand.Type.IsString())
            {
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
}