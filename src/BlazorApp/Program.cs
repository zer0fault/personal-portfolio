using BlazorApp;
using BlazorApp.Services;
using BlazorApp.Services.Static;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register HttpClient for any remaining needs (navigation, etc.)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register AutoMapper
builder.Services.AddAutoMapper(typeof(Application.Common.Mappings.MappingProfile));

// Register static data services (no API calls, all data embedded)
builder.Services.AddScoped<IProjectsService, StaticProjectsService>();
builder.Services.AddScoped<IEmploymentService, StaticEmploymentService>();
builder.Services.AddScoped<ISkillsService, StaticSkillsService>();
builder.Services.AddScoped<IContactService, StaticContactService>();
builder.Services.AddScoped<ISettingsService, StaticSettingsService>();
builder.Services.AddScoped<ThemeService>();
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();
