using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FastProjects.Endpoints.TestApp.FunctionalTests.Infrastructure;

public abstract class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
{
    protected readonly HttpClient Client;
    
    protected BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {
        IServiceScope scope = factory.Services.CreateScope();
        
        Uri applicationUrl = factory.Server.BaseAddress;

        Client = factory.CreateClient();
        Client.BaseAddress = applicationUrl;
    }
    
    protected static JsonSerializerSettings GetJsonSerializerSettings() => new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        }
    };
}
