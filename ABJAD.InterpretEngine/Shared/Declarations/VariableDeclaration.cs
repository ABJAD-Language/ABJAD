using ABJAD.InterpretEngine.Shared.Expressions;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Shared.Declarations;

public class VariableDeclaration : Declaration
{
    public string Name { get; set; }
    public DataType Type { get; set; }
    public Expression? Value { get; set; }
}