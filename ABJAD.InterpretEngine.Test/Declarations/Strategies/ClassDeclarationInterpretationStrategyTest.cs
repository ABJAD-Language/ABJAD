using ABJAD.InterpretEngine.Declarations.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.Declarations.Strategies;

public class ClassDeclarationInterpretationStrategyTest
{
    private readonly Interpreter<Declaration> declarationInterpreter = Substitute.For<Interpreter<Declaration>>();
    private readonly ScopeFacade scope = Substitute.For<ScopeFacade>();

    [Fact(DisplayName = "throws error if a class with the same name exists in the scope")]
    public void throws_error_if_a_class_with_the_same_name_exists_in_the_scope()
    {
        var classDeclaration = new ClassDeclaration { Name = "class" };
        scope.TypeExists("class").Returns(true);
        var strategy = new ClassDeclarationInterpretationStrategy(classDeclaration, scope);
        Assert.Throws<MatchingClassDeclarationAlreadyExistsException>(() => strategy.Apply());
    }

    [Fact(DisplayName = "adds the new type to the scope on the happy path")]
    public void adds_the_new_type_to_the_scope_on_the_happy_path()
    {
        var declarations = new List<Declaration> { Substitute.For<Declaration>()};
        var classDeclaration = new ClassDeclaration { Name = "class", Declarations = declarations};
        var strategy = new ClassDeclarationInterpretationStrategy(classDeclaration, scope);
        strategy.Apply();
        scope.Received(1).DefineType("class", Arg.Compat.Is<ClassElement>(element => element.Declarations == declarations));
    }

}