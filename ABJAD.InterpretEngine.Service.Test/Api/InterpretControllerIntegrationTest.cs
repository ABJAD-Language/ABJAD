﻿using System.Net;
using System.Net.Mime;
using System.Text;
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
        await ValidateRequest("print.json", "print.txt");
    }

    [Fact(DisplayName = "interprets class instantiation")]
    public async Task interprets_class_instantiation()
    {
        await ValidateRequest("class_instantiation.json", "class_instantiation.txt");
    }

    [Fact(DisplayName = "interprets changing value from a global scope")]
    public async Task interprets_changing_value_from_a_global_scope()
    {
        await ValidateRequest("scopes.json", "scopes.txt");
    }

    [Fact(DisplayName = "references are always passed by value to the method calls")]
    public async Task references_are_always_passed_by_value_to_the_method_calls()
    {
        await ValidateRequest("references.json", "references.txt");

    }

    private async Task ValidateRequest(string requestJsonFileName, string responseTextFileName)
    {
        var request = ReadFile("Requests/" + requestJsonFileName);
        var body = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
        var response = await client.PostAsync("/", body);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(ReadFile("Responses/" + responseTextFileName), content);
    }

    private static string ReadFile(string fileName)
    {
        return File.ReadAllText("../../../Api/" + fileName);
    }
}