using Application.Common.Interfaces;
using Application.Projects.Commands.DeleteProject;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly DeleteProjectCommandHandler _handler;

    public DeleteProjectCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new DeleteProjectCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_SoftDeleteProject()
    {
        // Arrange
        var project = new Project
        {
            Title = "Test Project",
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

        var command = new DeleteProjectCommand { Id = project.Id };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var deletedProject = await _context.Projects.FindAsync(project.Id);
        deletedProject.Should().NotBeNull();
        deletedProject!.IsDeleted.Should().BeTrue();
        deletedProject.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenProjectNotFound()
    {
        // Arrange
        var command = new DeleteProjectCommand { Id = 999 };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Project not found");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenProjectAlreadyDeleted()
    {
        // Arrange
        var project = new Project
        {
            Title = "Already Deleted",
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

        var command = new DeleteProjectCommand { Id = project.Id };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Project not found");
    }

    [Fact]
    public async Task Handle_Should_NotPhysicallyDeleteProject()
    {
        // Arrange
        var project = new Project
        {
            Title = "Test Project",
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

        var command = new DeleteProjectCommand { Id = project.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedProject = await _context.Projects
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == project.Id);

        deletedProject.Should().NotBeNull();
        deletedProject!.Title.Should().Be("Test Project");
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
