using ABJAD.ParseEngine.Bindings;
using ABJAD.ParseEngine.Service.Declarations;
using ABJAD.ParseEngine.Service.Statements;

namespace ABJAD.ParseEngine.Service.Bindings;

public static class BindingApiModelMapper
{
    public static BindingApiModel Map(Binding binding)
    {
        return binding switch
        {
            DeclarationBinding declarationBinding => DeclarationsApiModelMapper.Map(declarationBinding.Declaration),
            StatementBinding statementBinding => StatementApiModelMapper.Map(statementBinding.Statement)
        };
    }
}