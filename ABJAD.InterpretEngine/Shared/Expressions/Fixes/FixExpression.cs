using ABJAD.InterpretEngine.Shared.Expressions.Primitives;

namespace ABJAD.InterpretEngine.Shared.Expressions.Fixes;

public abstract class FixExpression : Expression
{
    public IdentifierPrimitive Target { get; set; }
}