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

    public object Apply()
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

    private static double HandleModulo(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsNumber(firstOperand);
        ValidateOperandIsNumber(secondOperand);

        return (double)firstOperand.Value % (double)secondOperand.Value;
    }

    private static double HandleDivision(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsNumber(firstOperand);
        ValidateOperandIsNumber(secondOperand);

        if ((double)secondOperand.Value == 0.0)
        {
            throw new DivisionByZeroException();
        }

        return (double)firstOperand.Value / (double)secondOperand.Value;
    }

    private static double HandleMultiplication(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsNumber(firstOperand);
        ValidateOperandIsNumber(secondOperand);
        
        return (double)firstOperand.Value * (double)secondOperand.Value;
    }

    private static double HandleSubtraction(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        ValidateOperandIsNumber(firstOperand);
        ValidateOperandIsNumber(secondOperand);

        return (double)firstOperand.Value - (double)secondOperand.Value;
    }

    private static void ValidateOperandIsNumber(EvaluatedResult operand)
    {
        if (!operand.Type.IsNumber())
        {
            throw new InvalidTypeException(operand.Type, DataType.Number());
        }
    }

    private static object HandleAddition(EvaluatedResult firstOperand, EvaluatedResult secondOperand)
    {
        if (firstOperand.Type.IsNumber())
        {
            if (secondOperand.Type.IsNumber())
            {
                return (double)firstOperand.Value + (double)secondOperand.Value;
            }

            if (secondOperand.Type.IsString())
            {
                return (double)firstOperand.Value + (string)secondOperand.Value;
            }
            
            throw new InvalidTypeException(secondOperand.Type, DataType.Number(), DataType.String());
        } 
        
        if (firstOperand.Type.IsString())
        {
            if (secondOperand.Type.IsString())
            {
                return (string)firstOperand.Value + (string)secondOperand.Value;
            }

            if (secondOperand.Type.IsNumber())
            {
                return (string)firstOperand.Value + (double)secondOperand.Value;
            }
            
            throw new InvalidTypeException(secondOperand.Type, DataType.Number(), DataType.String());
        }

        throw new InvalidTypeException(firstOperand.Type, DataType.Number(), DataType.String());
    }
}