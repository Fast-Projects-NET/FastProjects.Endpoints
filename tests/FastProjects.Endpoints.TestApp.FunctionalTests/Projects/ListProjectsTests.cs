using System.Net;
using FastProjects.Endpoints.TestApp.FunctionalTests.Infrastructure;
using FluentAssertions;
using Newtonsoft.Json;

namespace FastProjects.Endpoints.TestApp.FunctionalTests.Projects;

public sealed class ListProjectsTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task ListProjects_Should_ReturnOkStatus()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await Client.GetAsync(AppUrls.ListProjects);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ListProjects_Should_ReturnBodyResponse()
    {
        // Arrange
        // Act
        HttpResponseMessage response = await Client.GetAsync(AppUrls.ListProjects);
        string responseContent = await response.Content.ReadAsStringAsync();

        ListProjectsResponse responseModel = JsonConvert.DeserializeObject<ListProjectsResponse>(responseContent, GetJsonSerializerSettings());

        // Assert
        responseContent.Should().NotBeNullOrEmpty();
        responseModel.Should().NotBeNull();
        responseModel!.Projects.Should().NotBeEmpty();
    }
}
