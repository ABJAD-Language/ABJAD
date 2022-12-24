using ABJAD.InterpretEngine.Declarations.Strategies;
using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Shared.Declarations;
using ABJAD.InterpretEngine.Shared.Statements;
using ABJAD.InterpretEngine.Types;
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

    [Fact(DisplayName = "does not add the constructor declarations to the type list of declarations")]
    public void does_not_add_the_constructor_declarations_to_the_type_list_of_declarations()
    {
        var constructorDeclaration = new ConstructorDeclaration() { Parameters = new List<Parameter>() };
        var variableDeclaration = new VariableDeclaration();
        var declarations = new List<Declaration> { constructorDeclaration, variableDeclaration };
        var classDeclaration = new ClassDeclaration { Name = "class", Declarations = declarations};
        var strategy = new ClassDeclarationInterpretationStrategy(classDeclaration, scope);
        strategy.Apply();
        scope.Received(1).DefineType("class", Arg.Compat.Is<ClassElement>(c => c.Declarations.Count == 1 && c.Declarations[0] == variableDeclaration));
    }

    [Fact(DisplayName = "adds the constructor declarations to the class element")]
    public void adds_the_constructor_declarations_to_the_class_element()
    {
        var parameterType = Substitute.For<DataType>();
        var body = new Block();
        var constructorDeclaration = new ConstructorDeclaration
        {
            Body = body,
            Parameters = new List<Parameter> { new() { Type = parameterType, Name = "param" }}
        };
        var variableDeclaration = new VariableDeclaration();
        var declarations = new List<Declaration> { constructorDeclaration, variableDeclaration };
        var classDeclaration = new ClassDeclaration { Name = "class", Declarations = declarations};
        var strategy = new ClassDeclarationInterpretationStrategy(classDeclaration, scope);
        strategy.Apply();
        scope.Received(1).DefineTypeConstructor("class", Arg.Compat.Is<ConstructorElement>(c =>             
            c.Parameters.Count == 1 &&
            c.Parameters[0].Name == "param" &&
            c.Parameters[0].Type == parameterType &&
            c.Body == body
        ));
    }

    [Fact(DisplayName = "throws error if a matching constructor for the class is already added")]
    public void throws_error_if_a_matching_constructor_for_the_class_is_already_added()
    {
        var parameterType = Substitute.For<DataType>();
        var constructorDeclaration = new ConstructorDeclaration
        {
            Parameters = new List<Parameter> { new() { Type = parameterType, Name = "param" }}
        };
        var classDeclaration = new ClassDeclaration { Name = "class", Declarations = new List<Declaration> { constructorDeclaration }};
        var strategy = new ClassDeclarationInterpretationStrategy(classDeclaration, scope);

        scope.TypeHasConstructor("class", parameterType).Returns(true);

        Assert.Throws<MatchingConstructorExistsException>(() => strategy.Apply());
    }

}