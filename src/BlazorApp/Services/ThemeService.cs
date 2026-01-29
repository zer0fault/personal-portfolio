using Microsoft.JSInterop;

namespace BlazorApp.Services;

public class ThemeService
{
    private readonly IJSRuntime _jsRuntime;
    private string _currentTheme = "light";
    public event Action? OnThemeChanged;

    public ThemeService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public string CurrentTheme => _currentTheme;

    public async Task InitializeAsync()
    {
        try
        {
            _currentTheme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "theme") ?? "light";
            await ApplyThemeAsync(_currentTheme);
        }
        catch
        {
            _currentTheme = "light";
        }
    }

    public async Task ToggleThemeAsync()
    {
        _currentTheme = _currentTheme == "light" ? "dark" : "light";
        await ApplyThemeAsync(_currentTheme);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "theme", _currentTheme);
        OnThemeChanged?.Invoke();
    }

    private async Task ApplyThemeAsync(string theme)
    {
        await _jsRuntime.InvokeVoidAsync("eval", $"document.documentElement.setAttribute('data-theme', '{theme}')");
    }
}
