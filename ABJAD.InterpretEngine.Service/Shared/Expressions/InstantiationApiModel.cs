using ABJAD.InterpretEngine.Shared.Expressions.Primitives;

namespace ABJAD.InterpretEngine.Service.Shared.Expressions;

public class InstantiationApiModel
{
    public IdentifierPrimitive Class { get; }
    public List<object> Arguments { get; }

    public InstantiationApiModel(IdentifierPrimitive @class, List<object> arguments)
    {
        Class = @class;
        Arguments = arguments;
    }
}