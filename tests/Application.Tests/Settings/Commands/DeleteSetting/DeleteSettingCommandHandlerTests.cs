using Application.Common.Interfaces;
using Application.Settings.Commands.DeleteSetting;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Settings.Commands.DeleteSetting;

public class DeleteSettingCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly DeleteSettingCommandHandler _handler;

    public DeleteSettingCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new DeleteSettingCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_SoftDeleteSetting()
    {
        // Arrange
        var setting = new Domain.Entities.Settings
        {
            Key = "HeroHeadline",
            Value = "Full-Stack Developer",
            Category = "Hero",
            IsDeleted = false,
            LastModified = DateTime.UtcNow.AddDays(-1),
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            ModifiedDate = DateTime.UtcNow.AddDays(-1)
        };
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();

        var command = new DeleteSettingCommand(setting.Id);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedSetting = await _context.Settings
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == setting.Id);

        deletedSetting.Should().NotBeNull();
        deletedSetting!.IsDeleted.Should().BeTrue();
        deletedSetting.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenSettingNotFound()
    {
        // Arrange
        var command = new DeleteSettingCommand(999);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Setting with ID 999 not found");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenSettingAlreadyDeleted()
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

        var command = new DeleteSettingCommand(setting.Id);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Handle_Should_PreserveOtherFields_OnDeletion()
    {
        // Arrange
        var createdDate = DateTime.UtcNow.AddDays(-10);
        var lastModified = DateTime.UtcNow.AddDays(-5);

        var setting = new Domain.Entities.Settings
        {
            Key = "PreserveTest",
            Value = "Original Value",
            Category = "Original Category",
            IsDeleted = false,
            LastModified = lastModified,
            CreatedDate = createdDate,
            ModifiedDate = lastModified
        };
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();

        var command = new DeleteSettingCommand(setting.Id);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedSetting = await _context.Settings
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == setting.Id);

        deletedSetting.Should().NotBeNull();
        deletedSetting!.Key.Should().Be("PreserveTest");
        deletedSetting.Value.Should().Be("Original Value");
        deletedSetting.Category.Should().Be("Original Category");
        deletedSetting.CreatedDate.Should().Be(createdDate);
        deletedSetting.IsDeleted.Should().BeTrue();
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
