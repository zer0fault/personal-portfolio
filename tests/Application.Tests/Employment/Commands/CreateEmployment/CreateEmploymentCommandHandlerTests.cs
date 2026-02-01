using Application.Common.Interfaces;
using Application.Employment.Commands.CreateEmployment;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Employment.Commands.CreateEmployment;

public class CreateEmploymentCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly CreateEmploymentCommandHandler _handler;

    public CreateEmploymentCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new CreateEmploymentCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_CreateEmployment_WithValidData()
    {
        // Arrange
        var command = new CreateEmploymentCommand
        {
            CompanyName = "Test Company",
            JobTitle = "Senior Software Engineer",
            StartDate = new DateTime(2020, 1, 1),
            EndDate = new DateTime(2023, 12, 31),
            Responsibilities = new List<string> { "Led development team", "Architected solutions" },
            Achievements = new List<string> { "Increased performance by 50%", "Reduced costs" },
            Technologies = new List<string> { "C#", ".NET", "Azure" },
            DisplayOrder = 1
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
        var employment = await _context.EmploymentHistory.FindAsync(result);
        employment.Should().NotBeNull();
        employment!.CompanyName.Should().Be("Test Company");
        employment.JobTitle.Should().Be("Senior Software Engineer");
        employment.StartDate.Should().Be(new DateTime(2020, 1, 1));
        employment.EndDate.Should().Be(new DateTime(2023, 12, 31));
        employment.DisplayOrder.Should().Be(1);
        employment.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        employment.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_SerializeResponsibilities_AsJson()
    {
        // Arrange
        var command = new CreateEmploymentCommand
        {
            CompanyName = "Tech Corp",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow,
            Responsibilities = new List<string> { "Code review", "Mentoring", "Design" },
            Achievements = new List<string>(),
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var employment = await _context.EmploymentHistory.FindAsync(result);
        employment.Should().NotBeNull();
        employment!.Responsibilities.Should().Contain("Code review");
        employment.Responsibilities.Should().Contain("Mentoring");
        employment.Responsibilities.Should().Contain("Design");
    }

    [Fact]
    public async Task Handle_Should_SerializeAchievements_AsJson()
    {
        // Arrange
        var command = new CreateEmploymentCommand
        {
            CompanyName = "Innovation Inc",
            JobTitle = "Tech Lead",
            StartDate = DateTime.UtcNow,
            Responsibilities = new List<string>(),
            Achievements = new List<string> { "Award winner", "Patent filed", "Team growth" },
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var employment = await _context.EmploymentHistory.FindAsync(result);
        employment.Should().NotBeNull();
        employment!.Achievements.Should().Contain("Award winner");
        employment.Achievements.Should().Contain("Patent filed");
        employment.Achievements.Should().Contain("Team growth");
    }

    [Fact]
    public async Task Handle_Should_SerializeTechnologies_AsJson()
    {
        // Arrange
        var command = new CreateEmploymentCommand
        {
            CompanyName = "Software Solutions",
            JobTitle = "Full Stack Developer",
            StartDate = DateTime.UtcNow,
            Responsibilities = new List<string>(),
            Achievements = new List<string>(),
            Technologies = new List<string> { "React", "TypeScript", "Node.js", "PostgreSQL" },
            DisplayOrder = 1
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var employment = await _context.EmploymentHistory.FindAsync(result);
        employment.Should().NotBeNull();
        employment!.Technologies.Should().Contain("React");
        employment!.Technologies.Should().Contain("TypeScript");
        employment!.Technologies.Should().Contain("Node.js");
        employment!.Technologies.Should().Contain("PostgreSQL");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenCompanyNameIsEmpty()
    {
        // Arrange
        var command = new CreateEmploymentCommand
        {
            CompanyName = "",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow,
            Responsibilities = new List<string>(),
            Achievements = new List<string>(),
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Company name is required");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenJobTitleIsEmpty()
    {
        // Arrange
        var command = new CreateEmploymentCommand
        {
            CompanyName = "Test Company",
            JobTitle = "",
            StartDate = DateTime.UtcNow,
            Responsibilities = new List<string>(),
            Achievements = new List<string>(),
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Job title is required");
    }

    [Fact]
    public async Task Handle_Should_CreateEmployment_WithoutEndDate_ForCurrentPosition()
    {
        // Arrange
        var command = new CreateEmploymentCommand
        {
            CompanyName = "Current Company",
            JobTitle = "Senior Developer",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = null,
            Responsibilities = new List<string> { "Current role" },
            Achievements = new List<string>(),
            Technologies = new List<string> { "C#" },
            DisplayOrder = 1
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var employment = await _context.EmploymentHistory.FindAsync(result);
        employment.Should().NotBeNull();
        employment!.EndDate.Should().BeNull();
        employment.StartDate.Should().Be(new DateTime(2023, 1, 1));
    }

    [Fact]
    public async Task Handle_Should_CreateEmployment_WithEmptyLists()
    {
        // Arrange
        var command = new CreateEmploymentCommand
        {
            CompanyName = "Minimal Corp",
            JobTitle = "Junior Developer",
            StartDate = DateTime.UtcNow,
            EndDate = null,
            Responsibilities = new List<string>(),
            Achievements = new List<string>(),
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var employment = await _context.EmploymentHistory.FindAsync(result);
        employment.Should().NotBeNull();
        employment!.Responsibilities.Should().Be("[]");
        employment.Achievements.Should().Be("[]");
        employment.Technologies.Should().Be("[]");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private class TestDbContext : DbContext, IApplicationDbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<ProjectImage> ProjectImages { get; set; } = null!;
        public DbSet<Domain.Entities.Employment> EmploymentHistory { get; set; } = null!;
        public DbSet<ContactSubmission> ContactSubmissions { get; set; } = null!;
        public DbSet<Skill> Skills { get; set; } = null!;
        public DbSet<Domain.Entities.Settings> Settings { get; set; } = null!;
    }
}
