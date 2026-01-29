using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace BlazorApp.Services;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private string? _token;
    private bool _isAuthenticated;

    public event Action? OnAuthStateChanged;

    public AuthService(HttpClient httpClient, IJSRuntime jsRuntime)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
    }

    public bool IsAuthenticated => _isAuthenticated;
    public string? Token => _token;

    public async Task InitializeAsync()
    {
        try
        {
            _token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", "authToken");
            _isAuthenticated = !string.IsNullOrEmpty(_token);

            if (_isAuthenticated)
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
            }
        }
        catch
        {
            _isAuthenticated = false;
            _token = null;
        }
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            Console.WriteLine($"Attempting login for user: {username}");
            Console.WriteLine($"API Base URL: {_httpClient.BaseAddress}");

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", new
            {
                Username = username,
                Password = password
            });

            Console.WriteLine($"Response status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result?.Token != null)
                {
                    _token = result.Token;
                    _isAuthenticated = true;

                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "authToken", _token);
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);

                    OnAuthStateChanged?.Invoke();
                    Console.WriteLine("Login successful!");
                    return true;
                }
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login failed: {errorContent}");
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login exception: {ex.Message}");
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        _token = null;
        _isAuthenticated = false;

        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "authToken");
        _httpClient.DefaultRequestHeaders.Authorization = null;

        OnAuthStateChanged?.Invoke();
    }

    private class LoginResponse
    {
        public string? Token { get; set; }
    }
}
