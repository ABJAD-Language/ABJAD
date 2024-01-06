using ABJAD.Interpreter.Domain.Shared.Expressions.Primitives;

namespace ABJAD.Interpreter.Shared.Expressions;

public class InstanceFieldAccessApiModel
{
    public IdentifierPrimitive Instance { get; }
    public List<IdentifierPrimitive> Fields { get; }

    public InstanceFieldAccessApiModel(IdentifierPrimitive instance, List<IdentifierPrimitive> fields)
    {
        Instance = instance;
        Fields = fields;
    }
}