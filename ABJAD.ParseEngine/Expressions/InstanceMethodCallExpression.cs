namespace ABJAD.ParseEngine.Expressions;

public class InstanceMethodCallExpression : CallExpression
{
    public InstanceMethodCallExpression(IEnumerable<PrimitiveExpression> instances, PrimitiveExpression method,
        List<Expression> arguments) : base(method, arguments)
    {
        Instances = instances;
    }

    public IEnumerable<PrimitiveExpression> Instances { get; }
}