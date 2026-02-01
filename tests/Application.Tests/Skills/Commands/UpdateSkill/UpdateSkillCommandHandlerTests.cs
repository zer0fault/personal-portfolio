using Application.Common.Interfaces;
using Application.Skills.Commands.UpdateSkill;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Skills.Commands.UpdateSkill;

public class UpdateSkillCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly UpdateSkillCommandHandler _handler;

    public UpdateSkillCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new UpdateSkillCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_UpdateSkill_WithValidData()
    {
        // Arrange
        var skill = new Skill
        {
            Name = "Original Skill",
            Category = SkillCategory.Language,
            ProficiencyLevel = ProficiencyLevel.Intermediate,
            DisplayOrder = 1,
            IconUrl = "https://example.com/old-icon.svg",
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            ModifiedDate = DateTime.UtcNow.AddDays(-1)
        };
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        var command = new UpdateSkillCommand
        {
            Id = skill.Id,
            Name = "Updated Skill",
            Category = SkillCategory.Framework,
            DisplayOrder = 2,
            IconUrl = "https://example.com/new-icon.svg"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var updatedSkill = await _context.Skills.FindAsync(skill.Id);
        updatedSkill.Should().NotBeNull();
        updatedSkill!.Name.Should().Be("Updated Skill");
        updatedSkill.Category.Should().Be(SkillCategory.Framework);
        updatedSkill.DisplayOrder.Should().Be(2);
        updatedSkill.IconUrl.Should().Be("https://example.com/new-icon.svg");
        updatedSkill.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenSkillNotFound()
    {
        // Arrange
        var command = new UpdateSkillCommand
        {
            Id = 999,
            Name = "Non-existent Skill",
            Category = SkillCategory.Language,
            DisplayOrder = 1,
            IconUrl = null
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Skill not found");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var skill = new Skill
        {
            Name = "Original Skill",
            Category = SkillCategory.Language,
            ProficiencyLevel = ProficiencyLevel.Advanced,
            DisplayOrder = 1,
            IconUrl = null,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        var command = new UpdateSkillCommand
        {
            Id = skill.Id,
            Name = "",
            Category = SkillCategory.Language,
            DisplayOrder = 1,
            IconUrl = null
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Skill name is required");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenNameIsWhitespace()
    {
        // Arrange
        var skill = new Skill
        {
            Name = "Original Skill",
            Category = SkillCategory.Framework,
            ProficiencyLevel = ProficiencyLevel.Expert,
            DisplayOrder = 1,
            IconUrl = null,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        var command = new UpdateSkillCommand
        {
            Id = skill.Id,
            Name = "   ",
            Category = SkillCategory.Framework,
            DisplayOrder = 1,
            IconUrl = null
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Skill name is required");
    }

    [Fact]
    public async Task Handle_Should_UpdateModifiedDate_NotCreatedDate()
    {
        // Arrange
        var originalCreatedDate = DateTime.UtcNow.AddDays(-10);
        var skill = new Skill
        {
            Name = "Original Skill",
            Category = SkillCategory.Language,
            ProficiencyLevel = ProficiencyLevel.Advanced,
            DisplayOrder = 1,
            IconUrl = null,
            CreatedDate = originalCreatedDate,
            ModifiedDate = originalCreatedDate
        };
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        var command = new UpdateSkillCommand
        {
            Id = skill.Id,
            Name = "Updated Skill",
            Category = SkillCategory.Framework,
            DisplayOrder = 2,
            IconUrl = null
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedSkill = await _context.Skills.FindAsync(skill.Id);
        updatedSkill!.CreatedDate.Should().Be(originalCreatedDate);
        updatedSkill.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_UpdateIconUrl_ToNull()
    {
        // Arrange
        var skill = new Skill
        {
            Name = "Vue.js",
            Category = SkillCategory.Framework,
            ProficiencyLevel = ProficiencyLevel.Intermediate,
            DisplayOrder = 1,
            IconUrl = "https://example.com/vue.svg",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        var command = new UpdateSkillCommand
        {
            Id = skill.Id,
            Name = "Vue.js",
            Category = SkillCategory.Framework,
            DisplayOrder = 1,
            IconUrl = null
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedSkill = await _context.Skills.FindAsync(skill.Id);
        updatedSkill!.IconUrl.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_PreserveProficiencyLevel_WhenUpdating()
    {
        // Arrange
        var skill = new Skill
        {
            Name = "Python",
            Category = SkillCategory.Language,
            ProficiencyLevel = ProficiencyLevel.Expert,
            DisplayOrder = 1,
            IconUrl = null,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        var command = new UpdateSkillCommand
        {
            Id = skill.Id,
            Name = "Python 3",
            Category = SkillCategory.Language,
            DisplayOrder = 2,
            IconUrl = "https://example.com/python.svg"
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedSkill = await _context.Skills.FindAsync(skill.Id);
        updatedSkill!.ProficiencyLevel.Should().Be(ProficiencyLevel.Expert);
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
