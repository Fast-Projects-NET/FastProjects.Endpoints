using FastEndpoints;
using FastProjects.ResultPattern;
using FastProjects.SharedKernel;
using FluentValidation;
using MediatR;

namespace FastProjects.Endpoints.TestApp;

// Example of a FastEndpoint with request and response

// Application layer below implemented via MediatR
public sealed record GetProjectByIdQuery(Guid ProjectId)
    : IQuery<GetProjectByIdQueryResponse>;

public sealed record GetProjectByIdQueryResponse(
    Guid Id,
    string Name);

public sealed class GetProjectByIdQueryHandler : IQueryHandler<GetProjectByIdQuery, GetProjectByIdQueryResponse>
{
    public async Task<Result<GetProjectByIdQueryResponse>> Handle(GetProjectByIdQuery request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        GetProjectByIdQueryResponse response = new(request.ProjectId, $"Test Project {request.ProjectId}");
        return Result.Success(response);
    }
}

// Presentation layer below implemented via FastEndpoints 
public sealed class GetProjectByIdRequest
{
    public const string Route = "/projects/{ProjectId:guid}";

    public static string BuildRoute(Guid projectId) =>
        Route.Replace("{ProjectId:guid}", projectId.ToString());

    public Guid ProjectId { get; set; }
}

public sealed record GetProjectByIdResponse(
    Guid Id,
    string Name);

public sealed class GetProjectByIdValidator : Validator<GetProjectByIdRequest>
{
    public GetProjectByIdValidator()
    {
        RuleFor(x => x.ProjectId).NotEmpty();
    }
}

public sealed class GetProjectByIdEndpoint(IMediator mediator)
    : FastEndpoint<GetProjectByIdRequest,
        GetProjectByIdResponse,
        GetProjectByIdQuery,
        Result<GetProjectByIdQueryResponse>,
        GetProjectByIdQueryResponse>(mediator)
{
    public override void Configure()
    {
        Get(GetProjectByIdRequest.Route);
        Version(0);
        AllowAnonymous();
        
        Summary(s =>
        {
            var id = Guid.NewGuid();
            s.ExampleRequest = new GetProjectByIdRequest { ProjectId = id };
            s.ResponseExamples[200] = new GetProjectByIdResponse(id, $"Test Project {id}");
        });
    }
    
    protected override GetProjectByIdQuery CreateMediatorCommand(GetProjectByIdRequest request) =>
        new(request.ProjectId);

    protected override GetProjectByIdResponse CreateResponse(GetProjectByIdQueryResponse data) =>
        new(data.Id, data.Name);
}
