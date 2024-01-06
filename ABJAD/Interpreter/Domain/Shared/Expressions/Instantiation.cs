namespace ABJAD.Interpreter.Domain.Shared.Expressions;

public class Instantiation : Expression
{
    public string ClassName { get; set; }
    public List<Expression> Arguments { get; set; }
}