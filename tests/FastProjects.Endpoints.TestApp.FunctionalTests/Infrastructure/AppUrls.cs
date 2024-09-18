namespace FastProjects.Endpoints.TestApp.FunctionalTests.Infrastructure;

internal static class AppUrls
{
    private const string ProjectsRoute = "/projects";
    private const string ProjectIdParam = "{projectId}";

    public const string ListProjects = $"{ProjectsRoute}";
    public const string CreateProject = $"{ProjectsRoute}";
    public const string DeleteProjects = $"{ProjectsRoute}";
    public const string GetProjectById = $"{ProjectsRoute}/{ProjectIdParam}";

    public static string WithProjectId(this string route, Guid projectId) =>
        route.Replace(ProjectIdParam, projectId.ToString());
}
