using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ABJAD.InterpretEngine.Service.Test.Api;

public class InterpretControllerIntegrationTest
{
    private readonly HttpClient client;

    public InterpretControllerIntegrationTest()
    {
        var applicationFactory = new WebApplicationFactory<Program>().WithWebHostBuilder(_ => { });
        client = applicationFactory.CreateClient();
    }

    [Fact(DisplayName = "interprets print statement")]
    public async Task interprets_print_statement()
    {
        await ValidateRequest("print.json", "print.json");
    }

    [Fact(DisplayName = "interprets class instantiation")]
    public async Task interprets_class_instantiation()
    {
        await ValidateRequest("class_instantiation.json", "class_instantiation.json");
    }

    [Fact(DisplayName = "interprets changing value from a global scope")]
    public async Task interprets_changing_value_from_a_global_scope()
    {
        await ValidateRequest("scopes.json", "scopes.json");
    }

    [Fact(DisplayName = "references are always passed by value to the method calls")]
    public async Task references_are_always_passed_by_value_to_the_method_calls()
    {
        await ValidateRequest("references.json", "references.json");
    }

    [Fact(DisplayName = "runs nested for loops as expected")]
    public async Task runs_nested_for_loops_as_expected()
    {
        await ValidateRequest("if_without_else.json", "if_without_else.json");
    }

    [Fact(DisplayName = "test methods calling each other", Skip = "will be fixed soon")]
    public async Task test_methods_calling_each_other()
    {
        await ValidateRequest("methods_reference.json", "methods_reference.json");
    }

    private async Task ValidateRequest(string requestJsonFileName, string responseTextFileName)
    {
        var request = ReadFile("Requests/" + requestJsonFileName);
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/", body);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(RemoveWhiteSpaces(ReadFile("Responses/" + responseTextFileName)), content.Replace("\\r", ""));
    }

    private static string ReadFile(string fileName)
    {
        return File.ReadAllText("../../../Api/" + fileName);
    }

    private static string RemoveWhiteSpaces(string content)
    {
        return Regex.Replace(content, "(\"(?:[^\"\\\\]|\\\\.)*\")|\\s+", "$1");
    }
}