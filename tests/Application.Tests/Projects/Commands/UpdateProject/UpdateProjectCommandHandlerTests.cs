using Application.Common.Interfaces;
using Application.Projects.Commands.UpdateProject;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly UpdateProjectCommandHandler _handler;

    public UpdateProjectCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new UpdateProjectCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_UpdateProject_WithValidData()
    {
        // Arrange
        var project = new Project
        {
            Title = "Original Title",
            ShortDescription = "Original Short",
            FullDescription = "Original Full",
            Technologies = "[\"Old Tech\"]",
            DisplayOrder = 1,
            Status = ProjectStatus.Draft,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            ModifiedDate = DateTime.UtcNow.AddDays(-1)
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        var command = new UpdateProjectCommand
        {
            Id = project.Id,
            Title = "Updated Title",
            ShortDescription = "Updated Short",
            FullDescription = "Updated Full",
            Technologies = new List<string> { "New Tech", "C#" },
            GitHubUrl = "https://github.com/updated",
            LiveDemoUrl = "https://updated.com",
            DisplayOrder = 2,
            Status = ProjectStatus.Published
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var updatedProject = await _context.Projects.FindAsync(project.Id);
        updatedProject.Should().NotBeNull();
        updatedProject!.Title.Should().Be("Updated Title");
        updatedProject.ShortDescription.Should().Be("Updated Short");
        updatedProject.FullDescription.Should().Be("Updated Full");
        updatedProject.GitHubUrl.Should().Be("https://github.com/updated");
        updatedProject.LiveDemoUrl.Should().Be("https://updated.com");
        updatedProject.DisplayOrder.Should().Be(2);
        updatedProject.Status.Should().Be(ProjectStatus.Published);
        updatedProject.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenProjectNotFound()
    {
        // Arrange
        var command = new UpdateProjectCommand
        {
            Id = 999,
            Title = "Test",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = new List<string>(),
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Project not found");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenProjectIsDeleted()
    {
        // Arrange
        var project = new Project
        {
            Title = "Deleted Project",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = "[]",
            IsDeleted = true,
            DisplayOrder = 1,
            Status = ProjectStatus.Published,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        var command = new UpdateProjectCommand
        {
            Id = project.Id,
            Title = "Updated",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = new List<string>(),
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Project not found");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenTitleIsEmpty()
    {
        // Arrange
        var project = new Project
        {
            Title = "Original",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = "[]",
            IsDeleted = false,
            DisplayOrder = 1,
            Status = ProjectStatus.Published,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        var command = new UpdateProjectCommand
        {
            Id = project.Id,
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
    public async Task Handle_Should_UpdateModifiedDate_NotCreatedDate()
    {
        // Arrange
        var originalCreatedDate = DateTime.UtcNow.AddDays(-10);
        var project = new Project
        {
            Title = "Original",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = "[]",
            IsDeleted = false,
            DisplayOrder = 1,
            Status = ProjectStatus.Published,
            CreatedDate = originalCreatedDate,
            ModifiedDate = originalCreatedDate
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        var command = new UpdateProjectCommand
        {
            Id = project.Id,
            Title = "Updated",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = new List<string>(),
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedProject = await _context.Projects.FindAsync(project.Id);
        updatedProject!.CreatedDate.Should().Be(originalCreatedDate);
        updatedProject.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
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
