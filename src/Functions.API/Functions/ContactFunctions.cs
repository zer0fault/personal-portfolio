using Application.Contact.Commands.SubmitContact;
using Application.Contact.Queries.GetAllContactSubmissions;
using Application.Contact.Queries.GetContactSubmissionById;
using Application.Contact.Commands.MarkContactAsRead;
using Application.Contact.Commands.DeleteContactSubmission;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Functions.API.Functions;

/// <summary>
/// HTTP trigger functions for Contact endpoints
/// </summary>
public class ContactFunctions
{
    private readonly IMediator _mediator;
    private readonly ILogger<ContactFunctions> _logger;

    public ContactFunctions(IMediator mediator, ILogger<ContactFunctions> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// POST /api/contact - Submit contact form
    /// GET /api/contact - Get all contact submissions (admin)
    /// </summary>
    [Function("Contact")]
    public async Task<HttpResponseData> Contact(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "options", Route = "contact")] HttpRequestData req,
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
            return await GetAllContactSubmissions(req);
        }
        else if (req.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
        {
            return await SubmitContact(req);
        }

        var badResponse = req.CreateResponse(HttpStatusCode.MethodNotAllowed);
        badResponse.Headers.Add("Access-Control-Allow-Origin", "*");
        return badResponse;
    }

    /// <summary>
    /// GET /api/contact/{id} - Get contact submission by ID (admin)
    /// PATCH /api/contact/{id} - Mark as read/unread (admin)
    /// DELETE /api/contact/{id} - Delete submission (admin)
    /// </summary>
    [Function("ContactById")]
    public async Task<HttpResponseData> ContactById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "patch", "delete", "options", Route = "contact/{id}")] HttpRequestData req,
        FunctionContext context,
        int id)
    {
        // Handle OPTIONS preflight request
        if (req.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            var preflightResponse = req.CreateResponse(HttpStatusCode.OK);
            preflightResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            preflightResponse.Headers.Add("Access-Control-Allow-Methods", "GET, PATCH, DELETE, OPTIONS");
            preflightResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            return preflightResponse;
        }

        if (req.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
        {
            return await GetContactSubmissionById(req, id);
        }
        else if (req.Method.Equals("PATCH", StringComparison.OrdinalIgnoreCase))
        {
            return await MarkContactAsRead(req, id);
        }
        else if (req.Method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
        {
            return await DeleteContactSubmission(req, id);
        }

        var badResponse = req.CreateResponse(HttpStatusCode.MethodNotAllowed);
        badResponse.Headers.Add("Access-Control-Allow-Origin", "*");
        return badResponse;
    }

    private async Task<HttpResponseData> GetAllContactSubmissions(HttpRequestData req)
    {
        // Admin only - log access without PII
        _logger.LogInformation("Admin accessing all contact submissions");

        try
        {
            var query = new GetAllContactSubmissionsQuery();
            var submissions = await _mediator.Send(query);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(submissions);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contact submissions");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving submissions" });
            return response;
        }
    }

    private async Task<HttpResponseData> GetContactSubmissionById(HttpRequestData req, int id)
    {
        // Admin only - log access without PII
        _logger.LogInformation("Admin accessing contact submission {SubmissionId}", id);

        try
        {
            var query = new GetContactSubmissionByIdQuery(id);
            var submission = await _mediator.Send(query);

            if (submission == null)
            {
                var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
                notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await notFoundResponse.WriteAsJsonAsync(new { error = "Contact submission not found" });
                return notFoundResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(submission);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving contact submission {SubmissionId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving the submission" });
            return response;
        }
    }

    private async Task<HttpResponseData> MarkContactAsRead(HttpRequestData req, int id)
    {
        _logger.LogInformation("Marking contact submission {SubmissionId} as read/unread", id);

        try
        {
            var body = await req.ReadFromJsonAsync<MarkContactAsReadRequest>();
            if (body == null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badRequestResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await badRequestResponse.WriteAsJsonAsync(new { error = "Invalid request body" });
                return badRequestResponse;
            }

            var command = new MarkContactAsReadCommand
            {
                Id = id,
                IsRead = body.IsRead
            };

            await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { success = true });
            return response;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Contact submission {SubmissionId} not found", id);
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await notFoundResponse.WriteAsJsonAsync(new { error = ex.Message });
            return notFoundResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking contact submission {SubmissionId} as read", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while updating the submission" });
            return response;
        }
    }

    private async Task<HttpResponseData> DeleteContactSubmission(HttpRequestData req, int id)
    {
        // Deletion is logged in the handler for audit trail
        try
        {
            var command = new DeleteContactSubmissionCommand { Id = id };
            await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { success = true });
            return response;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Contact submission {SubmissionId} not found", id);
            var notFoundResponse = req.CreateResponse(HttpStatusCode.NotFound);
            notFoundResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await notFoundResponse.WriteAsJsonAsync(new { error = ex.Message });
            return notFoundResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting contact submission {SubmissionId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while deleting the submission" });
            return response;
        }
    }

    private async Task<HttpResponseData> SubmitContact(HttpRequestData req)
    {
        _logger.LogInformation("Processing contact form submission");

        try
        {
            var requestBody = await req.ReadAsStringAsync();
            if (string.IsNullOrEmpty(requestBody))
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badRequestResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await badRequestResponse.WriteAsJsonAsync(new { error = "Request body is required" });
                return badRequestResponse;
            }

            var dto = JsonSerializer.Deserialize<Application.Contact.Commands.ContactSubmissionDto>(
                requestBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (dto == null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badRequestResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await badRequestResponse.WriteAsJsonAsync(new { error = "Invalid request format" });
                return badRequestResponse;
            }

            var command = new SubmitContactCommand(
                dto.Name,
                dto.Email,
                dto.Subject,
                dto.Message);

            var submissionId = await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            await response.WriteAsJsonAsync(new { id = submissionId, message = "Contact form submitted successfully" });
            return response;
        }
        catch (FluentValidation.ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed for contact submission");
            var validationResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            validationResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await validationResponse.WriteAsJsonAsync(new
            {
                error = "Validation failed",
                errors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
            });
            return validationResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing contact submission");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while processing your submission" });
            return response;
        }
    }

    private class MarkContactAsReadRequest
    {
        public bool IsRead { get; set; }
    }
}
