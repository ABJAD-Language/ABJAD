using ABJAD.ParseEngine.Service.Expressions;

namespace ABJAD.ParseEngine.Service.Declarations;

public class VariableDeclarationApiModel : DeclarationApiModel
{
    public string VariableType { get; }
    public string Name { get; }
    public ExpressionApiModel? Value { get; }

    public VariableDeclarationApiModel(string variableType, string name, ExpressionApiModel? value)
    {
        VariableType = variableType;
        Name = name;
        Value = value;
        Type = "declaration.variable";
    }
}