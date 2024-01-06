using ABJAD.Parser.Expressions;
using Newtonsoft.Json;

namespace ABJAD.Parser.Declarations;

public class VariableDeclarationApiModel : DeclarationApiModel
{
    [JsonProperty("type")]
    public string VariableType { get; }
    public string Name { get; }
    public ExpressionApiModel? Value { get; }

    public VariableDeclarationApiModel(string variableType, string name, ExpressionApiModel? value) : this(variableType, name)
    {
        Value = value;
    }

    public VariableDeclarationApiModel(string variableType, string name)
    {
        VariableType = variableType;
        Name = name;
        Type = "declaration.variable";
    }
}