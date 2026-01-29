using Application.Contact.Commands.SubmitContact;
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
    /// </summary>
    [Function("SubmitContact")]
    public async Task<HttpResponseData> SubmitContact(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", "options", Route = "contact")] HttpRequestData req,
        FunctionContext context)
    {
        // Handle OPTIONS preflight request
        if (req.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            var preflightResponse = req.CreateResponse(HttpStatusCode.OK);
            preflightResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            preflightResponse.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
            preflightResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            return preflightResponse;
        }

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
}
