using Application.Skills.Queries.GetAllSkills;
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
    /// </summary>
    [Function("GetSkills")]
    public async Task<HttpResponseData> GetSkills(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "skills")] HttpRequestData req,
        FunctionContext context)
    {
        // Handle OPTIONS preflight request
        if (req.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            var preflightResponse = req.CreateResponse(HttpStatusCode.OK);
            preflightResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            preflightResponse.Headers.Add("Access-Control-Allow-Methods", "GET, OPTIONS");
            preflightResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            return preflightResponse;
        }

        _logger.LogInformation("Getting all skills");

        try
        {
            var query = new GetAllSkillsQuery();
            var skills = await _mediator.Send(query);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
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
}
