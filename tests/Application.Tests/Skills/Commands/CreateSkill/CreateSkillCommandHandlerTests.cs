using Application.Common.Interfaces;
using Application.Skills.Commands.CreateSkill;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Skills.Commands.CreateSkill;

public class CreateSkillCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly CreateSkillCommandHandler _handler;

    public CreateSkillCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new CreateSkillCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_CreateSkill_WithValidData()
    {
        // Arrange
        var command = new CreateSkillCommand
        {
            Name = "C#",
            Category = SkillCategory.Language,
            DisplayOrder = 1,
            IconUrl = "https://example.com/csharp-icon.svg"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
        var skill = await _context.Skills.FindAsync(result);
        skill.Should().NotBeNull();
        skill!.Name.Should().Be("C#");
        skill.Category.Should().Be(SkillCategory.Language);
        skill.DisplayOrder.Should().Be(1);
        skill.IconUrl.Should().Be("https://example.com/csharp-icon.svg");
        skill.ProficiencyLevel.Should().Be(ProficiencyLevel.Advanced);
        skill.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        skill.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_CreateSkill_WithoutIconUrl()
    {
        // Arrange
        var command = new CreateSkillCommand
        {
            Name = "TypeScript",
            Category = SkillCategory.Language,
            DisplayOrder = 7,
            IconUrl = null
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var skill = await _context.Skills.FindAsync(result);
        skill.Should().NotBeNull();
        skill!.Name.Should().Be("TypeScript");
        skill.IconUrl.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_CreateSkill_WithZeroDisplayOrder()
    {
        // Arrange
        var command = new CreateSkillCommand
        {
            Name = "JavaScript",
            Category = SkillCategory.Language,
            DisplayOrder = 0,
            IconUrl = null
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var skill = await _context.Skills.FindAsync(result);
        skill.Should().NotBeNull();
        skill!.DisplayOrder.Should().Be(0);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateSkillCommand
        {
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
        var command = new CreateSkillCommand
        {
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
