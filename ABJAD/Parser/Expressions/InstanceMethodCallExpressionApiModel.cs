using ABJAD.Parser.Expressions.Primitives;

namespace ABJAD.Parser.Expressions;

public class InstanceMethodCallExpressionApiModel : ExpressionApiModel
{
    public List<PrimitiveExpressionApiModel> Instances { get; } // TODO move to string instances when core class moves to IdentifierPrimitive
    public PrimitiveExpressionApiModel Method { get; } // TODO move to string method when core class moves to IdentifierPrimitive
    public List<ExpressionApiModel> Arguments { get; }

    public InstanceMethodCallExpressionApiModel(List<PrimitiveExpressionApiModel> instances, PrimitiveExpressionApiModel method, List<ExpressionApiModel> arguments)
    {
        Instances = instances;
        Method = method;
        Arguments = arguments;
        Type = "expression.instanceMethodCall";
    }
}