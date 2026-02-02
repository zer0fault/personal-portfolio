using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Functions.API.IntegrationTests;

/// <summary>
/// Integration tests for database setup, seeding, and basic operations
/// </summary>
public class DatabaseIntegrationTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public DatabaseIntegrationTests()
    {
        _dateTime = new DateTimeService();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options, _dateTime);
    }

    [Fact]
    public async Task DatabaseSeeder_ShouldSeedAllEntities()
    {
        // Act
        await DatabaseSeeder.SeedAsync(_context);

        // Assert
        var projects = await _context.Projects.ToListAsync();
        var employment = await _context.EmploymentHistory.ToListAsync();
        var skills = await _context.Skills.ToListAsync();
        var settings = await _context.Settings.ToListAsync();

        projects.Should().NotBeEmpty();
        employment.Should().NotBeEmpty();
        skills.Should().NotBeEmpty();
        settings.Should().NotBeEmpty();
    }

    [Fact]
    public async Task DatabaseSeeder_ShouldNotSeedTwice()
    {
        // Arrange
        await DatabaseSeeder.SeedAsync(_context);
        var firstCount = await _context.Projects.CountAsync();

        // Act
        await DatabaseSeeder.SeedAsync(_context);

        // Assert
        var secondCount = await _context.Projects.CountAsync();
        secondCount.Should().Be(firstCount);
    }

    [Fact]
    public async Task Database_ShouldSupportCrudOperations()
    {
        // Arrange
        var project = new Project
        {
            Title = "Test Project",
            ShortDescription = "Test Description",
            FullDescription = "Full Description",
            Technologies = "[\"C#\"]",
            Status = ProjectStatus.Draft,
            DisplayOrder = 1
        };

        // Act - Create
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Assert - Read
        var saved = await _context.Projects.FirstOrDefaultAsync(p => p.Title == "Test Project");
        saved.Should().NotBeNull();
        saved!.Id.Should().BeGreaterThan(0);

        // Act - Update
        saved.Status = ProjectStatus.Published;
        await _context.SaveChangesAsync();

        // Assert
        var updated = await _context.Projects.FindAsync(saved.Id);
        updated!.Status.Should().Be(ProjectStatus.Published);

        // Act - Delete
        _context.Projects.Remove(updated);
        await _context.SaveChangesAsync();

        // Assert
        var deleted = await _context.Projects.FindAsync(saved.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task Database_ShouldSetAuditFields_OnSaveChanges()
    {
        // Arrange
        var skill = new Skill
        {
            Name = "Test Skill",
            Category = SkillCategory.Language,
            ProficiencyLevel = ProficiencyLevel.Advanced,
            DisplayOrder = 1
        };

        // Act
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        // Assert
        var saved = await _context.Skills.FirstOrDefaultAsync(s => s.Name == "Test Skill");
        saved.Should().NotBeNull();
        saved!.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        saved.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
