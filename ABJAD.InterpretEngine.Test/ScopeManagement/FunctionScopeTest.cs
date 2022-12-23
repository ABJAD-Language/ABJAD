using ABJAD.InterpretEngine.ScopeManagement;
using ABJAD.InterpretEngine.Types;
using NSubstitute;

namespace ABJAD.InterpretEngine.Test.ScopeManagement;

public class FunctionScopeTest
{
    [Fact(DisplayName = "returns false when asked about a function that does not exist")]
    public void returns_false_when_asked_about_a_function_that_does_not_exist()
    {
        var functionScope = new FunctionScope(new Dictionary<(string, int), FunctionElement>());
        Assert.False(functionScope.FunctionExists("func", 0));
    }

    [Fact(DisplayName = "returns false when asked about a function that does not exist with same number of parameters")]
    public void returns_false_when_asked_about_a_function_that_does_not_exist_with_same_number_of_parameters()
    {
        var state = new Dictionary<(string, int), FunctionElement>();
        state.Add(("func", 0), new FunctionElement());
        var functionScope = new FunctionScope(state);
        Assert.False(functionScope.FunctionExists("func", 1));
    }

    [Fact(DisplayName = "returns true when asked about a function that exists with same number of parameters")]
    public void returns_true_when_asked_about_a_function_that_exists_with_same_number_of_parameters()
    {
        var state = new Dictionary<(string, int), FunctionElement>();
        state.Add(("func", 0), new FunctionElement());
        var functionScope = new FunctionScope(state);
        Assert.True(functionScope.FunctionExists("func", 0));
    }

    [Fact(DisplayName = "returns the specified function return type when asked about")]
    public void returns_the_specified_function_return_type_when_asked_about()
    {
        var state = new Dictionary<(string, int), FunctionElement>();
        var firstFuncReturnType = Substitute.For<DataType>();
        var secondFuncReturnType = Substitute.For<DataType>();
        state.Add(("func", 0), new FunctionElement { ReturnType = firstFuncReturnType });
        state.Add(("func", 1), new FunctionElement { ReturnType = secondFuncReturnType });
        var functionScope = new FunctionScope(state);
        Assert.Equal(secondFuncReturnType, functionScope.GetFunctionReturnType("func", 1));
    }

    [Fact(DisplayName = "returns the specified function when asked about")]
    public void returns_the_specified_function_when_asked_about()
    {
        var state = new Dictionary<(string, int), FunctionElement>();
        var firstFunction = new FunctionElement();
        var secondFunction = new FunctionElement();
        state.Add(("func", 0), firstFunction);
        state.Add(("func", 1), secondFunction);
        var functionScope = new FunctionScope(state);
        Assert.Equal(firstFunction, functionScope.GetFunction("func", 0));
    }

    [Fact(DisplayName = "defining a new function adds it to the state")]
    public void defining_a_new_function_adds_it_to_the_state()
    {
        var functionScope = new FunctionScope(new Dictionary<(string, int), FunctionElement>());
        var function = new FunctionElement() { Parameters = new List<FunctionParameter> { new() }};
        functionScope.DefineFunction("func", function);
        Assert.Equal(function, functionScope.GetFunction("func", 1));
    }

    [Fact(DisplayName = "cloning the scope creates a deep copy of it")]
    public void cloning_the_scope_creates_a_deep_copy_of_it()
    {
        var state = new Dictionary<(string, int), FunctionElement>();
        var functionScope = new FunctionScope(state);
        var cloneFunctionScope = functionScope.Clone();
        functionScope.DefineFunction("func", new FunctionElement { Parameters = new List<FunctionParameter> { new() }});
        Assert.False(cloneFunctionScope.FunctionExists("func", 1));
        cloneFunctionScope.DefineFunction("func1", new FunctionElement { Parameters = new List<FunctionParameter>() });
        Assert.False(functionScope.FunctionExists("func1", 0));
    }
}