using System.Net;
using FastProjects.Endpoints.TestApp.FunctionalTests.Infrastructure;
using FluentAssertions;

namespace FastProjects.Endpoints.TestApp.FunctionalTests.Projects;

public sealed class DeleteProjectsTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task DeleteProject_Should_ReturnNoContentStatus()
    {
        // Arrange
        
        // Act
        HttpResponseMessage response = await Client.DeleteAsync(AppUrls.DeleteProjects);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Fact]
    public async Task DeleteProject_ShouldNot_ReturnBodyResponse()
    {
        // Arrange
        
        // Act
        HttpResponseMessage response = await Client.DeleteAsync(AppUrls.DeleteProjects);
        string responseContent = await response.Content.ReadAsStringAsync();
        
        // Assert
        responseContent.Should().BeNullOrEmpty();
    }
}
