using ABJAD.InterpretEngine.Declarations.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Declarations.Strategies;

public class FunctionDeclarationInterpretationStrategyTest
{
    private readonly ScopeFacade scope = Substitute.For<ScopeFacade>();

    [Fact(DisplayName = "throws error if a matching function declaration exists in the same scope")]
    public void throws_error_if_a_matching_function_declaration_exists_in_the_same_scope()
    {
        var functionDeclaration = new FunctionDeclaration() { Name = "func", Parameters = new List<Parameter>() };
        scope.FunctionExistsInCurrentScope("func").Returns(true);
        var strategy = new FunctionDeclarationInterpretationStrategy(functionDeclaration, scope);
        Assert.Throws<MatchingFunctionAlreadyExistsException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "adds the new function to the scope on the happy path")]
    public void adds_the_new_function_to_the_scope_on_the_happy_path()
    {
        var paramType = Substitute.For<DataType>();
        var parameters = new List<Parameter>() { new() { Name = "param", Type = paramType}};
        var returnType = Substitute.For<DataType>();
        var body = new Block { Bindings = new List<Binding> { Substitute.For<Statement>()}};
        var functionDeclaration = new FunctionDeclaration() { Name = "func", Parameters = parameters, Body = body , ReturnType = returnType };
        
        var strategy = new FunctionDeclarationInterpretationStrategy(functionDeclaration, scope);
        strategy.Apply();
        
        scope.Received(1).DefineFunction("func", Arg.Compat.Is<FunctionElement>(f => 
            body == f.Body 
            && returnType == f.ReturnType 
            && f.Parameters.Count == 1
            && "param" == f.Parameters.First().Name
            && paramType == f.Parameters.First().Type
        ));
    }
}