using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Settings.Queries.DTOs;
using Application.Settings.Queries.GetSettingsByCategory;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Settings.Queries.GetSettingsByCategory;

public class GetSettingsByCategoryQueryHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetSettingsByCategoryQueryHandler _handler;

    public GetSettingsByCategoryQueryHandlerTests()
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

        _handler = new GetSettingsByCategoryQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Settings_For_Specified_Category()
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
                Key = "HeroSubheadline",
                Value = "Subtitle",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 3,
                Key = "AboutBio",
                Value = "Bio text",
                Category = "About",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSettingsByCategoryQuery("Hero");
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(s => s.Category.Should().Be("Hero"));
    }

    [Fact]
    public async Task Handle_Should_Not_Return_Settings_From_Other_Categories()
    {
        // Arrange
        _context.Settings.AddRange(
            new Domain.Entities.Settings
            {
                Id = 1,
                Key = "HeroSetting",
                Value = "Value",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 2,
                Key = "AboutSetting",
                Value = "Value",
                Category = "About",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 3,
                Key = "SocialSetting",
                Value = "Value",
                Category = "Social",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSettingsByCategoryQuery("About");
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Category.Should().Be("About");
        result[0].Key.Should().Be("AboutSetting");
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
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 2,
                Key = "DeletedSetting",
                Value = "Deleted",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = true
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSettingsByCategoryQuery("Hero");
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Key.Should().Be("ActiveSetting");
    }

    [Fact]
    public async Task Handle_Should_Order_Settings_By_Key()
    {
        // Arrange
        _context.Settings.AddRange(
            new Domain.Entities.Settings
            {
                Id = 1,
                Key = "ZSetting",
                Value = "Value",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 2,
                Key = "ASetting",
                Value = "Value",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 3,
                Key = "MSetting",
                Value = "Value",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSettingsByCategoryQuery("Hero");
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result[0].Key.Should().Be("ASetting");
        result[1].Key.Should().Be("MSetting");
        result[2].Key.Should().Be("ZSetting");
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_Category_Not_Found()
    {
        // Arrange
        _context.Settings.Add(
            new Domain.Entities.Settings
            {
                Id = 1,
                Key = "HeroSetting",
                Value = "Value",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSettingsByCategoryQuery("NonExistentCategory");
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Settings()
    {
        // Arrange
        // No settings in database

        // Act
        var query = new GetSettingsByCategoryQuery("Hero");
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Be_Case_Sensitive_For_Category()
    {
        // Arrange
        _context.Settings.AddRange(
            new Domain.Entities.Settings
            {
                Id = 1,
                Key = "Setting1",
                Value = "Value",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            },
            new Domain.Entities.Settings
            {
                Id = 2,
                Key = "Setting2",
                Value = "Value",
                Category = "hero",
                LastModified = DateTime.UtcNow,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSettingsByCategoryQuery("Hero");
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Category.Should().Be("Hero");
    }

    [Fact]
    public async Task Handle_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var lastModified = new DateTime(2024, 5, 20, 15, 45, 0);
        _context.Settings.Add(
            new Domain.Entities.Settings
            {
                Id = 10,
                Key = "TestKey",
                Value = "TestValue",
                Category = "TestCategory",
                LastModified = lastModified,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSettingsByCategoryQuery("TestCategory");
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(10);
        result[0].Key.Should().Be("TestKey");
        result[0].Value.Should().Be("TestValue");
        result[0].Category.Should().Be("TestCategory");
        result[0].LastModified.Should().Be(lastModified);
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
