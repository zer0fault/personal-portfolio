using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((context, services) =>
    {
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
