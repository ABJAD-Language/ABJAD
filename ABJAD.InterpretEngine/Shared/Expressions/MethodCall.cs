namespace ABJAD.InterpretEngine.Shared.Expressions;

public class MethodCall : Expression
{
    public string MethodName { get; set; }
    public List<Expression> Arguments { get; set; }
}