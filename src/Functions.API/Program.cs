using Application;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
        // Add CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorApp", builder =>
            {
                builder.WithOrigins(
                    "http://localhost:5000",
                    "https://localhost:5001",
                    "http://localhost:5173",
                    "https://localhost:7000",
                    "https://zer0fault.github.io")
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        // Register Application layer (no database needed)
        services.AddApplication();
    })
    .Build();

host.Run();
