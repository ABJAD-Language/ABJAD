using ABJAD.InterpretEngine.Shared.Expressions.Primitives;

namespace ABJAD.InterpretEngine.Expressions.Strategies;

public class PrimitiveInterpretingStrategy : ExpressionInterpretingStrategy
{
    private readonly Primitive primitive;
    private readonly IScope scope;

    public PrimitiveInterpretingStrategy(Primitive primitive, IScope scope)
    {
        this.primitive = primitive;
        this.scope = scope;
    }

    public object Apply()
    {
        return primitive switch
        {
            BoolPrimitive boolPrimitive => boolPrimitive.Value,
            NumberPrimitive numberPrimitive => numberPrimitive.Value,
            StringPrimitive stringPrimitive => stringPrimitive.Value,
            NullPrimitive => null,
            IdentifierPrimitive identifierPrimitive => GetIdentifierValueIfExists(identifierPrimitive)
        };
    }

    private object GetIdentifierValueIfExists(IdentifierPrimitive identifierPrimitive)
    {
        if (scope.ReferenceExists(identifierPrimitive.Value))
        {
            return scope.Get(identifierPrimitive.Value);
        }

        throw new ReferenceNameDoesNotExistException(identifierPrimitive.Value);
    }
}