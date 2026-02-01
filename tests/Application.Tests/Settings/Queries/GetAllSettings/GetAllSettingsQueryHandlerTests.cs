using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Settings.Queries.DTOs;
using Application.Settings.Queries.GetAllSettings;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Settings.Queries.GetAllSettings;

public class GetAllSettingsQueryHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetAllSettingsQueryHandler _handler;

    public GetAllSettingsQueryHandlerTests()
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

        _handler = new GetAllSettingsQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_All_Non_Deleted_Settings()
    {
        // Arrange
        _context.Settings.AddRange(
            new Domain.Entities.Settings
            {
                Id = 1,
                Key = "HeroHeadline",
                Value = "Welcome",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 2,
                Key = "AboutBio",
                Value = "Bio text",
                Category = "About",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 3,
                Key = "FooterText",
                Value = "Footer",
                Category = "Footer",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllSettingsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task Handle_Should_Not_Return_Deleted_Settings()
    {
        // Arrange
        _context.Settings.AddRange(
            new Domain.Entities.Settings
            {
                Id = 1,
                Key = "ActiveSetting",
                Value = "Active",
                Category = "General",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 2,
                Key = "DeletedSetting",
                Value = "Deleted",
                Category = "General",
                LastModified = DateTime.UtcNow,
                IsDeleted = true
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllSettingsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Key.Should().Be("ActiveSetting");
    }

    [Fact]
    public async Task Handle_Should_Order_Settings_By_Category_Then_Key()
    {
        // Arrange
        _context.Settings.AddRange(
            new Domain.Entities.Settings
            {
                Id = 1,
                Key = "ZKey",
                Value = "Value",
                Category = "Category2",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 2,
                Key = "AKey",
                Value = "Value",
                Category = "Category1",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 3,
                Key = "BKey",
                Value = "Value",
                Category = "Category1",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllSettingsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result[0].Category.Should().Be("Category1");
        result[0].Key.Should().Be("AKey");
        result[1].Category.Should().Be("Category1");
        result[1].Key.Should().Be("BKey");
        result[2].Category.Should().Be("Category2");
        result[2].Key.Should().Be("ZKey");
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Settings()
    {
        // Arrange
        // No settings in database

        // Act
        var query = new GetAllSettingsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Group_Settings_By_Category()
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
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 3,
                Key = "Setting3",
                Value = "Value3",
                Category = "About",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllSettingsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Where(s => s.Category == "Hero").Should().HaveCount(2);
        result.Where(s => s.Category == "About").Should().HaveCount(1);
    }

    [Fact]
    public async Task Handle_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var lastModified = new DateTime(2024, 2, 15, 10, 30, 0);
        _context.Settings.Add(
            new Domain.Entities.Settings
            {
                Id = 5,
                Key = "TestKey",
                Value = "TestValue",
                Category = "TestCategory",
                LastModified = lastModified,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllSettingsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(5);
        result[0].Key.Should().Be("TestKey");
        result[0].Value.Should().Be("TestValue");
        result[0].Category.Should().Be("TestCategory");
        result[0].LastModified.Should().Be(lastModified);
    }

    [Fact]
    public async Task Handle_Should_Handle_Multiple_Categories()
    {
        // Arrange
        _context.Settings.AddRange(
            new Domain.Entities.Settings { Id = 1, Key = "Key1", Value = "Val", Category = "Hero", LastModified = DateTime.UtcNow, IsDeleted = false },
            new Domain.Entities.Settings { Id = 2, Key = "Key2", Value = "Val", Category = "About", LastModified = DateTime.UtcNow, IsDeleted = false },
            new Domain.Entities.Settings { Id = 3, Key = "Key3", Value = "Val", Category = "Social", LastModified = DateTime.UtcNow, IsDeleted = false },
            new Domain.Entities.Settings { Id = 4, Key = "Key4", Value = "Val", Category = "Footer", LastModified = DateTime.UtcNow, IsDeleted = false }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllSettingsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(4);
        result.Select(s => s.Category).Distinct().Should().HaveCount(4);
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
