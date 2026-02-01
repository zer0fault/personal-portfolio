using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Settings.Queries.DTOs;
using Application.Settings.Queries.GetSettingById;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Settings.Queries.GetSettingById;

public class GetSettingByIdQueryHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetSettingByIdQueryHandler _handler;

    public GetSettingByIdQueryHandlerTests()
    {
        // Create in-memory database
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);

        // Use the actual MappingProfile
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();

        _handler = new GetSettingByIdQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Setting_When_Exists()
    {
        // Arrange
        var setting = new Domain.Entities.Settings
        {
            Id = 1,
            Key = "HeroHeadline",
            Value = "Welcome to my portfolio",
            Category = "Hero",
            LastModified = DateTime.UtcNow,
            IsDeleted = false
        };
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSettingByIdQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Key.Should().Be("HeroHeadline");
        result.Value.Should().Be("Welcome to my portfolio");
        result.Category.Should().Be("Hero");
    }

    [Fact]
    public async Task Handle_Should_Throw_KeyNotFoundException_When_Setting_Not_Found()
    {
        // Arrange
        // No settings in database

        // Act
        var query = new GetSettingByIdQuery(999);
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Setting with ID 999 not found");
    }

    [Fact]
    public async Task Handle_Should_Throw_KeyNotFoundException_When_Setting_Is_Deleted()
    {
        // Arrange
        var setting = new Domain.Entities.Settings
        {
            Id = 1,
            Key = "DeletedSetting",
            Value = "Deleted value",
            Category = "General",
            LastModified = DateTime.UtcNow,
            IsDeleted = true
        };
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSettingByIdQuery(1);
        var act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Setting with ID 1 not found");
    }

    [Fact]
    public async Task Handle_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var lastModified = new DateTime(2024, 6, 10, 9, 15, 30);
        var setting = new Domain.Entities.Settings
        {
            Id = 5,
            Key = "TestKey",
            Value = "TestValue",
            Category = "TestCategory",
            LastModified = lastModified,
            IsDeleted = false
        };
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSettingByIdQuery(5);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(5);
        result.Key.Should().Be("TestKey");
        result.Value.Should().Be("TestValue");
        result.Category.Should().Be("TestCategory");
        result.LastModified.Should().Be(lastModified);
    }

    [Fact]
    public async Task Handle_Should_Return_Correct_Setting_When_Multiple_Exist()
    {
        // Arrange
        _context.Settings.AddRange(
            new Domain.Entities.Settings
            {
                Id = 1,
                Key = "Setting1",
                Value = "Value1",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 2,
                Key = "Setting2",
                Value = "Value2",
                Category = "About",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 3,
                Key = "Setting3",
                Value = "Value3",
                Category = "Social",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSettingByIdQuery(2);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(2);
        result.Key.Should().Be("Setting2");
        result.Value.Should().Be("Value2");
        result.Category.Should().Be("About");
    }

    [Fact]
    public async Task Handle_Should_Return_Settings_From_Different_Categories()
    {
        // Arrange
        _context.Settings.AddRange(
            new Domain.Entities.Settings
            {
                Id = 1,
                Key = "HeroSetting",
                Value = "Hero Value",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 2,
                Key = "AboutSetting",
                Value = "About Value",
                Category = "About",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 3,
                Key = "SocialSetting",
                Value = "Social Value",
                Category = "Social",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 4,
                Key = "FooterSetting",
                Value = "Footer Value",
                Category = "Footer",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act & Assert
        var result1 = await _handler.Handle(new GetSettingByIdQuery(1), CancellationToken.None);
        result1.Category.Should().Be("Hero");

        var result2 = await _handler.Handle(new GetSettingByIdQuery(2), CancellationToken.None);
        result2.Category.Should().Be("About");

        var result3 = await _handler.Handle(new GetSettingByIdQuery(3), CancellationToken.None);
        result3.Category.Should().Be("Social");

        var result4 = await _handler.Handle(new GetSettingByIdQuery(4), CancellationToken.None);
        result4.Category.Should().Be("Footer");
    }

    [Fact]
    public async Task Handle_Should_Return_Setting_With_Long_Value()
    {
        // Arrange
        var longValue = new string('A', 1000);
        var setting = new Domain.Entities.Settings
        {
            Id = 1,
            Key = "LongSetting",
            Value = longValue,
            Category = "General",
            LastModified = DateTime.UtcNow,
            IsDeleted = false
        };
        _context.Settings.Add(setting);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSettingByIdQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().Be(longValue);
        result.Value.Length.Should().Be(1000);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    // Test DbContext
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
