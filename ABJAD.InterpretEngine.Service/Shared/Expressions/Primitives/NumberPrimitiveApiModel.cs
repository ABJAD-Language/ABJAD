namespace ABJAD.InterpretEngine.Service.Shared.Expressions.Primitives;

public class NumberPrimitiveApiModel
{
    public double Value { get; }

    public NumberPrimitiveApiModel(double value)
    {
        Value = value;
    }
}