using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Skills.Queries.DTOs;
using Application.Skills.Queries.GetSkillByIdForAdmin;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Skills.Queries.GetSkillByIdForAdmin;

public class GetSkillByIdForAdminQueryHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetSkillByIdForAdminQueryHandler _handler;

    public GetSkillByIdForAdminQueryHandlerTests()
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

        _handler = new GetSkillByIdForAdminQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Skill_When_Exists()
    {
        // Arrange
        var skill = new Skill
        {
            Id = 1,
            Name = "C#",
            Category = SkillCategory.Language,
            ProficiencyLevel = ProficiencyLevel.Expert,
            DisplayOrder = 1,
            IconUrl = "/icons/csharp.svg"
        };
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSkillByIdForAdminQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("C#");
        result.Category.Should().Be(SkillCategory.Language);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Skill_Not_Found()
    {
        // Arrange
        // No skills in database

        // Act
        var query = new GetSkillByIdForAdminQuery(999);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Return_Skill_With_IconUrl()
    {
        // Arrange
        var skill = new Skill
        {
            Id = 1,
            Name = "Azure",
            Category = SkillCategory.Cloud,
            ProficiencyLevel = ProficiencyLevel.Advanced,
            DisplayOrder = 1,
            IconUrl = "/icons/azure.svg"
        };
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSkillByIdForAdminQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.IconUrl.Should().Be("/icons/azure.svg");
    }

    [Fact]
    public async Task Handle_Should_Return_Skill_Without_IconUrl()
    {
        // Arrange
        var skill = new Skill
        {
            Id = 1,
            Name = "Python",
            Category = SkillCategory.Language,
            ProficiencyLevel = ProficiencyLevel.Intermediate,
            DisplayOrder = 1,
            IconUrl = null
        };
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSkillByIdForAdminQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.IconUrl.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var skill = new Skill
        {
            Id = 5,
            Name = "ASP.NET Core",
            Category = SkillCategory.Framework,
            ProficiencyLevel = ProficiencyLevel.Expert,
            DisplayOrder = 10,
            IconUrl = "/icons/aspnet.svg"
        };
        _context.Skills.Add(skill);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetSkillByIdForAdminQuery(5);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(5);
        result.Name.Should().Be("ASP.NET Core");
        result.Category.Should().Be(SkillCategory.Framework);
        result.DisplayOrder.Should().Be(10);
        result.IconUrl.Should().Be("/icons/aspnet.svg");
    }

    [Fact]
    public async Task Handle_Should_Return_Skill_For_All_Categories()
    {
        // Arrange
        _context.Skills.AddRange(
            new Skill { Id = 1, Name = "C#", Category = SkillCategory.Language, ProficiencyLevel = ProficiencyLevel.Expert, DisplayOrder = 1 },
            new Skill { Id = 2, Name = "React", Category = SkillCategory.Framework, ProficiencyLevel = ProficiencyLevel.Advanced, DisplayOrder = 1 },
            new Skill { Id = 3, Name = "AWS", Category = SkillCategory.Cloud, ProficiencyLevel = ProficiencyLevel.Intermediate, DisplayOrder = 1 },
            new Skill { Id = 4, Name = "Clean Architecture", Category = SkillCategory.Architecture, ProficiencyLevel = ProficiencyLevel.Expert, DisplayOrder = 1 },
            new Skill { Id = 5, Name = "TDD", Category = SkillCategory.Practice, ProficiencyLevel = ProficiencyLevel.Advanced, DisplayOrder = 1 },
            new Skill { Id = 6, Name = "Insurance", Category = SkillCategory.Domain, ProficiencyLevel = ProficiencyLevel.Expert, DisplayOrder = 1 }
        );
        await _context.SaveChangesAsync();

        // Act & Assert for each category
        var result1 = await _handler.Handle(new GetSkillByIdForAdminQuery(1), CancellationToken.None);
        result1!.Category.Should().Be(SkillCategory.Language);

        var result2 = await _handler.Handle(new GetSkillByIdForAdminQuery(2), CancellationToken.None);
        result2!.Category.Should().Be(SkillCategory.Framework);

        var result3 = await _handler.Handle(new GetSkillByIdForAdminQuery(3), CancellationToken.None);
        result3!.Category.Should().Be(SkillCategory.Cloud);

        var result4 = await _handler.Handle(new GetSkillByIdForAdminQuery(4), CancellationToken.None);
        result4!.Category.Should().Be(SkillCategory.Architecture);

        var result5 = await _handler.Handle(new GetSkillByIdForAdminQuery(5), CancellationToken.None);
        result5!.Category.Should().Be(SkillCategory.Practice);

        var result6 = await _handler.Handle(new GetSkillByIdForAdminQuery(6), CancellationToken.None);
        result6!.Category.Should().Be(SkillCategory.Domain);
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
