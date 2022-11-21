using ABJAD.ParseEngine.Service.Bindings;
using ABJAD.ParseEngine.Service.Declarations;
using ABJAD.ParseEngine.Service.Expressions;
using ABJAD.ParseEngine.Service.Statements;
using Moq;

namespace ABJAD.ParseEngine.Service.Test.Declarations;

public class DeclarationApiModelTest
{
    [Fact(DisplayName = "constant declaration returns correct type")]
    public void constant_declaration_returns_correct_type()
    {
        Assert.Equal("declaration.constant", new ConstantDeclarationApiModel("", "", new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "variable declaration returns correct type")]
    public void variable_declaration_returns_correct_type()
    {
        Assert.Equal("declaration.variable", new VariableDeclarationApiModel("", "", new Mock<ExpressionApiModel>().Object).Type);
    }

    [Fact(DisplayName = "class declaration returns correct type")]
    public void class_declaration_returns_correct_type()
    {
        Assert.Equal("declaration.class", new ClassDeclarationApiModel("", new BlockStatementApiModel(new List<BindingApiModel>())).Type);
    }

    [Fact(DisplayName = "function declaration returns correct type")]
    public void function_declaration_returns_correct_type()
    {
        Assert.Equal("declaration.function",
            new FunctionDeclarationApiModel("func", "string", new List<FunctionParameterApiModel>(),
                new BlockStatementApiModel(new List<BindingApiModel>())).Type);
    }

    [Fact(DisplayName = "constructor declaration returns correct type")]
    public void constructor_declaration_returns_correct_type()
    {
        Assert.Equal("declaration.constructor",
            new ConstructorDeclarationApiModel(new List<FunctionParameterApiModel>(),
                new BlockStatementApiModel(new List<BindingApiModel>())).Type);
    }
}