using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Functions.API.IntegrationTests;

/// <summary>
/// Integration tests for repository operations and soft delete support
/// </summary>
public class RepositoryIntegrationTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public RepositoryIntegrationTests()
    {
        _dateTime = new DateTimeService();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options, _dateTime);
    }

    [Fact]
    public async Task SoftDelete_ShouldMarkProjectAsDeleted()
    {
        // Arrange
        var project = new Project
        {
            Title = "Test Project",
            ShortDescription = "Test",
            FullDescription = "Test",
            Technologies = "[\"C#\"]",
            Status = ProjectStatus.Published,
            DisplayOrder = 1,
            IsDeleted = false
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Act
        project.IsDeleted = true;
        await _context.SaveChangesAsync();

        // Assert
        var retrieved = await _context.Projects
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == project.Id);

        retrieved.Should().NotBeNull();
        retrieved!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task SoftDelete_ShouldMarkEmploymentAsDeleted()
    {
        // Arrange
        var employment = new Employment
        {
            CompanyName = "Test Company",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow.AddYears(-1),
            Responsibilities = "[\"Coding\"]",
            Achievements = "[\"Award\"]",
            Technologies = "[\"C#\"]",
            DisplayOrder = 1,
            IsDeleted = false
        };

        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        // Act
        employment.IsDeleted = true;
        await _context.SaveChangesAsync();

        // Assert
        var retrieved = await _context.EmploymentHistory.FindAsync(employment.Id);
        retrieved.Should().NotBeNull();
        retrieved!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task SoftDelete_ShouldMarkSettingsAsDeleted()
    {
        // Arrange
        var setting = new Settings
        {
            Key = "TestKey",
            Value = "TestValue",
            Category = "Test",
            LastModified = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();

        // Act
        setting.IsDeleted = true;
        await _context.SaveChangesAsync();

        // Assert
        var retrieved = await _context.Settings.FindAsync(setting.Id);
        retrieved.Should().NotBeNull();
        retrieved!.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task Queries_ShouldRespectDisplayOrder()
    {
        // Arrange
        await DatabaseSeeder.SeedAsync(_context);

        // Act
        var projects = await _context.Projects
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();

        var skills = await _context.Skills
            .OrderBy(s => s.DisplayOrder)
            .ThenBy(s => s.Name)
            .ToListAsync();

        // Assert
        projects.Should().NotBeEmpty();
        projects.Should().BeInAscendingOrder(p => p.DisplayOrder);

        skills.Should().NotBeEmpty();
        var firstSkill = skills.First();
        firstSkill.DisplayOrder.Should().Be(skills.Min(s => s.DisplayOrder));
    }

    [Fact]
    public async Task ComplexQuery_ShouldFilterByStatusAndOrderByDisplayOrder()
    {
        // Arrange
        var projects = new[]
        {
            new Project { Title = "P1", ShortDescription = "S", FullDescription = "F", Technologies = "[]", Status = ProjectStatus.Published, DisplayOrder = 2 },
            new Project { Title = "P2", ShortDescription = "S", FullDescription = "F", Technologies = "[]", Status = ProjectStatus.Draft, DisplayOrder = 1 },
            new Project { Title = "P3", ShortDescription = "S", FullDescription = "F", Technologies = "[]", Status = ProjectStatus.Published, DisplayOrder = 1 },
            new Project { Title = "P4", ShortDescription = "S", FullDescription = "F", Technologies = "[]", Status = ProjectStatus.Archived, DisplayOrder = 3 }
        };

        _context.Projects.AddRange(projects);
        await _context.SaveChangesAsync();

        // Act
        var publishedProjects = await _context.Projects
            .Where(p => p.Status == ProjectStatus.Published)
            .OrderBy(p => p.DisplayOrder)
            .ToListAsync();

        // Assert
        publishedProjects.Should().HaveCount(2);
        publishedProjects[0].Title.Should().Be("P3");
        publishedProjects[1].Title.Should().Be("P1");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
