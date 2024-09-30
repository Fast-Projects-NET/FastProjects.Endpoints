# 🚀 **FastProjects.Endpoints**

![Build Status](https://github.com/Fast-Projects-NET/FastProjects.Endpoints/actions/workflows/test.yml/badge.svg)
![NuGet](https://img.shields.io/nuget/v/FastProjects.Endpoints.svg)
![NuGet Downloads](https://img.shields.io/nuget/dt/FastProjects.Endpoints.svg)
![License](https://img.shields.io/github/license/Fast-Projects-NET/FastProjects.Endpoints.svg)
![Last Commit](https://img.shields.io/github/last-commit/Fast-Projects-NET/FastProjects.Endpoints.svg)
![GitHub Stars](https://img.shields.io/github/stars/Fast-Projects-NET/FastProjects.Endpoints.svg)
![GitHub Forks](https://img.shields.io/github/forks/Fast-Projects-NET/FastProjects.Endpoints.svg)

> 🚨 ALERT: Project Under Development
> This project is not yet production-ready and is still under active development. Currently, it's being used primarily for personal development needs. However, contributions are more than welcome! If you'd like to collaborate, feel free to submit issues or pull requests. Your input can help shape the future of FastProjects!

---

## 📚 **Overview**

Collection of endpoint base classes that wraps [FastEndpoints](https://fast-endpoints.com/) classes and integrated with [MediatR](https://github.com/jbogard/MediatR) and [Result](https://github.com/Fast-Projects-NET/FastProjects.ResultPattern) patterns.

Complete documentation of the [FastEndpoints](https://fast-endpoints.com/) can be found [here](https://fast-endpoints.com/docs).

---

## 🛠 **Roadmap**

- ✅ [FastEndpoint](src/FastProjects.Endpoints/FastEndpoint.cs) - Base class for endpoints that uses MediatR and Result patterns with response body
- ✅ [FastEndpointWithoutResponse](src/FastProjects.Endpoints/FastEndpointWithoutResponse.cs) - Base class for endpoints that uses MediatR and Result patterns without response body

---

## 🚀 **Installation**

You can download the NuGet package using the following command to install:
```bash
dotnet add package FastProjects.Endpoints
```

## Usage

Example of dummy create project use-case implementation:
```csharp
// Application layer below implemented via MediatR
public sealed record CreateProjectCommand(string Name) : ICommand;
    
public sealed class CreateProjectCommandHandler : SharedKernel.ICommandHandler<CreateProjectCommand>
{
    public async Task<Result> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return Result.Success();
    }
}

// Presentation layer below implemented via FastEndpoints

public sealed class CreateProjectRequest
{
    public const string Route = "/projects";
    
    public static string BuildRoute() => Route;
    
    public string Name { get; set; }
}

public sealed class CreateProjectValidator : Validator<CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public sealed class CreateProjectEndpoint(IMediator mediator)
    : FastEndpointWithoutResponse<CreateProjectRequest,
        CreateProjectCommand,
        Result>(mediator)
{
    public override void Configure()
    {
        Post(CreateProjectRequest.Route);
        Version(0);
        AllowAnonymous();
        
        Summary(s =>
        {
            s.ExampleRequest = new CreateProjectRequest { Name = $"Test Project" };
        });
    }
    
    protected override CreateProjectCommand CreateMediatorCommand(CreateProjectRequest request) =>
        new(request.Name);
}

```

More example can be found in sample [TestApp](./tests/FastProjects.Endpoints.TestApp) project.

---

## 🤝 **Contributing**

This project is still under development, but contributions are welcome! Whether you’re opening issues, submitting pull requests, or suggesting new features, we appreciate your involvement. For more details, please check the [contribution guide](CONTRIBUTING.md). Let’s build something amazing together! 🎉

---

## 📄 **License**

FastProjects is licensed under the **MIT License**. See the [LICENSE](LICENSE) file for full details.
