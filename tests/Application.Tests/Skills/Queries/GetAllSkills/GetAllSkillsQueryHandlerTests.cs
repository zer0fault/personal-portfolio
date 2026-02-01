using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Skills.Queries.DTOs;
using Application.Skills.Queries.GetAllSkills;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Skills.Queries.GetAllSkills;

public class GetAllSkillsQueryHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetAllSkillsQueryHandler _handler;

    public GetAllSkillsQueryHandlerTests()
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

        _handler = new GetAllSkillsQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_All_Skills()
    {
        // Arrange
        _context.Skills.AddRange(
            new Skill
            {
                Id = 1,
                Name = "C#",
                Category = SkillCategory.Language,
                ProficiencyLevel = ProficiencyLevel.Expert,
                DisplayOrder = 1
            },
            new Skill
            {
                Id = 2,
                Name = "ASP.NET Core",
                Category = SkillCategory.Framework,
                ProficiencyLevel = ProficiencyLevel.Advanced,
                DisplayOrder = 1
            },
            new Skill
            {
                Id = 3,
                Name = "Azure",
                Category = SkillCategory.Cloud,
                ProficiencyLevel = ProficiencyLevel.Intermediate,
                DisplayOrder = 1
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllSkillsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task Handle_Should_Order_Skills_By_Category_Then_DisplayOrder()
    {
        // Arrange
        _context.Skills.AddRange(
            new Skill
            {
                Id = 1,
                Name = "Framework 2",
                Category = SkillCategory.Framework,
                ProficiencyLevel = ProficiencyLevel.Advanced,
                DisplayOrder = 2
            },
            new Skill
            {
                Id = 2,
                Name = "Language 1",
                Category = SkillCategory.Language,
                ProficiencyLevel = ProficiencyLevel.Expert,
                DisplayOrder = 1
            },
            new Skill
            {
                Id = 3,
                Name = "Framework 1",
                Category = SkillCategory.Framework,
                ProficiencyLevel = ProficiencyLevel.Advanced,
                DisplayOrder = 1
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllSkillsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Language 1");
        result[1].Name.Should().Be("Framework 1");
        result[2].Name.Should().Be("Framework 2");
    }

    [Fact]
    public async Task Handle_Should_Group_Skills_By_Multiple_Categories()
    {
        // Arrange
        _context.Skills.AddRange(
            new Skill
            {
                Id = 1,
                Name = "C#",
                Category = SkillCategory.Language,
                ProficiencyLevel = ProficiencyLevel.Expert,
                DisplayOrder = 1
            },
            new Skill
            {
                Id = 2,
                Name = "Python",
                Category = SkillCategory.Language,
                ProficiencyLevel = ProficiencyLevel.Intermediate,
                DisplayOrder = 2
            },
            new Skill
            {
                Id = 3,
                Name = "ASP.NET Core",
                Category = SkillCategory.Framework,
                ProficiencyLevel = ProficiencyLevel.Expert,
                DisplayOrder = 1
            },
            new Skill
            {
                Id = 4,
                Name = "Azure",
                Category = SkillCategory.Cloud,
                ProficiencyLevel = ProficiencyLevel.Advanced,
                DisplayOrder = 1
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllSkillsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(4);

        // Verify order: Language (0), Framework (1), Cloud (2)
        result[0].Category.Should().Be(SkillCategory.Language);
        result[1].Category.Should().Be(SkillCategory.Language);
        result[2].Category.Should().Be(SkillCategory.Framework);
        result[3].Category.Should().Be(SkillCategory.Cloud);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Skills()
    {
        // Arrange
        // No skills in database

        // Act
        var query = new GetAllSkillsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Include_Skills_With_IconUrl()
    {
        // Arrange
        _context.Skills.Add(
            new Skill
            {
                Id = 1,
                Name = "C#",
                Category = SkillCategory.Language,
                ProficiencyLevel = ProficiencyLevel.Expert,
                DisplayOrder = 1,
                IconUrl = "/icons/csharp.svg"
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllSkillsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].IconUrl.Should().Be("/icons/csharp.svg");
    }

    [Fact]
    public async Task Handle_Should_Include_Skills_Without_IconUrl()
    {
        // Arrange
        _context.Skills.Add(
            new Skill
            {
                Id = 1,
                Name = "C#",
                Category = SkillCategory.Language,
                ProficiencyLevel = ProficiencyLevel.Expert,
                DisplayOrder = 1,
                IconUrl = null
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllSkillsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].IconUrl.Should().BeNull();
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
