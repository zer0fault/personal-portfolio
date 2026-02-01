using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Employment.Queries.DTOs;
using Application.Employment.Queries.GetEmploymentByIdForAdmin;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Employment.Queries.GetEmploymentByIdForAdmin;

public class GetEmploymentByIdForAdminQueryHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetEmploymentByIdForAdminQueryHandler _handler;

    public GetEmploymentByIdForAdminQueryHandlerTests()
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

        _handler = new GetEmploymentByIdForAdminQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Employment_When_Exists()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            Id = 1,
            CompanyName = "Test Company",
            JobTitle = "Software Engineer",
            StartDate = DateTime.UtcNow.AddYears(-2),
            EndDate = DateTime.UtcNow,
            Responsibilities = "[\"Task 1\", \"Task 2\"]",
            Achievements = "[\"Achievement 1\"]",
            Technologies = "[\"C#\", \".NET\"]",
            DisplayOrder = 1,
            IsDeleted = false
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetEmploymentByIdForAdminQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.CompanyName.Should().Be("Test Company");
        result.JobTitle.Should().Be("Software Engineer");
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Employment_Not_Found()
    {
        // Arrange
        // No employment in database

        // Act
        var query = new GetEmploymentByIdForAdminQuery(999);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Employment_Is_Deleted()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            Id = 1,
            CompanyName = "Deleted Company",
            JobTitle = "Developer",
            StartDate = DateTime.UtcNow.AddYears(-1),
            EndDate = DateTime.UtcNow,
            Responsibilities = "[]",
            Achievements = "[]",
            Technologies = "[]",
            DisplayOrder = 1,
            IsDeleted = true
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetEmploymentByIdForAdminQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Return_Employment_With_Null_EndDate()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            Id = 1,
            CompanyName = "Current Company",
            JobTitle = "Senior Developer",
            StartDate = DateTime.UtcNow.AddYears(-1),
            EndDate = null,
            Responsibilities = "[\"Current task\"]",
            Achievements = "[\"Current achievement\"]",
            Technologies = "[\"C#\"]",
            DisplayOrder = 1,
            IsDeleted = false
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetEmploymentByIdForAdminQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.EndDate.Should().BeNull();
        result.CompanyName.Should().Be("Current Company");
    }

    [Fact]
    public async Task Handle_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            Id = 1,
            CompanyName = "Test Company",
            JobTitle = "Full Stack Developer",
            StartDate = new DateTime(2020, 1, 1),
            EndDate = new DateTime(2023, 12, 31),
            Responsibilities = "[\"Design\", \"Development\", \"Testing\"]",
            Achievements = "[\"Award 1\", \"Award 2\"]",
            Technologies = "[\"C#\", \"JavaScript\", \"SQL\"]",
            DisplayOrder = 5,
            IsDeleted = false
        };
        _context.EmploymentHistory.Add(employment);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetEmploymentByIdForAdminQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.CompanyName.Should().Be("Test Company");
        result.JobTitle.Should().Be("Full Stack Developer");
        result.StartDate.Should().Be(new DateTime(2020, 1, 1));
        result.EndDate.Should().Be(new DateTime(2023, 12, 31));
        result.DisplayOrder.Should().Be(5);
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
