using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Functions.API.Functions;

/// <summary>
/// HTTP trigger functions for Authentication endpoints
/// </summary>
public class AuthFunctions
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthFunctions> _logger;

    public AuthFunctions(IConfiguration configuration, ILogger<AuthFunctions> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// POST /api/auth/login - Authenticate user and return JWT token
    /// </summary>
    [Function("Login")]
    public async Task<HttpResponseData> Login(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", "options", Route = "auth/login")] HttpRequestData req,
        FunctionContext context)
    {
        // Handle OPTIONS preflight request
        if (req.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
        {
            var preflightResponse = req.CreateResponse(HttpStatusCode.OK);
            preflightResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            preflightResponse.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
            preflightResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            return preflightResponse;
        }

        _logger.LogInformation("Login attempt");

        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var loginRequest = JsonSerializer.Deserialize<LoginRequest>(requestBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (loginRequest == null)
            {
                var badRequestResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                badRequestResponse.Headers.Add("Access-Control-Allow-Origin", "*");
                await badRequestResponse.WriteAsJsonAsync(new { error = "Invalid request" });
                return badRequestResponse;
            }

            // Get credentials from configuration with explicit defaults
            var adminUsername = "admin";
            var adminPassword = "admin123"; // TODO: Change this or use environment variables!

            // Try to get from configuration if available
            var configUsername = _configuration["AdminUsername"];
            var configPassword = _configuration["AdminPassword"];

            if (!string.IsNullOrEmpty(configUsername))
                adminUsername = configUsername;
            if (!string.IsNullOrEmpty(configPassword))
                adminPassword = configPassword;

            _logger.LogInformation($"Login attempt for user: {loginRequest.Username}");

            // Validate credentials
            if (loginRequest.Username == adminUsername && loginRequest.Password == adminPassword)
            {
                // Generate JWT token
                var token = GenerateJwtToken(loginRequest.Username);

                var response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Methods", "POST, OPTIONS");
                response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
                await response.WriteAsJsonAsync(new { token });
                return response;
            }

            // Invalid credentials
            var unauthorizedResponse = req.CreateResponse(HttpStatusCode.Unauthorized);
            unauthorizedResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            await unauthorizedResponse.WriteAsJsonAsync(new { error = "Invalid credentials" });
            return unauthorizedResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            var response = req.CreateResponse(HttpStatusCode.InternalServerError);
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            await response.WriteAsJsonAsync(new { error = "An error occurred during login" });
            return response;
        }
    }

    private string GenerateJwtToken(string username)
    {
        var jwtSecret = _configuration["JwtSecret"] ?? "YourSuperSecretKeyForJWTTokenGeneration123!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: "PortfolioAPI",
            audience: "PortfolioApp",
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
