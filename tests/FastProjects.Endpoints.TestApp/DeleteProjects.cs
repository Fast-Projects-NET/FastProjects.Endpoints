using FastEndpoints;
using FastProjects.ResultPattern;
using MediatR;
using ICommand = FastProjects.SharedKernel.ICommand;

namespace FastProjects.Endpoints.TestApp;

// Example of a FastEndpoint without request and response

// Application layer below implemented via MediatR

public sealed record DeleteProjectsCommand : ICommand;

public sealed class DeleteProjectsCommandHandler : SharedKernel.ICommandHandler<DeleteProjectsCommand>
{
    public async Task<Result> Handle(DeleteProjectsCommand request, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return Result.Success();
    }
}

// Presentation layer below implemented via FastEndpoints

public sealed class DeleteProjectsRequest
{
    public const string Route = "/projects";
    
    public static string BuildRoute() => Route;
}

public sealed class DeleteProjectsEndpoint(IMediator mediator)
    : FastEndpointWithoutResponse<
        DeleteProjectsCommand,
        Result>(mediator)
{
    public override void Configure()
    {
        Delete(DeleteProjectsRequest.Route);
        Version(0);
        AllowAnonymous();
        
        Summary(s =>
        {
            s.ExampleRequest = new DeleteProjectsRequest();
        });
    }

    protected override DeleteProjectsCommand CreateMediatorCommand(EmptyRequest request) =>
        new();
}
