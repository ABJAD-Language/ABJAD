using ABJAD.InterpretEngine.Shared.Expressions.Primitives;
using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.Shared.Declarations;

public class ConstantDeclaration : Declaration
{
    public string Name { get; set; }
    public DataType Type { get; set; }
    public Primitive Value { get; set; }
}