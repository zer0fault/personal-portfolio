using Application.Projects.Queries.GetAllProjects;
using Application.Projects.Queries.GetProjectById;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Functions.API.Functions;

/// <summary>
/// HTTP trigger functions for Projects endpoints
/// </summary>
public class ProjectsFunctions
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProjectsFunctions> _logger;

    public ProjectsFunctions(IMediator mediator, ILogger<ProjectsFunctions> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// GET /api/projects - Get all published projects
    /// </summary>
    [Function("GetProjects")]
    public async Task<HttpResponseData> GetProjects(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "projects")] HttpRequestData req,
        FunctionContext context)
    {
        _logger.LogInformation("Getting all projects");

        try
        {
            var query = new GetAllProjectsQuery();
            var projects = await _mediator.Send(query);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(projects);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting projects");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving projects" });
            return response;
        }
    }

    /// <summary>
    /// GET /api/projects/{id} - Get project by ID
    /// </summary>
    [Function("GetProjectById")]
    public async Task<HttpResponseData> GetProjectById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "projects/{id}")] HttpRequestData req,
        FunctionContext context,
        int id)
    {
        _logger.LogInformation("Getting project {ProjectId}", id);

        try
        {
            var query = new GetProjectByIdQuery(id);
            var project = await _mediator.Send(query);

            if (project == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                await notFoundResponse.WriteAsJsonAsync(new { error = "Project not found" });
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(project);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting project {ProjectId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving the project" });
            return response;
        }
    }
}
