using ABJAD.InterpretEngine.Shared.Expressions.Primitives;

namespace ABJAD.InterpretEngine.Service.Shared.Expressions;

public class InstanceMethodCallApiModel
{
    public List<IdentifierPrimitive> Instances { get; }
    public IdentifierPrimitive Method { get; }
    public List<object> Arguments { get; }

    public InstanceMethodCallApiModel(List<IdentifierPrimitive> instances, IdentifierPrimitive method, List<object> arguments)
    {
        Instances = instances;
        Method = method;
        Arguments = arguments;
    }
}