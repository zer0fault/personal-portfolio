using Application.Skills.Queries.GetAllSkills;
using Application.Skills.Queries.GetSkillByIdForAdmin;
using Application.Skills.Commands.CreateSkill;
using Application.Skills.Commands.UpdateSkill;
using Application.Skills.Commands.DeleteSkill;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Functions.API.Functions;

/// <summary>
/// HTTP trigger functions for Skills endpoints
/// </summary>
public class SkillsFunctions
{
    private readonly IMediator _mediator;
    private readonly ILogger<SkillsFunctions> _logger;

    public SkillsFunctions(IMediator mediator, ILogger<SkillsFunctions> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// GET /api/skills - Get all skills
    /// POST /api/skills - Create a new skill
    /// </summary>
    [Function("Skills")]
    public async Task<HttpResponseData> Skills(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "options", Route = "skills")] HttpRequestData req,
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
            return await GetAllSkills(req);
        }
        else if (req.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            return await CreateSkill(req);
        }

        var badResponse = req.CreateResponse(HttpStatusCode.MethodNotAllowed);
        badResponse.Headers.Add("Access-Control-Allow-Origin", "*");
        return badResponse;
    }

    /// <summary>
    /// GET /api/skills/{id} - Get skill by ID for admin
    /// PUT /api/skills/{id} - Update a skill
    /// DELETE /api/skills/{id} - Delete a skill
    /// </summary>
    [Function("SkillsById")]
    public async Task<HttpResponseData> SkillsById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "put", "delete", "options", Route = "skills/{id}")] HttpRequestData req,
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
            return await GetSkillByIdForAdmin(req, id);
        }
        else if (req.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
        {
            return await UpdateSkill(req, id);
        }
        else if (req.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
        {
            return await DeleteSkill(req, id);
        }

        var badResponse = req.CreateResponse(HttpStatusCode.MethodNotAllowed);
        badResponse.Headers.Add("Access-Control-Allow-Origin", "*");
        return badResponse;
    }

    private async Task<HttpResponseData> GetAllSkills(HttpRequestData req)
    {
        _logger.LogInformation("Getting all skills");

        try
        {
            var query = new GetAllSkillsQuery();
            var skills = await _mediator.Send(query);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(skills);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skills");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving skills" });
            return response;
        }
    }

    private async Task<HttpResponseData> GetSkillByIdForAdmin(HttpRequestData req, int id)
    {
        _logger.LogInformation("Getting skill {SkillId} for admin", id);

        try
        {
            var query = new GetSkillByIdForAdminQuery(id);
            var skill = await _mediator.Send(query);

            if (skill == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await notFoundResponse.WriteAsJsonAsync(new { error = "Skill not found" });
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(skill);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting skill {SkillId} for admin", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving the skill" });
            return response;
        }
    }

    private async Task<HttpResponseData> CreateSkill(HttpRequestData req)
    {
        _logger.LogInformation("Creating new skill");

        try
        {
            var command = await req.ReadFromJsonAsync<CreateSkillCommand>();
            if (command == null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badRequestResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await badRequestResponse.WriteAsJsonAsync(new { error = "Invalid request body" });
                return badRequestResponse;
            }

            var skillId = await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { id = skillId });
            return response;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error creating skill");
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await errorResponse.WriteAsJsonAsync(new { error = ex.Message });
            return errorResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating skill");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while creating the skill" });
            return response;
        }
    }

    private async Task<HttpResponseData> UpdateSkill(HttpRequestData req, int id)
    {
        _logger.LogInformation("Updating skill {SkillId}", id);

        try
        {
            var command = await req.ReadFromJsonAsync<UpdateSkillCommand>();
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
            _logger.LogWarning(ex, "Validation error updating skill {SkillId}", id);
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await errorResponse.WriteAsJsonAsync(new { error = ex.Message });
            return errorResponse;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Skill {SkillId} not found", id);
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await notFoundResponse.WriteAsJsonAsync(new { error = ex.Message });
            return notFoundResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating skill {SkillId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while updating the skill" });
            return response;
        }
    }

    private async Task<HttpResponseData> DeleteSkill(HttpRequestData req, int id)
    {
        _logger.LogInformation("Deleting skill {SkillId}", id);

        try
        {
            var command = new DeleteSkillCommand { Id = id };
            await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { success = true });
            return response;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Skill {SkillId} not found", id);
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await notFoundResponse.WriteAsJsonAsync(new { error = ex.Message });
            return notFoundResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting skill {SkillId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while deleting the skill" });
            return response;
        }
    }
}
