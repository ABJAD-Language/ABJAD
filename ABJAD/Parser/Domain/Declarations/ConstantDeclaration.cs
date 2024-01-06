using ABJAD.Parser.Domain.Expressions;

namespace ABJAD.Parser.Domain.Declarations;

public class ConstantDeclaration : Declaration
{
    public ConstantDeclaration(string type, string name, Expression value)
    {
        Type = type;
        Name = name;
        Value = value;
    }

    public string Type { get; }
    public string Name { get; }
    public Expression Value { get; }
}