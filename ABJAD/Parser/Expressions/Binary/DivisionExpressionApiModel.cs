﻿namespace ABJAD.Parser.Expressions.Binary;

public class DivisionExpressionApiModel : BinaryExpressionApiModel
{
    public DivisionExpressionApiModel(ExpressionApiModel firstOperand, ExpressionApiModel secondOperand) : base("division", firstOperand, secondOperand)
    {
    }
}