using Application.Employment.Queries.GetAllEmployment;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Functions.API.Functions;

/// <summary>
/// HTTP trigger functions for Employment endpoints
/// </summary>
public class EmploymentFunctions
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmploymentFunctions> _logger;

    public EmploymentFunctions(IMediator mediator, ILogger<EmploymentFunctions> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// GET /api/employment - Get all employment history
    /// </summary>
    [Function("GetEmployment")]
    public async Task<HttpResponseData> GetEmployment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employment")] HttpRequestData req,
        FunctionContext context)
    {
        _logger.LogInformation("Getting employment history");

        try
        {
            var query = new GetAllEmploymentQuery();
            var employment = await _mediator.Send(query);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(employment);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employment history");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving employment history" });
            return response;
        }
    }
}
