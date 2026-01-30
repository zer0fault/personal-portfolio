using Application.Employment.Queries.GetAllEmployment;
using Application.Employment.Queries.GetEmploymentByIdForAdmin;
using Application.Employment.Commands.CreateEmployment;
using Application.Employment.Commands.UpdateEmployment;
using Application.Employment.Commands.DeleteEmployment;
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
    /// POST /api/employment - Create a new employment entry
    /// </summary>
    [Function("Employment")]
    public async Task<HttpResponseData> Employment(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "options", Route = "employment")] HttpRequestData req,
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
            return await GetAllEmployment(req);
        }
        else if (req.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            return await CreateEmployment(req);
        }

        var badResponse = req.CreateResponse(HttpStatusCode.MethodNotAllowed);
        badResponse.Headers.Add("Access-Control-Allow-Origin", "*");
        return badResponse;
    }

    /// <summary>
    /// GET /api/employment/{id} - Get employment entry by ID for admin
    /// PUT /api/employment/{id} - Update an employment entry
    /// DELETE /api/employment/{id} - Delete an employment entry
    /// </summary>
    [Function("EmploymentById")]
    public async Task<HttpResponseData> EmploymentById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "put", "delete", "options", Route = "employment/{id}")] HttpRequestData req,
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
            return await GetEmploymentByIdForAdmin(req, id);
        }
        else if (req.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
        {
            return await UpdateEmployment(req, id);
        }
        else if (req.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
        {
            return await DeleteEmployment(req, id);
        }

        var badResponse = req.CreateResponse(HttpStatusCode.MethodNotAllowed);
        badResponse.Headers.Add("Access-Control-Allow-Origin", "*");
        return badResponse;
    }

    private async Task<HttpResponseData> GetAllEmployment(HttpRequestData req)
    {
        _logger.LogInformation("Getting employment history");

        try
        {
            var query = new GetAllEmploymentQuery();
            var employment = await _mediator.Send(query);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(employment);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employment history");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving employment history" });
            return response;
        }
    }

    private async Task<HttpResponseData> GetEmploymentByIdForAdmin(HttpRequestData req, int id)
    {
        _logger.LogInformation("Getting employment entry {EmploymentId} for admin", id);

        try
        {
            var query = new GetEmploymentByIdForAdminQuery(id);
            var employment = await _mediator.Send(query);

            if (employment == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await notFoundResponse.WriteAsJsonAsync(new { error = "Employment entry not found" });
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(employment);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employment entry {EmploymentId} for admin", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving the employment entry" });
            return response;
        }
    }

    private async Task<HttpResponseData> CreateEmployment(HttpRequestData req)
    {
        _logger.LogInformation("Creating new employment entry");

        try
        {
            var command = await req.ReadFromJsonAsync<CreateEmploymentCommand>();
            if (command == null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badRequestResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await badRequestResponse.WriteAsJsonAsync(new { error = "Invalid request body" });
                return badRequestResponse;
            }

            var employmentId = await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { id = employmentId });
            return response;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error creating employment entry");
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await errorResponse.WriteAsJsonAsync(new { error = ex.Message });
            return errorResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employment entry");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while creating the employment entry" });
            return response;
        }
    }

    private async Task<HttpResponseData> UpdateEmployment(HttpRequestData req, int id)
    {
        _logger.LogInformation("Updating employment entry {EmploymentId}", id);

        try
        {
            var command = await req.ReadFromJsonAsync<UpdateEmploymentCommand>();
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
            _logger.LogWarning(ex, "Validation error updating employment entry {EmploymentId}", id);
            var errorResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            errorResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await errorResponse.WriteAsJsonAsync(new { error = ex.Message });
            return errorResponse;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Employment entry {EmploymentId} not found", id);
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await notFoundResponse.WriteAsJsonAsync(new { error = ex.Message });
            return notFoundResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employment entry {EmploymentId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while updating the employment entry" });
            return response;
        }
    }

    private async Task<HttpResponseData> DeleteEmployment(HttpRequestData req, int id)
    {
        _logger.LogInformation("Deleting employment entry {EmploymentId}", id);

        try
        {
            var command = new DeleteEmploymentCommand { Id = id };
            await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { success = true });
            return response;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Employment entry {EmploymentId} not found", id);
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await notFoundResponse.WriteAsJsonAsync(new { error = ex.Message });
            return notFoundResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employment entry {EmploymentId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while deleting the employment entry" });
            return response;
        }
    }
}
