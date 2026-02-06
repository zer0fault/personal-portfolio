using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Functions.API.Functions;

/// <summary>
/// HTTP trigger functions for health check endpoints
/// </summary>
public class HealthFunctions
{
    private readonly ILogger<HealthFunctions> _logger;

    public HealthFunctions(ILogger<HealthFunctions> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// GET /api/health - Basic health check endpoint
    /// </summary>
    [Function("Health")]
    public async Task<HttpResponseData> Health(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "health")] HttpRequestData req,
        FunctionContext context)
    {
        _logger.LogInformation("Health check requested");

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Access-Control-Allow-Origin", "*");
        response.Headers.Add("Content-Type", "application/json");

        await response.WriteAsJsonAsync(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow,
            service = "Portfolio API",
            version = "1.0.0"
        });

        return response;
    }
}
