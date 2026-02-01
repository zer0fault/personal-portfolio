using Application.Common.Interfaces;
using Application.Settings.Commands.UpdateSetting;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Settings.Commands.UpdateSetting;

public class UpdateSettingCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly UpdateSettingCommandHandler _handler;

    public UpdateSettingCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new UpdateSettingCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_UpdateSetting_WithValidData()
    {
        // Arrange
        var setting = new Domain.Entities.Settings
        {
            Key = "HeroHeadline",
            Value = "Original Value",
            Category = "Hero",
            IsDeleted = false,
            LastModified = DateTime.UtcNow.AddDays(-1),
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            ModifiedDate = DateTime.UtcNow.AddDays(-1)
        };
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();

        var command = new UpdateSettingCommand
        {
            Id = setting.Id,
            Key = "HeroHeadline",
            Value = "Updated Value",
            Category = "Hero"
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedSetting = await _context.Settings.FindAsync(setting.Id);
        updatedSetting.Should().NotBeNull();
        updatedSetting!.Key.Should().Be("HeroHeadline");
        updatedSetting.Value.Should().Be("Updated Value");
        updatedSetting.Category.Should().Be("Hero");
        updatedSetting.LastModified.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        updatedSetting.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenSettingNotFound()
    {
        // Arrange
        var command = new UpdateSettingCommand
        {
            Id = 999,
            Key = "NonExistent",
            Value = "Value",
            Category = "Category"
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Setting with ID 999 not found");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenSettingIsDeleted()
    {
        // Arrange
        var setting = new Domain.Entities.Settings
        {
            Key = "DeletedSetting",
            Value = "Value",
            Category = "Category",
            IsDeleted = true,
            LastModified = DateTime.UtcNow.AddDays(-1),
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            ModifiedDate = DateTime.UtcNow.AddDays(-1)
        };
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();

        var command = new UpdateSettingCommand
        {
            Id = setting.Id,
            Key = "DeletedSetting",
            Value = "New Value",
            Category = "Category"
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_PreserveCreatedDate()
    {
        // Arrange
        var createdDate = DateTime.UtcNow.AddDays(-10);
        var setting = new Domain.Entities.Settings
        {
            Key = "TestKey",
            Value = "Value",
            Category = "Category",
            IsDeleted = false,
            LastModified = DateTime.UtcNow.AddDays(-10),
            CreatedDate = createdDate,
            ModifiedDate = DateTime.UtcNow.AddDays(-10)
        };
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();

        var command = new UpdateSettingCommand
        {
            Id = setting.Id,
            Key = "UpdatedKey",
            Value = "Updated Value",
            Category = "Updated Category"
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedSetting = await _context.Settings.FindAsync(setting.Id);
        updatedSetting.Should().NotBeNull();
        updatedSetting!.CreatedDate.Should().Be(createdDate);
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
