using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
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
                    "https://localhost:7000")
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
        });

        // Register Application and Infrastructure layers
        services.AddApplication();
        services.AddInfrastructure(context.Configuration);
    })
    .Build();

// Apply migrations and seed database for development
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Apply any pending migrations
    await dbContext.Database.MigrateAsync();

    // Seed initial data
    await DatabaseSeeder.SeedAsync(dbContext);
}

host.Run();
