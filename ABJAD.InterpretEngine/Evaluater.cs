namespace ABJAD.InterpretEngine;

public interface Evaluater<in T>
{
    object Evaluate(T target);
}