# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Personal portfolio website built with .NET 9, following Clean Architecture principles. Uses Blazor WebAssembly frontend hosted on GitHub Pages, Azure Functions for serverless backend API, and Azure SQL Database for persistence.

**Tech Stack:**
- .NET 9 / C# 13
- Blazor WebAssembly (frontend)
- Azure Functions .NET Isolated (backend)
- Entity Framework Core 9 with Code First migrations
- MediatR (CQRS pattern)
- FluentValidation
- xUnit + Moq (testing)

## Project Structure

The solution follows **Clean Architecture** with strict dependency flow:

```
Domain → Application → Infrastructure → Functions.API
                                      → BlazorApp
```

**Layer Responsibilities:**
- **Domain** (`src/Domain/`): Core entities (`Project`, `Employment`, `Skill`, `Settings`, `ContactSubmission`), enums, base classes. No external dependencies.
- **Application** (`src/Application/`): CQRS handlers via MediatR, DTOs, validators (FluentValidation), interfaces. Depends only on Domain.
- **Infrastructure** (`src/Infrastructure/`): EF Core `ApplicationDbContext`, `Repository<T>`, configurations, migrations, `DatabaseSeeder`. Implements Application interfaces.
- **Functions.API** (`src/Functions.API/`): Azure Functions HTTP triggers. Thin wrappers around MediatR handlers.
- **BlazorApp** (`src/BlazorApp/`): Blazor WASM frontend with services calling Functions.API.

## Common Development Commands

### Build
```bash
dotnet build
```

### Run Locally
```bash
# Run Azure Functions (API backend)
cd src/Functions.API
func start

# Run Blazor app (frontend) - in separate terminal
cd src/BlazorApp
dotnet run
```

### Database Migrations
```bash
# Add new migration (run from repository root)
dotnet ef migrations add MigrationName --project src/Infrastructure --startup-project src/Functions.API

# Apply migrations (happens automatically on Functions startup, but manual command is)
dotnet ef database update --project src/Infrastructure --startup-project src/Functions.API

# Remove last migration (if not applied)
dotnet ef migrations remove --project src/Infrastructure --startup-project src/Functions.API
```

### Testing
```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test tests/Application.Tests

# Run single test
dotnet test --filter "FullyQualifiedName~CreateProjectCommandHandlerTests.Handle_ValidCommand_CreatesProject"
```

### Azure Functions
```bash
# Start Functions runtime locally
cd src/Functions.API
func start

# Clean output directory if having issues
cd src/Functions.API
rm -rf bin/output
func start
```

## Architecture Patterns

### CQRS with MediatR

All business logic uses Command/Query separation:
- **Commands**: Mutate state (Create, Update, Delete) - return `Unit` or entity ID
- **Queries**: Read data - return DTOs

**Pattern:**
```csharp
// 1. Define query/command
public record GetProjectByIdQuery(int Id) : IRequest<ProjectDetailDto>;

// 2. Create handler
public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDetailDto>
{
    public async Task<ProjectDetailDto> Handle(GetProjectByIdQuery request, CancellationToken ct)
    {
        // Implementation using IRepository<T>
    }
}

// 3. Use in Azure Function
[Function("GetProjectById")]
public async Task<HttpResponseData> GetProjectById(
    [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "projects/{id}")] HttpRequestData req,
    int id)
{
    var result = await _mediator.Send(new GetProjectByIdQuery(id));
    // Return response
}
```

### Repository Pattern

Generic `IRepository<T>` with common operations:
```csharp
Task<T?> GetByIdAsync(int id);
Task<IEnumerable<T>> GetAllAsync();
Task<T> AddAsync(T entity);
Task UpdateAsync(T entity);
Task DeleteAsync(int id); // Soft delete if ISoftDeletable
```

**Soft Delete:** Entities implementing `ISoftDeletable` are marked `IsDeleted = true` instead of physical deletion.

### Database Seeding

`DatabaseSeeder.SeedAsync()` runs on Functions startup (Program.cs line 43). Seeds initial data for Settings, Skills, Projects, Employment. **Idempotent** - checks for existing data before seeding.

### Validation

FluentValidation validators for all commands:
```csharp
public class SubmitContactCommandValidator : AbstractValidator<SubmitContactCommand>
{
    public SubmitContactCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Message).NotEmpty().MaximumLength(2000);
    }
}
```

Validators automatically registered via `services.AddValidatorsFromAssembly()`.

## Key Conventions

### Entity Configuration
All entities configured via `IEntityTypeConfiguration<T>` in `Infrastructure/Persistence/Configurations/`:
- Primary keys
- Required fields
- Max lengths
- Relationships
- Indexes
- Default values

### Audit Fields
`BaseEntity` provides:
- `CreatedDate` - auto-set on insert
- `ModifiedDate` - auto-updated on change
- Managed by EF Core `SaveChangesAsync` override

### Display Order
Entities like `Project`, `Employment`, `Skill` have `DisplayOrder` for sorting. Lower numbers appear first.

### DTOs vs Entities
- **Never expose entities directly** to API consumers
- Always map to DTOs in query handlers
- DTOs live in `Application/[Feature]/Queries/DTOs/`

## Testing Strategy

**Test Coverage: 279 tests across 4 projects**

- **Domain.Tests** (39 tests): Entity validation, business rules
- **Application.Tests** (218 tests): All command/query handlers, validators
- **Infrastructure.Tests** (13 tests): Repository operations, configurations
- **Functions.API.IntegrationTests** (9 tests): Database operations, seeding

**Philosophy:** Unit test all handlers and validators. Integration tests focus on critical database operations. API functions are thin wrappers, so no separate API tests.

## Database Connection

The application uses **SQLite for local development** if no connection string is configured, **SQL Server for production**.

Connection string location:
- **Local dev**: `src/Functions.API/local.settings.json` (git-ignored)
- **Production**: Azure Function App Settings

EF Core migrations work with both SQLite and SQL Server.

## CORS Configuration

Azure Functions configured to allow Blazor app origins (Program.cs):
- `http://localhost:5000`
- `https://localhost:5001`
- `http://localhost:5173` (Vite dev server)
- `https://localhost:7000`

Update for production GitHub Pages domain when deploying.

## Critical Security Notes

**NEVER commit these files** (already in .gitignore):
- `local.settings.json` - Contains connection strings and secrets
- `appsettings.Development.json` - Contains development secrets
- `*.db`, `*.sqlite` - Local database files
- Planning documents: `broad_requirements.txt`, `REQUIREMENTS.md`, `ZERO-COST-CHECKLIST.md`, `IMPLEMENTATION-PLAN.md`

See `SECURITY.md` and `COMMIT-GUIDE.md` for detailed security practices.

**Before committing:** Run verification script if available, or manually check `git status` for sensitive files.

## Common Issues & Solutions

### Issue: Migrations fail with "No project was found"
**Solution:** Always specify `--project src/Infrastructure --startup-project src/Functions.API` with EF Core commands.

### Issue: Functions not loading after code changes
**Solution:** Clean output directory: `rm -rf src/Functions.API/bin/output && func start`

### Issue: Database seed runs multiple times
**Solution:** Seeder is idempotent - it checks for existing data. If duplicates appear, check logic in `DatabaseSeeder.cs`.

### Issue: CORS errors in browser
**Solution:** Verify Blazor app origin is in `Program.cs` CORS policy. Check browser console for exact origin being blocked.

## Deployment

**Frontend (Blazor):** GitHub Pages
**Backend (Functions):** Azure Functions Consumption Plan
**Database:** Azure SQL Database (or SQLite locally)

Deployment uses GitHub Actions. Secrets stored as GitHub Secrets, not in code.

## Adding New Features

### New Entity
1. Create entity in `Domain/Entities/`
2. Add `DbSet<T>` to `IApplicationDbContext` and `ApplicationDbContext`
3. Create configuration in `Infrastructure/Persistence/Configurations/`
4. Add migration: `dotnet ef migrations add Add[Entity]`
5. Update `DatabaseSeeder` if needed

### New Query/Command
1. Create query/command in `Application/[Feature]/Queries|Commands/`
2. Create handler (same folder or subfolder)
3. Create validator if command mutates state
4. Add Azure Function in `Functions.API/Functions/`
5. Write tests in `Application.Tests/[Feature]/`

### New Azure Function
1. Create in `Functions.API/Functions/[FeatureName]Functions.cs`
2. Inject `IMediator` via constructor
3. Use `[Function("FunctionName")]` attribute
4. Define HTTP trigger with route
5. Send MediatR request, handle response
6. Return `HttpResponseData` with JSON

## Useful File Locations

- **Entry point (Functions):** `src/Functions.API/Program.cs`
- **Entry point (Blazor):** `src/BlazorApp/Program.cs`
- **DbContext:** `src/Infrastructure/Persistence/ApplicationDbContext.cs`
- **Migrations:** `src/Infrastructure/Migrations/`
- **Database seeder:** `src/Infrastructure/Persistence/DatabaseSeeder.cs`
- **DI setup (Application):** `src/Application/DependencyInjection.cs`
- **DI setup (Infrastructure):** `src/Infrastructure/DependencyInjection.cs`
