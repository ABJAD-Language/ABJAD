using ABJAD.InterpretEngine.Types;

namespace ABJAD.InterpretEngine.ScopeManagement;

public record StateElement
{
    public DataType Type { get; init; }
    public object Value { get; init; }
}