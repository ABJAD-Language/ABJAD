using ABJAD.Interpreter.Domain.ScopeManagement;
using ABJAD.Interpreter.Domain.Types;
using NSubstitute;

namespace ABJAD.Test.Interpreter.Domain.ScopeManagement;

public class FunctionScopeTest
{
    [Fact(DisplayName = "returns false when asked about a function that does not exist")]
    public void returns_false_when_asked_about_a_function_that_does_not_exist()
    {
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        Assert.False(functionScope.FunctionExists("func", Substitute.For<DataType>()));
    }

    [Fact(DisplayName = "returns false when asked about a function that does not exist with same number of parameters")]
    public void returns_false_when_asked_about_a_function_that_does_not_exist_with_same_number_of_parameters()
    {
        var state = new Dictionary<string, List<FunctionElement>>();
        state.Add("func", new List<FunctionElement>() { new() { Parameters = new List<FunctionParameter>() { new() { Type = Substitute.For<DataType>() } } } });
        var functionScope = new FunctionScope(state);
        Assert.False(functionScope.FunctionExists("func", Substitute.For<DataType>(), Substitute.For<DataType>()));
    }

    [Fact(DisplayName = "returns false when asked about a function that exists with same number of parameters but with different types")]
    public void returns_false_when_asked_about_a_function_that_exists_with_same_number_of_parameters_but_with_different_types()
    {
        var state = new Dictionary<string, List<FunctionElement>>();
        state.Add("func", new List<FunctionElement>() { new() { Parameters = new List<FunctionParameter>() { new() { Type = Substitute.For<DataType>() } } } });
        var functionScope = new FunctionScope(state);
        Assert.False(functionScope.FunctionExists("func", Substitute.For<DataType>()));
    }

    [Fact(DisplayName = "returns true when asked about a function that exists")]
    public void returns_false_when_asked_about_a_function_that_exists()
    {
        var state = new Dictionary<string, List<FunctionElement>>();
        var parametersType = Substitute.For<DataType>();
        parametersType.Is(parametersType).Returns(true);
        state.Add("func", new List<FunctionElement>() { new() { Parameters = new List<FunctionParameter>() { new() { Type = parametersType } } } });
        var functionScope = new FunctionScope(state);
        Assert.True(functionScope.FunctionExists("func", parametersType));
    }

    [Fact(DisplayName = "returns the specified function return type when asked about")]
    public void returns_the_specified_function_return_type_when_asked_about()
    {
        var state = new Dictionary<string, List<FunctionElement>>();
        var firstFuncReturnType = Substitute.For<DataType>();
        var secondFuncReturnType = Substitute.For<DataType>();
        var paramType = Substitute.For<DataType>();
        paramType.Is(paramType).Returns(true);
        state.Add("func", new List<FunctionElement>
        {
            new() { ReturnType = firstFuncReturnType, Parameters = new List<FunctionParameter>() },
            new() { ReturnType = secondFuncReturnType, Parameters = new List<FunctionParameter> { new() { Type = paramType }}}
        });
        var functionScope = new FunctionScope(state);
        Assert.Equal(secondFuncReturnType, functionScope.GetFunctionReturnType("func", paramType));
    }

    [Fact(DisplayName = "returns the specified function when asked about")]
    public void returns_the_specified_function_when_asked_about()
    {
        var state = new Dictionary<string, List<FunctionElement>>();
        var firstFunction = new FunctionElement() { Parameters = new List<FunctionParameter>() };
        var secondFunction = new FunctionElement() { Parameters = new List<FunctionParameter>() { new() } };
        state.Add("func", new List<FunctionElement>
        {
            firstFunction,
            secondFunction
        });
        var functionScope = new FunctionScope(state);
        Assert.Equal(firstFunction, functionScope.GetFunction("func"));
    }

    [Fact(DisplayName = "defining a new function adds it to the state")]
    public void defining_a_new_function_adds_it_to_the_state()
    {
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var paramType = Substitute.For<DataType>();
        paramType.Is(paramType).Returns(true);
        var function = new FunctionElement() { Parameters = new List<FunctionParameter> { new() { Type = paramType } } };
        functionScope.DefineFunction("func", function);
        Assert.Equal(function, functionScope.GetFunction("func", paramType));
    }

    [Fact(DisplayName = "cloning the scope creates a deep copy of it")]
    public void cloning_the_scope_creates_a_deep_copy_of_it()
    {
        var state = new Dictionary<(string, int), FunctionElement>();
        var functionScope = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        var cloneFunctionScope = functionScope.Clone();
        functionScope.DefineFunction("func", new FunctionElement { Parameters = new List<FunctionParameter> { new() { Type = DataType.Bool() } } });
        Assert.False(cloneFunctionScope.FunctionExists("func", DataType.Bool()));
        cloneFunctionScope.DefineFunction("func1", new FunctionElement { Parameters = new List<FunctionParameter>() });
        Assert.False(functionScope.FunctionExists("func1"));
    }

    [Fact(DisplayName = "aggregating another scope squashes it on top of the current one")]
    public void aggregating_another_scope_squashes_it_on_top_of_the_current_one()
    {
        var func1 = new FunctionElement() { Parameters = new List<FunctionParameter>() };
        var functionScope1 = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        functionScope1.DefineFunction("func1", func1);

        var func2 = new FunctionElement() { Parameters = new List<FunctionParameter>() };
        var functionScope2 = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        functionScope2.DefineFunction("func2", func2);

        var functionScope = functionScope1.Aggregate(functionScope2);
        Assert.True(functionScope.FunctionExists("func1"));
        Assert.True(functionScope.FunctionExists("func2"));
    }

    [Fact(DisplayName = "aggregating two scopes that have the same method name with different parameter order keeps both definitions")]
    public void aggregating_two_scopes_that_have_the_same_method_name_with_different_parameter_order_keeps_both_definitions()
    {
        var func1 = new FunctionElement() { Parameters = new List<FunctionParameter>() };
        var functionScope1 = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        functionScope1.DefineFunction("func", func1);

        var func2 = new FunctionElement() { Parameters = new List<FunctionParameter>() { new() { Type = DataType.String() } } };
        var functionScope2 = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        functionScope2.DefineFunction("func", func2);

        var functionScope = functionScope1.Aggregate(functionScope2);
        Assert.True(functionScope.FunctionExists("func"));
        Assert.True(functionScope.FunctionExists("func", DataType.String()));
    }

    [Fact(DisplayName = "aggregating two scopes that have the same method keeps the second one")]
    public void aggregating_two_scopes_that_have_the_same_method_keeps_the_second_one()
    {
        var func1 = new FunctionElement() { Parameters = new List<FunctionParameter>() };
        var functionScope1 = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        functionScope1.DefineFunction("func", func1);

        var func2 = new FunctionElement() { Parameters = new List<FunctionParameter>() };
        var functionScope2 = new FunctionScope(new Dictionary<string, List<FunctionElement>>());
        functionScope2.DefineFunction("func", func2);

        var functionScope = functionScope1.Aggregate(functionScope2);
        Assert.Equal(func2, functionScope.GetFunction("func"));
    }
}