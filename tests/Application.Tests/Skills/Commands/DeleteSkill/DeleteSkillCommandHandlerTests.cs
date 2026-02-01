using Application.Common.Interfaces;
using Application.Skills.Commands.DeleteSkill;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Skills.Commands.DeleteSkill;

public class DeleteSkillCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly DeleteSkillCommandHandler _handler;

    public DeleteSkillCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new DeleteSkillCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_DeleteSkill()
    {
        // Arrange
        var skill = new Skill
        {
            Name = "Test Skill",
            Category = SkillCategory.Language,
            ProficiencyLevel = ProficiencyLevel.Advanced,
            DisplayOrder = 1,
            IconUrl = "https://example.com/test-icon.svg",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        var command = new DeleteSkillCommand { Id = skill.Id };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var deletedSkill = await _context.Skills.FindAsync(skill.Id);
        deletedSkill.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenSkillNotFound()
    {
        // Arrange
        var command = new DeleteSkillCommand { Id = 999 };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Skill not found");
    }

    [Fact]
    public async Task Handle_Should_PhysicallyDeleteSkill()
    {
        // Arrange
        var skill = new Skill
        {
            Name = "C#",
            Category = SkillCategory.Language,
            ProficiencyLevel = ProficiencyLevel.Expert,
            DisplayOrder = 1,
            IconUrl = "https://example.com/csharp.svg",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        var command = new DeleteSkillCommand { Id = skill.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedSkill = await _context.Skills
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == skill.Id);

        deletedSkill.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_NotAffectOtherSkills_WhenDeletingOne()
    {
        // Arrange
        var skill1 = new Skill
        {
            Name = "Skill 1",
            Category = SkillCategory.Language,
            ProficiencyLevel = ProficiencyLevel.Advanced,
            DisplayOrder = 1,
            IconUrl = null,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        var skill2 = new Skill
        {
            Name = "Skill 2",
            Category = SkillCategory.Framework,
            ProficiencyLevel = ProficiencyLevel.Expert,
            DisplayOrder = 2,
            IconUrl = null,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _context.Skills.AddRange(skill1, skill2);
        await _context.SaveChangesAsync();

        var command = new DeleteSkillCommand { Id = skill1.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedSkill = await _context.Skills.FindAsync(skill1.Id);
        deletedSkill.Should().BeNull();

        var remainingSkill = await _context.Skills.FindAsync(skill2.Id);
        remainingSkill.Should().NotBeNull();
        remainingSkill!.Name.Should().Be("Skill 2");
        remainingSkill.Category.Should().Be(SkillCategory.Framework);
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
