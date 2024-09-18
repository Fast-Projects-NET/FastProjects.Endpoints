using System.Net;
using System.Text;
using System.Text.Json;
using FastProjects.Endpoints.TestApp.FunctionalTests.Infrastructure;
using FluentAssertions;

namespace FastProjects.Endpoints.TestApp.FunctionalTests.Projects;

public sealed class CreateProjectTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task CreateProject_Should_ReturnNoContentStatus()
    {
        // Arrange
        const string testProjectName = "Test Project";
        var createProjectRequest = new CreateProjectRequest { Name = testProjectName };
        var requestContent = new StringContent(JsonSerializer.Serialize(createProjectRequest), Encoding.UTF8, "application/json");

        // Act
        HttpResponseMessage response = await Client.PostAsync(AppUrls.CreateProject, requestContent);
        response.EnsureSuccessStatusCode();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task CreateProject_Should_ReturnValidationError_WhenNameIsEmpty()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest { Name = string.Empty };
        var requestContent = new StringContent(JsonSerializer.Serialize(createProjectRequest), Encoding.UTF8, "application/json");

        // Act
        HttpResponseMessage response = await Client.PostAsync(AppUrls.CreateProject, requestContent);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateProject_ShouldNot_ReturnBodyResponse()
    {
        // Arrange
        const string testProjectName = "Test Project";
        var createProjectRequest = new CreateProjectRequest { Name = testProjectName };
        var requestContent = new StringContent(JsonSerializer.Serialize(createProjectRequest), Encoding.UTF8, "application/json");

        // Act
        HttpResponseMessage response = await Client.PostAsync(AppUrls.CreateProject, requestContent);
        response.EnsureSuccessStatusCode();
        string responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        responseContent.Should().BeNullOrEmpty();
    }
}
