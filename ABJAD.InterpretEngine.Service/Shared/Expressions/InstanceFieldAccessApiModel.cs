using ABJAD.InterpretEngine.Shared.Expressions.Primitives;

namespace ABJAD.InterpretEngine.Service.Shared.Expressions;

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