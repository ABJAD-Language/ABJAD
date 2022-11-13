using ABJAD.ParseEngine.Bindings;

namespace ABJAD.ParseEngine;

public interface IParser
{
    List<Binding> Parse();
}