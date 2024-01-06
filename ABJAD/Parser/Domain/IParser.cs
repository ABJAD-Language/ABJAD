using ABJAD.Parser.Domain.Bindings;

namespace ABJAD.Parser.Domain;

public interface IParser
{
    List<Binding> Parse();
}