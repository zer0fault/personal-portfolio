using Application.Common.Interfaces;
using Application.Projects.Commands.CreateProject;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Projects.Commands.CreateProject;

public class CreateProjectCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new CreateProjectCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_CreateProject_WithValidData()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Title = "Test Project",
            ShortDescription = "A test project description",
            FullDescription = "This is a full description of the test project",
            Technologies = new List<string> { "C#", ".NET", "Azure" },
            GitHubUrl = "https://github.com/test/repo",
            LiveDemoUrl = "https://test.com",
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
        var project = await _context.Projects.FindAsync(result);
        project.Should().NotBeNull();
        project!.Title.Should().Be("Test Project");
        project.ShortDescription.Should().Be("A test project description");
        project.FullDescription.Should().Be("This is a full description of the test project");
        project.GitHubUrl.Should().Be("https://github.com/test/repo");
        project.LiveDemoUrl.Should().Be("https://test.com");
        project.DisplayOrder.Should().Be(1);
        project.Status.Should().Be(ProjectStatus.Published);
        project.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        project.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_SerializeTechnologies_AsJson()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Title = "Tech Test",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = new List<string> { "React", "TypeScript", "Node.js" },
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var project = await _context.Projects.FindAsync(result);
        project.Should().NotBeNull();
        project!.Technologies.Should().Contain("React");
        project.Technologies.Should().Contain("TypeScript");
        project.Technologies.Should().Contain("Node.js");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenTitleIsEmpty()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Title = "",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = new List<string>(),
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Title is required");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenShortDescriptionIsEmpty()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Title = "Test",
            ShortDescription = "",
            FullDescription = "Full",
            Technologies = new List<string>(),
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Short description is required");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenFullDescriptionIsEmpty()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Title = "Test",
            ShortDescription = "Short",
            FullDescription = "",
            Technologies = new List<string>(),
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Full description is required");
    }

    [Fact]
    public async Task Handle_Should_CreateProject_WithoutOptionalUrls()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Title = "Minimal Project",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = new List<string> { "C#" },
            GitHubUrl = null,
            LiveDemoUrl = null,
            DisplayOrder = 1,
            Status = ProjectStatus.Draft
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var project = await _context.Projects.FindAsync(result);
        project.Should().NotBeNull();
        project!.GitHubUrl.Should().BeNull();
        project.LiveDemoUrl.Should().BeNull();
        project.Status.Should().Be(ProjectStatus.Draft);
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
