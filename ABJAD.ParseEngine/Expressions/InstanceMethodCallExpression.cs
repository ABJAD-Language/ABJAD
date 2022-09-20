namespace ABJAD.ParseEngine.Expressions;

public class InstanceMethodCallExpression : CallExpression
{
    public InstanceMethodCallExpression(PrimitiveExpression instance, PrimitiveExpression method, List<Expression> arguments) : base(method, arguments)
    {
        Instance = instance;
    }

    public PrimitiveExpression Instance { get; }
}