using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Types;

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

    public EvaluatedResult Apply()
    {
        return primitive switch
        {
            BoolPrimitive boolPrimitive => new EvaluatedResult { Type = DataType.Bool(), Value = boolPrimitive.Value },
            NumberPrimitive numberPrimitive => new EvaluatedResult { Type = DataType.Number(), Value = numberPrimitive.Value },
            StringPrimitive stringPrimitive => new EvaluatedResult { Type = DataType.String(), Value = stringPrimitive.Value },
            NullPrimitive => new EvaluatedResult { Type = DataType.Undefined() },
            IdentifierPrimitive identifierPrimitive => GetIdentifierValueIfExists(identifierPrimitive)
        };
    }

    private EvaluatedResult GetIdentifierValueIfExists(IdentifierPrimitive identifierPrimitive)
    {
        if (scope.ReferenceExists(identifierPrimitive.Value))
        {
            var type = scope.GetType(identifierPrimitive.Value);
            var value = scope.Get(identifierPrimitive.Value);
            return new EvaluatedResult { Type = type, Value = value };
        }

        throw new ReferenceNameDoesNotExistException(identifierPrimitive.Value);
    }
}