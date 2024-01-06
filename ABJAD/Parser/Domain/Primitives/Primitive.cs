namespace ABJAD.Parser.Domain.Primitives;

public interface Primitive
{

}

public abstract class Primitive<T> : Primitive
{
    public T Value { get; init; } = default!;
}