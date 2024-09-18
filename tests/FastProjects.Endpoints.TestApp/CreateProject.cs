using FastEndpoints;
using FastProjects.ResultPattern;
using FluentValidation;
using MediatR;
using ICommand = FastProjects.SharedKernel.ICommand;

namespace FastProjects.Endpoints.TestApp;

// Example of a FastEndpoint with request but without response

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
