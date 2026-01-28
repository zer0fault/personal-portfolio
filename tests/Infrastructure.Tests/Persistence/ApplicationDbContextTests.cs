using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infrastructure.Tests.Persistence;

public class ApplicationDbContextTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly IDateTime _dateTime;

    public ApplicationDbContextTests()
    {
        _dateTime = new DateTimeService();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options, _dateTime);
    }

    [Fact]
    public async Task SaveChangesAsync_Should_Set_CreatedDate_For_New_Entity()
    {
        // Arrange
        var project = new Project
        {
            Title = "Test Project",
            ShortDescription = "Short description",
            FullDescription = "Full description",
            Technologies = "[\"C#\", \".NET\"]",
            Status = ProjectStatus.Draft,
            DisplayOrder = 1
        };

        // Act
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Assert
        project.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        project.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task SaveChangesAsync_Should_Update_ModifiedDate_For_Modified_Entity()
    {
        // Arrange
        var project = new Project
        {
            Title = "Test Project",
            ShortDescription = "Short description",
            FullDescription = "Full description",
            Technologies = "[\"C#\"]",
            Status = ProjectStatus.Draft,
            DisplayOrder = 1
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        var originalModifiedDate = project.ModifiedDate;

        // Wait a bit to ensure time difference
        await Task.Delay(10);

        // Act
        project.Title = "Updated Project";
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();

        // Assert
        project.ModifiedDate.Should().BeAfter(originalModifiedDate);
        project.CreatedDate.Should().BeCloseTo(originalModifiedDate, TimeSpan.FromMilliseconds(100)); // Should not change significantly
    }

    [Fact]
    public async Task Projects_Should_Have_Cascade_Delete_For_Images()
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

        var image = new ProjectImage
        {
            ImagePath = "/images/test.jpg",
            AltText = "Test image",
            DisplayOrder = 1,
            IsThumbnail = true,
            Project = project
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Act
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        // Assert
        var imageExists = await _context.ProjectImages.AnyAsync(i => i.Id == image.Id);
        imageExists.Should().BeFalse("Images should be deleted when project is deleted");
    }

    [Fact]
    public async Task Employment_Should_Support_Soft_Delete()
    {
        // Arrange
        var employment = new Employment
        {
            CompanyName = "Test Company",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow.AddYears(-2),
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
    public async Task ContactSubmissions_Should_Be_Saved_Correctly()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject",
            Message = "Test message content",
            SubmittedDate = DateTime.UtcNow,
            IsRead = false
        };

        // Act
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        // Assert
        var retrieved = await _context.ContactSubmissions.FindAsync(submission.Id);
        retrieved.Should().NotBeNull();
        retrieved!.Name.Should().Be("John Doe");
        retrieved.IsRead.Should().BeFalse();
    }

    [Fact]
    public async Task Skills_Should_Be_Ordered_By_Category_And_DisplayOrder()
    {
        // Arrange
        var skills = new[]
        {
            new Skill
            {
                Name = "C#",
                Category = SkillCategory.Language,
                ProficiencyLevel = ProficiencyLevel.Expert,
                DisplayOrder = 2
            },
            new Skill
            {
                Name = "JavaScript",
                Category = SkillCategory.Language,
                ProficiencyLevel = ProficiencyLevel.Advanced,
                DisplayOrder = 1
            },
            new Skill
            {
                Name = "Azure",
                Category = SkillCategory.Cloud,
                ProficiencyLevel = ProficiencyLevel.Intermediate,
                DisplayOrder = 1
            }
        };

        _context.Skills.AddRange(skills);
        await _context.SaveChangesAsync();

        // Act
        var retrieved = await _context.Skills
            .OrderBy(s => s.Category)
            .ThenBy(s => s.DisplayOrder)
            .ToListAsync();

        // Assert
        retrieved.Should().HaveCount(3);
        retrieved[0].Name.Should().Be("JavaScript"); // Language, DisplayOrder 1
        retrieved[1].Name.Should().Be("C#");         // Language, DisplayOrder 2
        retrieved[2].Name.Should().Be("Azure");      // Cloud, DisplayOrder 1
    }

    [Fact]
    public async Task Settings_Should_Be_Queryable_By_Key()
    {
        // Arrange
        var setting = new Settings
        {
            Key = "HeroHeadline",
            Value = "Welcome to my portfolio",
            Category = "Hero",
            LastModified = DateTime.UtcNow
        };

        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();

        // Act
        var retrieved = await _context.Settings
            .FirstOrDefaultAsync(s => s.Key == "HeroHeadline");

        // Assert
        retrieved.Should().NotBeNull();
        retrieved!.Value.Should().Be("Welcome to my portfolio");
        retrieved.Category.Should().Be("Hero");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
