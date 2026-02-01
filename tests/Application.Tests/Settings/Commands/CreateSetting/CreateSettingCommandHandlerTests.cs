using Application.Common.Interfaces;
using Application.Settings.Commands.CreateSetting;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Settings.Commands.CreateSetting;

public class CreateSettingCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly CreateSettingCommandHandler _handler;

    public CreateSettingCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new CreateSettingCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_CreateSetting_WithValidData()
    {
        // Arrange
        var command = new CreateSettingCommand
        {
            Key = "HeroHeadline",
            Value = "Full-Stack Developer",
            Category = "Hero"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
        var setting = await _context.Settings.FindAsync(result);
        setting.Should().NotBeNull();
        setting!.Key.Should().Be("HeroHeadline");
        setting.Value.Should().Be("Full-Stack Developer");
        setting.Category.Should().Be("Hero");
        setting.IsDeleted.Should().BeFalse();
        setting.LastModified.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        setting.CreatedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        setting.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_CreateMultipleSettings()
    {
        // Arrange
        var command1 = new CreateSettingCommand
        {
            Key = "Setting1",
            Value = "Value1",
            Category = "Category1"
        };

        var command2 = new CreateSettingCommand
        {
            Key = "Setting2",
            Value = "Value2",
            Category = "Category2"
        };

        // Act
        var result1 = await _handler.Handle(command1, CancellationToken.None);
        var result2 = await _handler.Handle(command2, CancellationToken.None);

        // Assert
        result1.Should().BeGreaterThan(0);
        result2.Should().BeGreaterThan(0);
        result2.Should().NotBe(result1);

        var settings = await _context.Settings.ToListAsync();
        settings.Should().HaveCount(2);
        settings.Should().Contain(s => s.Key == "Setting1");
        settings.Should().Contain(s => s.Key == "Setting2");
    }

    [Fact]
    public async Task Handle_Should_AllowDuplicateKeys_InDifferentCategories()
    {
        // Arrange
        var command1 = new CreateSettingCommand
        {
            Key = "Title",
            Value = "Hero Title",
            Category = "Hero"
        };

        var command2 = new CreateSettingCommand
        {
            Key = "Title",
            Value = "About Title",
            Category = "About"
        };

        // Act
        var result1 = await _handler.Handle(command1, CancellationToken.None);
        var result2 = await _handler.Handle(command2, CancellationToken.None);

        // Assert
        var settings = await _context.Settings.Where(s => s.Key == "Title").ToListAsync();
        settings.Should().HaveCount(2);
        settings.Should().Contain(s => s.Category == "Hero" && s.Value == "Hero Title");
        settings.Should().Contain(s => s.Category == "About" && s.Value == "About Title");
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
