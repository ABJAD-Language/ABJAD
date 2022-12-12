namespace ABJAD.InterpretEngine;

public interface IScope
{
    bool ReferenceExists(string name);
    object Get(string name);
}