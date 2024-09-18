using System.Net;
using FastProjects.Endpoints.TestApp.FunctionalTests.Infrastructure;
using FluentAssertions;
using Newtonsoft.Json;

namespace FastProjects.Endpoints.TestApp.FunctionalTests.Projects;

public sealed class GetProjectByIdTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task GetProjectById_Should_ReturnOkStatus()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        
        // Act
        HttpResponseMessage response = await Client.GetAsync(AppUrls.GetProjectById.WithProjectId(projectId));
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetProjectById_Should_ReturnBodyResponse()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        // Act
        HttpResponseMessage response = await Client.GetAsync(AppUrls.GetProjectById.WithProjectId(projectId));
        string responseContent = await response.Content.ReadAsStringAsync();

        GetProjectByIdResponse responseModel = JsonConvert.DeserializeObject<GetProjectByIdResponse>(responseContent, GetJsonSerializerSettings());

        // Assert
        responseContent.Should().NotBeNullOrEmpty();
        responseModel.Should().NotBeNull();
        responseModel!.Id.Should().Be(projectId);
    }
}
