using Application.Settings.Commands.CreateSetting;
using Application.Settings.Commands.DeleteSetting;
using Application.Settings.Commands.UpdateSetting;
using Application.Settings.Queries.GetAllSettings;
using Application.Settings.Queries.GetSettingById;
using Application.Settings.Queries.GetSettingsByCategory;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Functions.API.Functions;

/// <summary>
/// HTTP trigger functions for Settings endpoints
/// </summary>
public class SettingsFunctions
{
    private readonly IMediator _mediator;
    private readonly ILogger<SettingsFunctions> _logger;

    public SettingsFunctions(IMediator mediator, ILogger<SettingsFunctions> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// GET /api/settings - Get all settings
    /// </summary>
    [Function("GetSettings")]
    public async Task<HttpResponseData> GetSettings(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "settings")] HttpRequestData req,
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

        _logger.LogInformation("Getting all settings");

        try
        {
            var query = new GetAllSettingsQuery();
            var settings = await _mediator.Send(query);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            await response.WriteAsJsonAsync(settings);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting settings");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving settings" });
            return response;
        }
    }

    /// <summary>
    /// GET /api/settings/category/{category} - Get settings by category
    /// </summary>
    [Function("GetSettingsByCategory")]
    public async Task<HttpResponseData> GetSettingsByCategory(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "options", Route = "settings/category/{category}")] HttpRequestData req,
        FunctionContext context,
        string category)
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

        _logger.LogInformation("Getting settings for category {Category}", category);

        try
        {
            var query = new GetSettingsByCategoryQuery(category);
            var settings = await _mediator.Send(query);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", "GET, OPTIONS");
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            await response.WriteAsJsonAsync(settings);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting settings for category {Category}", category);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving settings" });
            return response;
        }
    }

    /// <summary>
    /// GET /api/settings/{id} - Get setting by ID (admin)
    /// </summary>
    [Function("GetSettingById")]
    public async Task<HttpResponseData> GetSettingById(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "settings/{id:int}")] HttpRequestData req,
        FunctionContext context,
        int id)
    {
        _logger.LogInformation("Getting setting with ID {SettingId}", id);

        try
        {
            var query = new GetSettingByIdQuery(id);
            var setting = await _mediator.Send(query);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(setting);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting setting {SettingId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while retrieving the setting" });
            return response;
        }
    }

    /// <summary>
    /// POST /api/settings - Create a new setting (admin)
    /// </summary>
    [Function("CreateSetting")]
    public async Task<HttpResponseData> CreateSetting(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "settings")] HttpRequestData req,
        FunctionContext context)
    {
        _logger.LogInformation("Creating new setting");

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var command = JsonSerializer.Deserialize<CreateSettingCommand>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (command == null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badRequestResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await badRequestResponse.WriteAsJsonAsync(new { error = "Invalid request body" });
                return badRequestResponse;
            }

            var settingId = await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.Created);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { id = settingId });
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating setting");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while creating the setting" });
            return response;
        }
    }

    /// <summary>
    /// PUT /api/settings/{id} - Update an existing setting (admin)
    /// </summary>
    [Function("UpdateSetting")]
    public async Task<HttpResponseData> UpdateSetting(
        [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "settings/{id:int}")] HttpRequestData req,
        FunctionContext context,
        int id)
    {
        _logger.LogInformation("Updating setting with ID {SettingId}", id);

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var command = JsonSerializer.Deserialize<UpdateSettingCommand>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

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
            await response.WriteAsJsonAsync(new { message = "Setting updated successfully" });
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating setting {SettingId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while updating the setting" });
            return response;
        }
    }

    /// <summary>
    /// DELETE /api/settings/{id} - Delete a setting (admin)
    /// </summary>
    [Function("DeleteSetting")]
    public async Task<HttpResponseData> DeleteSetting(
        [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "settings/{id:int}")] HttpRequestData req,
        FunctionContext context,
        int id)
    {
        _logger.LogInformation("Deleting setting with ID {SettingId}", id);

        try
        {
            var command = new DeleteSettingCommand(id);
            await _mediator.Send(command);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { message = "Setting deleted successfully" });
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting setting {SettingId}", id);
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred while deleting the setting" });
            return response;
        }
    }
}
