﻿using ABJAD.Parser.Domain.Primitives;

namespace ABJAD.Parser.Expressions.Primitives;

public static class PrimitiveApiModelMapper
{
    public static PrimitiveExpressionApiModel Map(Primitive primitive)
    {
        return primitive switch
        {
            StringPrimitive stringPrimitive => new StringPrimitiveApiModel(stringPrimitive.Value),
            NumberPrimitive numberPrimitive => new NumberPrimitiveApiModel(numberPrimitive.Value),
            BoolPrimitive boolPrimitive => new BoolPrimitiveApiModel(boolPrimitive.Value),
            IdentifierPrimitive identifierPrimitive => new IdentifierPrimitiveApiModel(identifierPrimitive.Value),
            NullPrimitive => new NullPrimitiveApiModel()
        };
    }
}