using Ardalis.GuardClauses;

namespace ABJAD.Parser.Domain.Declarations;

public class FunctionParameter
{
    public FunctionParameter(string type, string name)
    {
        Guard.Against.NullOrEmpty(type);
        Guard.Against.NullOrEmpty(name);
        Type = type;
        Name = name;
    }

    public string Type { get; }
    public string Name { get; }
}