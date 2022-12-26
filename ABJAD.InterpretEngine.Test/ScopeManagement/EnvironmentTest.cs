using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Types;
using NSubstitute;
using Environment = ABJAD.InterpretEngine.ScopeManagement.Environment;

namespace ABJAD.InterpretEngine.Test.ScopeManagement;

public class EnvironmentTest
{
    [Fact(DisplayName = "returns false when asked about a reference that does not exist in any of the scopes")]
    public void returns_false_when_asked_about_a_reference_that_does_not_exist_in_any_of_the_scopes()
    {
        var firstScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id1", new StateElement() }});
        var secondScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id2", new StateElement() }});
        var scopes = new List<Scope>() { new(firstScope, null, null), new(secondScope, null, null) };
        var environment = new Environment(scopes);
        Assert.False(environment.ReferenceExists("id3"));
    }

    [Fact(DisplayName = "returns true when asked about a reference that exists in one of the scopes")]
    public void returns_true_when_asked_about_a_reference_that_exists_in_one_of_the_scopes()
    {
        var firstScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id1", new StateElement() }});
        var secondScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id2", new StateElement() }});
        var scopes = new List<Scope>() { new(firstScope, null, null), new(secondScope, null, null) };
        var environment = new Environment(scopes);
        Assert.True(environment.ReferenceExists("id2"));
    }

    [Fact(DisplayName = "returns true if asked about a reference existence in the current scope which exists")]
    public void returns_true_if_asked_about_a_reference_existence_in_the_current_scope_which_exists()
    {
        var firstScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id1", new StateElement() }});
        var secondScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id2", new StateElement() }});
        var scopes = new List<Scope>() { new(firstScope, null, null), new(secondScope, null, null) };
        var environment = new Environment(scopes);
        Assert.True(environment.ReferenceExistsInCurrentScope("id2"));
    }

    [Fact(DisplayName = "returns false if asked about a reference existence in the current scope which does not exist")]
    public void returns_false_if_asked_about_a_reference_existence_in_the_current_scope_which_does_not_exist()
    {
        var firstScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id1", new StateElement() }});
        var secondScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id2", new StateElement() }});
        var scopes = new List<Scope>() { new(firstScope, null, null), new(secondScope, null, null) };
        var environment = new Environment(scopes);
        Assert.False(environment.ReferenceExistsInCurrentScope("id1"));
    }

    [Fact(DisplayName = "getting the type of a reference gives priority to the state in the later scopes")]
    public void getting_the_type_of_a_reference_gives_priority_to_the_state_in_the_later_scopes()
    {
        var firstScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Type = DataType.Number() } }});
        var secondScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Type = DataType.String() } }});
        var scopes = new List<Scope>() { new(firstScope, null, null), new(secondScope, null, null) };
        var environment = new Environment(scopes);
        Assert.False(environment.GetReferenceType("id").IsNumber());
        Assert.True(environment.GetReferenceType("id").IsString());
    }

    [Fact(DisplayName = "retrieving the value of a reference gives priority to the state in the later scopes")]
    public void retrieving_the_value_of_a_reference_gives_priority_to_the_state_in_the_later_scopes()
    {
        var firstScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var secondScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 2 } }});
        var scopes = new List<Scope>() { new(firstScope, null, null), new(secondScope, null, null) };
        var environment = new Environment(scopes);
        Assert.Equal(2, environment.GetReference("id"));
    }

    [Fact(DisplayName = "returns true if asked about a constant reference if is constant")]
    public void returns_true_if_asked_about_a_constant_reference_if_is_constant()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { IsConstant = true } }});
        var scopes = new List<Scope>() { new(scope, null, null) };
        var environment = new Environment(scopes);
        Assert.True(environment.IsReferenceConstant("id"));
    }

    [Fact(DisplayName = "returns false if asked about a variable reference if is constant")]
    public void returns_true_if_asked_about_a_variable_reference_if_is_constant()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { IsConstant = false } }});
        var scopes = new List<Scope>() { new(scope, null, null) };
        var environment = new Environment(scopes);
        Assert.False(environment.IsReferenceConstant("id"));
    }

    [Fact(DisplayName = "checking if reference is constant gives priority to the later scopes")]
    public void checking_if_reference_is_constant_gives_priority_to_the_later_scopes()
    {
        var firstScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { IsConstant = true } }});
        var secondScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { IsConstant = false } }});
        var scopes = new List<Scope>() { new(firstScope, null, null), new(secondScope, null, null) };
        var environment = new Environment(scopes);
        Assert.False(environment.IsReferenceConstant("id"));
    }

    [Fact(DisplayName = "setting a value to a reference should preserve its type when it exists in the last scope")]
    public void setting_a_value_to_a_reference_should_preserve_its_type_when_it_exists_in_the_last_scope()
    {
        var firstScope = new ReferenceScope(new Dictionary<string, StateElement>());
        var secondScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Type = DataType.Number(), Value = 1 } }});
        var scopes = new List<Scope>() { new(firstScope, null, null), new(secondScope, null, null) };
        var environment = new Environment(scopes);
        environment.UpdateReference("id", 5);
        Assert.True(environment.GetReferenceType("id").IsNumber());
        Assert.Equal(5, environment.GetReference("id"));
    }

    [Fact(DisplayName = "setting a value to a reference should preserve its type when it exists in other than the last scope")]
    public void setting_a_value_to_a_reference_should_preserve_its_type_when_it_exists_in_other_than_the_last_scope()
    {
        var firstScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Type = DataType.Number(), Value = 1 } }});
        var secondScope = new ReferenceScope(new Dictionary<string, StateElement>());
        var scopes = new List<Scope>() { new(firstScope, null, null), new(secondScope, null, null) };
        var environment = new Environment(scopes);
        environment.UpdateReference("id", 5);
        Assert.True(environment.GetReferenceType("id").IsNumber());
        Assert.Equal(5, environment.GetReference("id"));
    }

    [Fact(DisplayName = "defining a new variable adds it to the last scope")]
    public void defining_a_new_variable_adds_it_to_the_last_scope()
    {
        var firstScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var secondScope = new ReferenceScope(new Dictionary<string, StateElement>());
        var scopes = new List<Scope>() { new(firstScope, null, null), new(secondScope, null, null) };
        var environment = new Environment(scopes);
        environment.DefineVariable("id", DataType.String(), "hello");
        Assert.True(environment.GetReferenceType("id").IsString());
        Assert.Equal("hello", environment.GetReference("id"));
        Assert.False(environment.IsReferenceConstant("id"));
    }
    
    [Fact(DisplayName = "adding a new variable to a cloned environment does not add it to the original one")]
    public void adding_a_new_variable_to_a_cloned_environment_does_not_add_it_to_the_original_one()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var scopes = new List<Scope>() { new(scope, functionScope, typeScope) };
        var environment = new Environment(scopes);

        var cloneEnvironment = (Environment) environment.CloneScope();
        cloneEnvironment.DefineVariable("id2", DataType.String(), "hello");
        Assert.True(cloneEnvironment.ReferenceExists("id2"));
        Assert.False(environment.ReferenceExists("id2"));
    }

    [Fact(DisplayName = "defining a new variable in an environment does not add it to its clone")]
    public void defining_a_new_variable_in_an_environment_does_not_add_it_to_its_clone()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var scopes = new List<Scope>() { new(scope, functionScope, typeScope) };
        var environment = new Environment(scopes);
        var cloneEnvironment = (Environment) environment.CloneScope();

        environment.DefineVariable("id2", DataType.String(), "hello");
        Assert.False(cloneEnvironment.ReferenceExists("id2"));
        Assert.True(environment.ReferenceExists("id2"));
    }

    [Fact(DisplayName = "defining a new constant adds it to the last scope")]
    public void defining_a_new_constant_adds_it_to_the_last_scope()
    {
        var firstScope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var secondScope = new ReferenceScope(new Dictionary<string, StateElement>());
        var scopes = new List<Scope>() { new(firstScope, null, null), new(secondScope, null, null) };
        var environment = new Environment(scopes);
        environment.DefineConstant("id", DataType.String(), "hello");
        Assert.True(environment.GetReferenceType("id").IsString());
        Assert.Equal("hello", environment.GetReference("id"));
        Assert.True(environment.IsReferenceConstant("id"));
    }

    [Fact(DisplayName = "adding a new constant to a cloned environment does not add it to the original one")]
    public void adding_a_new_constant_to_a_cloned_environment_does_not_add_it_to_the_original_one()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var scopes = new List<Scope>() { new(scope, functionScope, typeScope) };
        var environment = new Environment(scopes);

        var cloneEnvironment = (Environment) environment.CloneScope();
        cloneEnvironment.DefineConstant("id2", DataType.String(), "hello");
        Assert.True(cloneEnvironment.ReferenceExists("id2"));
        Assert.False(environment.ReferenceExists("id2"));
    }
    
    [Fact(DisplayName = "defining a new constant in an environment does not add it to its clone")]
    public void defining_a_new_constant_in_an_environment_does_not_add_it_to_its_clone()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var scopes = new List<Scope>() { new(scope, functionScope, typeScope) };
        var environment = new Environment(scopes);
        var cloneEnvironment = (Environment) environment.CloneScope();

        environment.DefineConstant("id2", DataType.String(), "hello");
        Assert.False(cloneEnvironment.ReferenceExists("id2"));
        Assert.True(environment.ReferenceExists("id2"));
    }
    
    [Fact(DisplayName = "modifying the value of a reference in a cloned environment does not modify it in the original one")]
    public void modifying_the_value_of_a_reference_in_a_cloned_environment_does_not_modify_it_in_the_original_one()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var scopes = new List<Scope>() { new(scope, functionScope, typeScope) };
        var environment = new Environment(scopes);
        var cloneEnvironment = (Environment) environment.CloneScope();

        cloneEnvironment.UpdateReference("id", 2);
        Assert.Equal(1, environment.GetReference("id"));
        Assert.Equal(2, cloneEnvironment.GetReference("id"));
    }

    [Fact(DisplayName = "modifying the value of a reference in an environment does not modify it in its clone")]
    public void modifying_the_value_of_a_reference_in_an_environment_does_not_modify_it_in_its_clone()
    {
        var scope = new ReferenceScope(new Dictionary<string, StateElement>() { { "id", new StateElement() { Value = 1 } }});
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var scopes = new List<Scope>() { new(scope, functionScope, typeScope) };
        var environment = new Environment(scopes);
        var cloneEnvironment = (Environment) environment.CloneScope();

        environment.UpdateReference("id", 2);
        Assert.Equal(2, environment.GetReference("id"));
        Assert.Equal(1, cloneEnvironment.GetReference("id"));
    }

    [Fact(DisplayName = "checking if function exists delegates to the function scope")]
    public void checking_if_function_exists_delegates_to_the_function_scope()
    {
        var firstFunctionScope = Substitute.For<IFunctionScope>();
        var secondFunctionScope = Substitute.For<IFunctionScope>();
        var parametersType = Substitute.For<DataType>();
        firstFunctionScope.FunctionExists("func", parametersType).Returns(true);

        var scopes = new List<Scope>()
        {
            new(null, firstFunctionScope, null), 
            new(null, secondFunctionScope, null)
        };
        var environment = new Environment(scopes);
        var functionExists = environment.FunctionExists("func", parametersType);
        Assert.True(functionExists);
    }

    [Fact(DisplayName = "checking if function exists in current scope only delegates to the last scope")]
    public void checking_if_function_exists_in_current_scope_only_delegates_to_the_last_scope()
    {
        var firstFunctionScope = Substitute.For<IFunctionScope>();
        var secondFunctionScope = Substitute.For<IFunctionScope>();
        var parametersType = Substitute.For<DataType>();
        firstFunctionScope.FunctionExists("func", parametersType).Returns(true);
        
        var scopes = new List<Scope>()
        {
            new(null, firstFunctionScope, null), 
            new(null, secondFunctionScope, null)
        };
        var environment = new Environment(scopes);
        var functionExists = environment.FunctionExistsInCurrentScope("func", parametersType);
        firstFunctionScope.DidNotReceive().FunctionExists("func", parametersType);
        secondFunctionScope.Received(1).FunctionExists("func", parametersType);
        Assert.False(functionExists);
    }

    [Fact(DisplayName = "getting function return type delegates to the function scope giving priority to the last ones")]
    public void getting_function_return_type_delegates_to_the_function_scope_giving_priority_to_the_last_ones()
    {
        var firstFunctionScope = Substitute.For<IFunctionScope>();
        var secondFunctionScope = Substitute.For<IFunctionScope>();

        var parametersType = Substitute.For<DataType>();
        firstFunctionScope.FunctionExists("func", parametersType).Returns(true);
        secondFunctionScope.FunctionExists("func", parametersType).Returns(true);

        var functionReturnType = Substitute.For<DataType>();
        secondFunctionScope.GetFunctionReturnType("func", parametersType).Returns(functionReturnType);
        
        var scopes = new List<Scope>()
        {
            new(null, firstFunctionScope, null), 
            new(null, secondFunctionScope, null)
        };
        var environment = new Environment(scopes);
        var returnType = environment.GetFunctionReturnType("func", parametersType);
        
        Assert.Equal(functionReturnType, returnType);
    }

    [Fact(DisplayName = "getting function delegates to the function scope giving priority to the last ones")]
    public void getting_function_delegates_to_the_function_scope_giving_priority_to_the_last_ones()
    {
        var firstFunctionScope = Substitute.For<IFunctionScope>();
        var secondFunctionScope = Substitute.For<IFunctionScope>();

        var parametersType = Substitute.For<DataType>();
        secondFunctionScope.FunctionExists("func", parametersType).Returns(true);
        
        var function = new FunctionElement();
        secondFunctionScope.GetFunction("func", parametersType).Returns(function);
        
        var scopes = new List<Scope>()
        {
            new(null, firstFunctionScope, null), 
            new(null, secondFunctionScope, null)
        };
        var environment = new Environment(scopes);
        Assert.Equal(function, environment.GetFunction("func", parametersType));
    }

    [Fact(DisplayName = "defining a new function adds it to the last scope")]
    public void defining_a_new_function_adds_it_to_the_last_scope()
    {
        var firstFunctionScope = Substitute.For<IFunctionScope>();
        var secondFunctionScope = Substitute.For<IFunctionScope>();

        secondFunctionScope.FunctionExists("func").Returns(false);
        firstFunctionScope.FunctionExists("func").Returns(true);

        var scopes = new List<Scope>()
        {
            new(null, firstFunctionScope, null), 
            new(null, secondFunctionScope, null)
        };
        var environment = new Environment(scopes);
        var function = new FunctionElement() { Parameters = new List<FunctionParameter>()};
        environment.DefineFunction("func", function);
        secondFunctionScope.Received(1).DefineFunction("func", function);
    }

    [Fact(DisplayName = "adding functions to a cloned environment does not add it to the original one")]
    public void adding_functions_to_a_cloned_environment_does_not_add_it_to_the_original_one()
    {
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var referenceScope = new ReferenceScope(new Dictionary<string, StateElement>());
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var scope = new Scope(referenceScope, functionScope, typeScope);
        var environment = new Environment(new List<Scope>() { scope });
        var clonedEnvironment = environment.CloneScope();

        var parameterType = Substitute.For<DataType>();
        environment.DefineFunction("func", new FunctionElement() { Parameters = new List<FunctionParameter> { new() { Type = parameterType} }});
        Assert.False(clonedEnvironment.FunctionExists("func", parameterType));
        
        clonedEnvironment.DefineFunction("func1", new FunctionElement() { Parameters = new List<FunctionParameter>()});
        Assert.False(environment.FunctionExists("func"));
    }

    [Fact(DisplayName = "checking if type exists delegates to the type scope")]
    public void checking_if_type_exists_delegates_to_the_type_scope()
    {
        var typeScope = Substitute.For<ITypeScope>();
        var scopes = new List<Scope> { new(Substitute.For<IReferenceScope>(), Substitute.For<IFunctionScope>(), typeScope) };
        var environment = new Environment(scopes);

        typeScope.TypeExists("type").Returns(true);

        var typeExists = environment.TypeExists("type");
        typeScope.Received(1).TypeExists("type");
        Assert.True(typeExists);
    }

    [Fact(DisplayName = "checking if a type has a certain constructor delegates to the type scope")]
    public void checking_if_a_type_has_a_certain_constructor_delegates_to_the_type_scope()
    {
        var typeScope = Substitute.For<ITypeScope>();
        var scopes = new List<Scope> { new(Substitute.For<IReferenceScope>(), Substitute.For<IFunctionScope>(), typeScope) };
        var environment = new Environment(scopes);

        var parameterTypeOne = Substitute.For<DataType>();
        var parameterTypeTwo = Substitute.For<DataType>();

        typeScope.TypeExists("type").Returns(true);
        typeScope.HasConstructor("type", parameterTypeOne, parameterTypeTwo).Returns(true);

        var typeHasConstructor = environment.TypeHasConstructor("type", parameterTypeOne, parameterTypeTwo);

        typeScope.Received(1).HasConstructor("type", parameterTypeOne, parameterTypeTwo);
        Assert.True(typeHasConstructor);
    }

    [Fact(DisplayName = "getting a type delegates to the type scope")]
    public void getting_a_type_delegates_to_the_type_scope()
    {
        var typeScope = Substitute.For<ITypeScope>();
        var scopes = new List<Scope> { new(Substitute.For<IReferenceScope>(), Substitute.For<IFunctionScope>(), typeScope) };
        var environment = new Environment(scopes);

        var classElement = new ClassElement();
        typeScope.TypeExists("type").Returns(true);
        typeScope.Get("type").Returns(classElement);

        var actualClassElement = environment.GetType("type");
        typeScope.Received(1).Get("type");
        Assert.Equal(classElement, actualClassElement);
    }

    [Fact(DisplayName = "getting a type constructor delegates to the type scope")]
    public void getting_a_type_constructor_delegates_to_the_type_scope()
    {
        var typeScope = Substitute.For<ITypeScope>();
        var scopes = new List<Scope> { new(Substitute.For<IReferenceScope>(), Substitute.For<IFunctionScope>(), typeScope) };
        var environment = new Environment(scopes);

        var parameterType = Substitute.For<DataType>();
        var constructorElement = new ConstructorElement();
        typeScope.TypeExists("type").Returns(true);
        typeScope.GetConstructor("type", parameterType).Returns(constructorElement);

        var actualConstructorElement = environment.GetTypeConstructor("type", parameterType);
        typeScope.Received(1).GetConstructor("type", parameterType);
        Assert.Equal(constructorElement, actualConstructorElement);
    }

    [Fact(DisplayName = "defining a new type delegates to the current scope")]
    public void defining_a_new_type_delegates_to_the_current_scope()
    {
        var globalTypeScope = Substitute.For<ITypeScope>();
        var localTypeScope = Substitute.For<ITypeScope>();
        var globalScope = new Scope(Substitute.For<IReferenceScope>(), Substitute.For<IFunctionScope>(), globalTypeScope);
        var localScope = new Scope(Substitute.For<IReferenceScope>(), Substitute.For<IFunctionScope>(), localTypeScope);
        var scopes = new List<Scope> { globalScope, localScope };
        var environment = new Environment(scopes);

        var classElement = new ClassElement();
        environment.DefineType("type", classElement);
        localTypeScope.Received(1).Define("type", classElement);
        globalTypeScope.DidNotReceive().Define("type", classElement);
    }

    [Fact(DisplayName = "defining a new constructor to a type delegates it to the local type scope")]
    public void defining_a_new_constructor_to_a_type_delegates_it_to_the_local_type_scope()
    {
        var globalTypeScope = Substitute.For<ITypeScope>();
        var localTypeScope = Substitute.For<ITypeScope>();
        var globalScope = new Scope(Substitute.For<IReferenceScope>(), Substitute.For<IFunctionScope>(), globalTypeScope);
        var localScope = new Scope(Substitute.For<IReferenceScope>(), Substitute.For<IFunctionScope>(), localTypeScope);
        var scopes = new List<Scope> { globalScope, localScope };
        var environment = new Environment(scopes);

        var constructorElement = new ConstructorElement();
        environment.DefineTypeConstructor("type", constructorElement);
        localTypeScope.Received(1).DefineConstructor("type", constructorElement);
    }

    [Fact(DisplayName = "adding a new type to a cloned environment does not affect the old one")]
    public void adding_a_new_type_to_a_cloned_environment_does_not_affect_the_old_one()
    {
        var referenceScope = new ReferenceScope(new Dictionary<string, StateElement>());
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var scopes = new List<Scope> { new(referenceScope, functionScope, typeScope) };
        var environment = new Environment(scopes);

        var clonedEnvironment = environment.CloneScope();
        
        environment.DefineType("type1", new ClassElement());
        clonedEnvironment.DefineType("type2", new ClassElement());
        
        Assert.False(clonedEnvironment.TypeExists("type1"));
        Assert.False(environment.TypeExists("type2"));
    }

    [Fact(DisplayName = "adding a new scope allows us to define existing references and functions")]
    public void adding_a_new_scope_allows_us_to_define_existing_references_and_functions()
    {
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var referenceScope = new ReferenceScope(new Dictionary<string, StateElement>());
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var scope = new Scope(referenceScope, functionScope, typeScope);
        var environment = new Environment(new List<Scope>() { scope });
        
        functionScope.DefineFunction("func", new FunctionElement() { Parameters = new List<FunctionParameter>()});
        referenceScope.DefineVariable("id", DataType.Number(), 10);
        
        environment.AddNewScope();
        
        Assert.False(environment.ReferenceExistsInCurrentScope("id"));
        Assert.False(environment.FunctionExistsInCurrentScope("func"));
    }

    [Fact(DisplayName = "removing the last scope removes all the declarations defined at it")]
    public void removing_the_last_scope_removes_all_the_declarations_defined_at_it()
    {
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var referenceScope = new ReferenceScope(new Dictionary<string, StateElement>());
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var scope = new Scope(referenceScope, functionScope, typeScope);
        var environment = new Environment(new List<Scope>() { scope });
        
        environment.AddNewScope();
        
        environment.DefineFunction("func", new FunctionElement() { Parameters = new List<FunctionParameter>()});
        environment.DefineVariable("id", DataType.Number(), 10);
        
        environment.RemoveLastScope();
        
        Assert.False(environment.ReferenceExistsInCurrentScope("id"));
        Assert.False(environment.FunctionExistsInCurrentScope("func"));
    }

    [Fact(DisplayName = "when calling on remove scope and only one scope exists it is replaced with an empty one")]
    public void when_calling_on_remove_scope_and_only_one_scope_exists_it_is_replaced_with_an_empty_one()
    {
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var referenceScope = new ReferenceScope(new Dictionary<string, StateElement>());
        var typeScope = new TypeScope(new Dictionary<string, ClassElement>());
        var scope = new Scope(referenceScope, functionScope, typeScope);
        var environment = new Environment(new List<Scope>() { scope });
        
        environment.DefineFunction("func", new FunctionElement() { Parameters = new List<FunctionParameter>()});
        environment.DefineVariable("id", DataType.Number(), 10);
        
        environment.RemoveLastScope();
        
        Assert.False(environment.ReferenceExistsInCurrentScope("id"));
        Assert.False(environment.FunctionExistsInCurrentScope("func"));
    }

    [Fact(DisplayName = "aggregating an existing scope squash it on top")]
    public void aggregating_an_existing_scope_squash_it_on_top()
    {
        var scopes = new List<Scope>() { ScopeFactory.NewScope() };
        var environment = new Environment(scopes);

        var innerScope1 = ScopeFactory.NewScope();
        var innerScope2 = ScopeFactory.NewScope();
        innerScope1.ReferenceScope.DefineVariable("id", DataType.String(), "hello");
        innerScope2.FunctionScope.DefineFunction("func", new FunctionElement() { Parameters = new List<FunctionParameter>()});
        var scopes2 = new List<Scope>() { innerScope1, innerScope2 };
        var environment2 = new Environment(scopes2);
        
        environment.AddScope(environment2);
        
        Assert.True(environment.ReferenceExistsInCurrentScope("id"));
        Assert.True(environment.FunctionExistsInCurrentScope("func"));
    }

    [Fact(DisplayName = "getting the scopes return the list of defined scopes in the environment")]
    public void getting_the_scopes_return_the_list_of_defined_scopes_in_the_environment()
    {
        var scopes = new List<Scope>()
        {
            new Scope(
                Substitute.For<IReferenceScope>(),
                Substitute.For<IFunctionScope>(),
                Substitute.For<ITypeScope>()
            )
        };
        var environment = new Environment(scopes);
        
        Assert.Equal(scopes, environment.GetScopes());
    }
}