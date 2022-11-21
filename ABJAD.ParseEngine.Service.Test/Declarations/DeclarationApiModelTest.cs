using ABJAD.ParseEngine.Service.Declarations;
using ABJAD.ParseEngine.Service.Expressions;
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
}