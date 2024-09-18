using Microsoft.AspNetCore.Mvc.Testing;

namespace FastProjects.Endpoints.TestApp.FunctionalTests.Infrastructure;

public class FunctionalTestWebAppFactory: WebApplicationFactory<Program>, IAsyncLifetime
{
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }
}
