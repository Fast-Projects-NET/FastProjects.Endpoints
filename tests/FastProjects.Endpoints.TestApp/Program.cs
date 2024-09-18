using FastEndpoints;
using FastEndpoints.Swagger;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.DocumentName = "Initial Release";
        s.Title = "FastProjects.Endpoints Test API";
        s.Description = "API to test the FastProjects.Endpoints library";
        s.Version = "v0";
    };
});
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

WebApplication app = builder.Build();

app.MapFastEndpoints();
app.UseSwaggerGen();

app.Run();

public partial class Program;
