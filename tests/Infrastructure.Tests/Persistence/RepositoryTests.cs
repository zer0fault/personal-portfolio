using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.Tests.Persistence;

public class RepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IRepository<Project> _repository;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options, new DateTimeService());
        _repository = new Repository<Project>(_context);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Entity_When_Exists()
    {
        // Arrange
        var project = new Project
        {
            Title = "Test Project",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = "[]",
            Status = ProjectStatus.Published,
            DisplayOrder = 1
        };

        await _repository.AddAsync(project);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(project.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().Be("Test Project");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Null_When_Not_Exists()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_Should_Return_All_Entities()
    {
        // Arrange
        var projects = new[]
        {
            new Project
            {
                Title = "Project 1",
                ShortDescription = "Short 1",
                FullDescription = "Full 1",
                Technologies = "[]",
                Status = ProjectStatus.Published,
                DisplayOrder = 1
            },
            new Project
            {
                Title = "Project 2",
                ShortDescription = "Short 2",
                FullDescription = "Full 2",
                Technologies = "[]",
                Status = ProjectStatus.Draft,
                DisplayOrder = 2
            }
        };

        foreach (var project in projects)
        {
            await _repository.AddAsync(project);
        }
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(p => p.Title == "Project 1");
        result.Should().Contain(p => p.Title == "Project 2");
    }

    [Fact]
    public async Task AddAsync_Should_Add_Entity()
    {
        // Arrange
        var project = new Project
        {
            Title = "New Project",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = "[]",
            Status = ProjectStatus.Draft,
            DisplayOrder = 1
        };

        // Act
        await _repository.AddAsync(project);
        await _context.SaveChangesAsync();

        // Assert
        var saved = await _context.Projects.FindAsync(project.Id);
        saved.Should().NotBeNull();
        saved!.Title.Should().Be("New Project");
    }

    [Fact]
    public async Task Update_Should_Modify_Entity()
    {
        // Arrange
        var project = new Project
        {
            Title = "Original Title",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = "[]",
            Status = ProjectStatus.Draft,
            DisplayOrder = 1
        };

        await _repository.AddAsync(project);
        await _context.SaveChangesAsync();

        // Act
        project.Title = "Updated Title";
        _repository.Update(project);
        await _context.SaveChangesAsync();

        // Assert
        var updated = await _context.Projects.FindAsync(project.Id);
        updated.Should().NotBeNull();
        updated!.Title.Should().Be("Updated Title");
    }

    [Fact]
    public async Task Delete_Should_Remove_Entity()
    {
        // Arrange
        var project = new Project
        {
            Title = "To Delete",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = "[]",
            Status = ProjectStatus.Draft,
            DisplayOrder = 1
        };

        await _repository.AddAsync(project);
        await _context.SaveChangesAsync();

        var projectId = project.Id;

        // Act
        _repository.Delete(project);
        await _context.SaveChangesAsync();

        // Assert
        var deleted = await _context.Projects.FindAsync(projectId);
        deleted.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
