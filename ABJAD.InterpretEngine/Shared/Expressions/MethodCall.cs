namespace ABJAD.InterpretEngine.Shared.Expressions;

public class MethodCall : Expression
{
    public List<string> Instances { get; set; }
    public string MethodName { get; set; }
    public List<Expression> Arguments { get; set; }
}