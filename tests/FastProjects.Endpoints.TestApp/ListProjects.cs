using FastEndpoints;
using FastProjects.ResultPattern;
using FastProjects.SharedKernel;
using MediatR;

namespace FastProjects.Endpoints.TestApp;

// Example of a FastEndpoint without request but with response

// Application layer below implemented via MediatR

public sealed record ListProjectsQuery : IQuery<ListProjectsQueryResponse>;

public sealed record ListProjectsQueryResponse(
    List<ListProjectsQueryResponse.ProjectItem> Projects)
{
    public sealed record ProjectItem(
        Guid Id,
        string Name);
}

public sealed class ListProjectsQueryHandler : IQueryHandler<ListProjectsQuery, ListProjectsQueryResponse>
{
    public async Task<Result<ListProjectsQueryResponse>> Handle(ListProjectsQuery request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        ListProjectsQueryResponse response = new(
        [
            new ListProjectsQueryResponse.ProjectItem(Guid.NewGuid(), "Test Project 1"),
            new ListProjectsQueryResponse.ProjectItem(Guid.NewGuid(), "Test Project 2"),
            new ListProjectsQueryResponse.ProjectItem(Guid.NewGuid(), "Test Project 3")
        ]);
        return Result.Success(response);
    }
}

// Presentation layer below implemented via FastEndpoints

public sealed class ListProjectsRequest
{
    public const string Route = "/projects";

    public static string BuildRoute() => Route;
}

public sealed record ListProjectsResponse(
    List<ListProjectsResponse.ProjectItem> Projects)
{
    public sealed record ProjectItem(
        Guid Id,
        string Name);
}

public sealed class ListProjectsEndpoint(IMediator mediator)
    : FastEndpoint<
        ListProjectsResponse,
        ListProjectsQuery,
        Result<ListProjectsQueryResponse>,
        ListProjectsQueryResponse>(mediator)
{
    public override void Configure()
    {
        Get(ListProjectsRequest.Route);
        Version(0);
        AllowAnonymous();
    }

    protected override ListProjectsQuery CreateMediatorCommand(EmptyRequest request) =>
        new();

    protected override ListProjectsResponse CreateResponse(ListProjectsQueryResponse data) =>
        new(data.Projects.Select(x => new ListProjectsResponse.ProjectItem(x.Id, x.Name)).ToList());
}
