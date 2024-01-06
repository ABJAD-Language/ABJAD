using ABJAD.Parser.Declarations;
using ABJAD.Parser.Domain.Bindings;
using ABJAD.Parser.Statements;

namespace ABJAD.Parser.Bindings;

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