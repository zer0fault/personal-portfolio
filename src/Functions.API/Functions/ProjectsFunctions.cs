using Application.Projects.Queries.GetAllProjects;
using Application.Projects.Queries.GetAllProjectsForAdmin;
using Application.Projects.Queries.GetProjectById;
using Application.Projects.Queries.GetProjectByIdForAdmin;
using Application.Projects.Commands.CreateProject;
using Application.Projects.Commands.UpdateProject;
using Application.Projects.Commands.DeleteProject;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

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
    /// POST /api/projects - Create a new project
    /// </summary>
    [Function("Projects")]
    public async Task<HttpResponseData> Projects(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "options", Route = "projects")] HttpRequestData req,
        FunctionContext context)
    {
        // Handle OPTIONS preflight request
        if (req.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            var preflightResponse = req.CreateResponse(HttpStatusCode.OK);
            preflightResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            preflightResponse.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            preflightResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            return preflightResponse;
        }

        if (req.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            return await GetAllProjects(req);
        }
        else if (req.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            return await CreateProject(req);
        }

        var badResponse = req.CreateResponse(HttpStatusCode.MethodNotAllowed);
        badResponse.Headers.Add("Access-Control-Allow-Origin", "*");
        return badResponse;
    }

    /// <summary>
    /// GET /api/projects-admin - Get all projects for admin (includes all statuses)
    /// </summary>
    [Function("ProjectsAdmin")]
    public async Task<HttpResponseData> ProjectsAdmin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "projects-admin")] HttpRequestData req,
        FunctionContext context)
    {
        // Handle OPTIONS preflight request
        if (req.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            var preflightResponse = req.CreateResponse(HttpStatusCode.OK);
            preflightResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            preflightResponse.Headers.Add("Access-Control-Allow-Methods", "GET, OPTIONS");
            preflightResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            return preflightResponse;
        }

        return await GetAllProjectsForAdmin(req);
    }

    /// <summary>
    /// GET /api/projects-admin/{id} - Get project by ID for admin (any status)
    /// </summary>
    [Function("ProjectByIdAdmin")]
    public async Task<HttpResponseData> ProjectByIdAdmin(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "projects-admin/{id}")] HttpRequestData req,
        FunctionContext context,
        int id)
    {
        // Handle OPTIONS preflight request
        if (req.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            var preflightResponse = req.CreateResponse(HttpStatusCode.OK);
            preflightResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            preflightResponse.Headers.Add("Access-Control-Allow-Methods", "GET, OPTIONS");
            preflightResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            return preflightResponse;
        }

        return await GetProjectByIdForAdmin(req, id);
    }

    /// <summary>
    /// GET /api/projects/{id} - Get project by ID
    /// PUT /api/projects/{id} - Update an existing project
    /// DELETE /api/projects/{id} - Delete a project
    /// </summary>
    [Function("ProjectById")]
    public async Task<HttpResponseData> ProjectById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "put", "delete", "options", Route = "projects/{id}")] HttpRequestData req,
        FunctionContext context,
        int id)
    {
        // Handle OPTIONS preflight request
        if (req.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            var preflightResponse = req.CreateResponse(HttpStatusCode.OK);
            preflightResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            preflightResponse.Headers.Add("Access-Control-Allow-Methods", "GET, PUT, DELETE, OPTIONS");
            preflightResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            return preflightResponse;
        }

        if (req.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            return await GetProjectById(req, id);
        }
        else if (req.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
        {
            return await UpdateProject(req, id);
        }
        else if (req.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
        {
            return await DeleteProject(req, id);
        }

        var badResponse = req.CreateResponse(HttpStatusCode.MethodNotAllowed);
        badResponse.Headers.Add("Access-Control-Allow-Origin", "*");
        return badResponse;
    }

    private async Task<HttpResponseData> GetAllProjects(HttpRequestData req)
    {
        _logger.LogInformation("Getting all published projects");

        try
        {
            var query = new GetAllProjectsQuery();
            var projects = await _mediator.Send(query);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(projects);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting projects");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving projects" });
            return response;
        }
    }

    private async Task<HttpResponseData> GetAllProjectsForAdmin(HttpRequestData req)
    {
        _logger.LogInformation("Getting all projects for admin");

        try
        {
            var query = new GetAllProjectsForAdminQuery();
            var projects = await _mediator.Send(query);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(projects);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting projects for admin");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving projects" });
            return response;
        }
    }

    private async Task<HttpResponseData> GetProjectById(HttpRequestData req, int id)
    {
        _logger.LogInformation("Getting project {ProjectId}", id);

        try
        {
            var query = new GetProjectByIdQuery(id);
            var project = await _mediator.Send(query);

            if (project == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await notFoundResponse.WriteAsJsonAsync(new { error = "Project not found" });
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(project);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting project {ProjectId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving the project" });
            return response;
        }
    }

    private async Task<HttpResponseData> GetProjectByIdForAdmin(HttpRequestData req, int id)
    {
        _logger.LogInformation("Getting project {ProjectId} for admin", id);

        try
        {
            var query = new GetProjectByIdForAdminQuery(id);
            var project = await _mediator.Send(query);

            if (project == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await notFoundResponse.WriteAsJsonAsync(new { error = "Project not found" });
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(project);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting project {ProjectId} for admin", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving the project" });
            return response;
        }
    }

    private async Task<HttpResponseData> CreateProject(HttpRequestData req)
    {
        _logger.LogInformation("Creating new project");

        try
        {
            var command = await req.ReadFromJsonAsync<CreateProjectCommand>();
            if (command == null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badRequestResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await badRequestResponse.WriteAsJsonAsync(new { error = "Invalid request body" });
                return badRequestResponse;
            }

            var projectId = await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { id = projectId });
            return response;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error creating project");
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await errorResponse.WriteAsJsonAsync(new { error = ex.Message });
            return errorResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while creating the project" });
            return response;
        }
    }

    private async Task<HttpResponseData> UpdateProject(HttpRequestData req, int id)
    {
        _logger.LogInformation("Updating project {ProjectId}", id);

        try
        {
            var command = await req.ReadFromJsonAsync<UpdateProjectCommand>();
            if (command == null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badRequestResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await badRequestResponse.WriteAsJsonAsync(new { error = "Invalid request body" });
                return badRequestResponse;
            }

            command.Id = id;
            await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { success = true });
            return response;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error updating project {ProjectId}", id);
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await errorResponse.WriteAsJsonAsync(new { error = ex.Message });
            return errorResponse;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Project {ProjectId} not found", id);
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await notFoundResponse.WriteAsJsonAsync(new { error = ex.Message });
            return notFoundResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating project {ProjectId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while updating the project" });
            return response;
        }
    }

    private async Task<HttpResponseData> DeleteProject(HttpRequestData req, int id)
    {
        _logger.LogInformation("Deleting project {ProjectId}", id);

        try
        {
            var command = new DeleteProjectCommand { Id = id };
            await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { success = true });
            return response;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Project {ProjectId} not found", id);
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await notFoundResponse.WriteAsJsonAsync(new { error = ex.Message });
            return notFoundResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting project {ProjectId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while deleting the project" });
            return response;
        }
    }
}
