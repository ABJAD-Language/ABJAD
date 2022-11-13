using ABJAD.ParseEngine.Bindings;
using ABJAD.ParseEngine.Service.Api;

namespace ABJAD.ParseEngine.Service.Core;

public interface IBindingApiMapper
{
    BindingApiModel Map(Binding binding);
}