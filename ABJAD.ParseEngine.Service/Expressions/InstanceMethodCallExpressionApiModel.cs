namespace ABJAD.ParseEngine.Service.Expressions;

public class InstanceMethodCallExpressionApiModel : ExpressionApiModel
{
    public List<PrimitiveExpressionApiModel> Instances { get; }
    public PrimitiveExpressionApiModel Method { get; }
    public List<ExpressionApiModel> Arguments { get; }

    public InstanceMethodCallExpressionApiModel(List<PrimitiveExpressionApiModel> instances, PrimitiveExpressionApiModel method, List<ExpressionApiModel> arguments)
    {
        Instances = instances;
        Method = method;
        Arguments = arguments;
        Type = "expression.instanceMethodCall";
    }
}