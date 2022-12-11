using ABJAD.ParseEngine.Service.Expressions;
using Newtonsoft.Json;

namespace ABJAD.ParseEngine.Service.Declarations;

public class ConstantDeclarationApiModel : DeclarationApiModel
{
    [JsonProperty("type")]
    public string ConstantType { get; }
    public string Name { get; }
    public ExpressionApiModel Value { get; }

    public ConstantDeclarationApiModel(string constantType, string name, ExpressionApiModel value)
    {
        ConstantType = constantType;
        Name = name;
        Value = value;
        Type = "declaration.constant";
    }
}