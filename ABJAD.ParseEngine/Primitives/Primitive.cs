namespace ABJAD.ParseEngine.Primitives;

public abstract class Primitive<T>
{
    public T Value { get; init; } = default!;
}