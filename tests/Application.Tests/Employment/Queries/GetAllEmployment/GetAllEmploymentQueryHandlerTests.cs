using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Employment.Queries.DTOs;
using Application.Employment.Queries.GetAllEmployment;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Employment.Queries.GetAllEmployment;

public class GetAllEmploymentQueryHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetAllEmploymentQueryHandler _handler;

    public GetAllEmploymentQueryHandlerTests()
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

        _handler = new GetAllEmploymentQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_All_Non_Deleted_Employment()
    {
        // Arrange
        _context.EmploymentHistory.AddRange(
            new Domain.Entities.Employment
            {
                Id = 1,
                CompanyName = "Company A",
                JobTitle = "Software Engineer",
                StartDate = DateTime.UtcNow.AddYears(-2),
                EndDate = DateTime.UtcNow,
                Responsibilities = "[\"Task 1\", \"Task 2\"]",
                Achievements = "[\"Achievement 1\"]",
                Technologies = "[\"C#\", \".NET\"]",
                DisplayOrder = 1,
                IsDeleted = false
            },
            new Domain.Entities.Employment
            {
                Id = 2,
                CompanyName = "Company B",
                JobTitle = "Senior Developer",
                StartDate = DateTime.UtcNow.AddYears(-3),
                EndDate = DateTime.UtcNow.AddYears(-2),
                Responsibilities = "[\"Task 3\"]",
                Achievements = "[\"Achievement 2\"]",
                Technologies = "[\"Python\"]",
                DisplayOrder = 2,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllEmploymentQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_Should_Not_Return_Deleted_Employment()
    {
        // Arrange
        _context.EmploymentHistory.AddRange(
            new Domain.Entities.Employment
            {
                Id = 1,
                CompanyName = "Active Company",
                JobTitle = "Developer",
                StartDate = DateTime.UtcNow.AddYears(-1),
                EndDate = null,
                Responsibilities = "[]",
                Achievements = "[]",
                Technologies = "[]",
                DisplayOrder = 1,
                IsDeleted = false
            },
            new Domain.Entities.Employment
            {
                Id = 2,
                CompanyName = "Deleted Company",
                JobTitle = "Developer",
                StartDate = DateTime.UtcNow.AddYears(-2),
                EndDate = DateTime.UtcNow.AddYears(-1),
                Responsibilities = "[]",
                Achievements = "[]",
                Technologies = "[]",
                DisplayOrder = 2,
                IsDeleted = true
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllEmploymentQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].CompanyName.Should().Be("Active Company");
    }

    [Fact]
    public async Task Handle_Should_Order_Employment_By_DisplayOrder()
    {
        // Arrange
        _context.EmploymentHistory.AddRange(
            new Domain.Entities.Employment
            {
                Id = 1,
                CompanyName = "Company 3",
                JobTitle = "Developer",
                StartDate = DateTime.UtcNow,
                EndDate = null,
                Responsibilities = "[]",
                Achievements = "[]",
                Technologies = "[]",
                DisplayOrder = 3,
                IsDeleted = false
            },
            new Domain.Entities.Employment
            {
                Id = 2,
                CompanyName = "Company 1",
                JobTitle = "Developer",
                StartDate = DateTime.UtcNow,
                EndDate = null,
                Responsibilities = "[]",
                Achievements = "[]",
                Technologies = "[]",
                DisplayOrder = 1,
                IsDeleted = false
            },
            new Domain.Entities.Employment
            {
                Id = 3,
                CompanyName = "Company 2",
                JobTitle = "Developer",
                StartDate = DateTime.UtcNow,
                EndDate = null,
                Responsibilities = "[]",
                Achievements = "[]",
                Technologies = "[]",
                DisplayOrder = 2,
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllEmploymentQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result[0].CompanyName.Should().Be("Company 1");
        result[1].CompanyName.Should().Be("Company 2");
        result[2].CompanyName.Should().Be("Company 3");
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Employment()
    {
        // Arrange
        // No employment in database

        // Act
        var query = new GetAllEmploymentQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Handle_Current_Employment_With_Null_EndDate()
    {
        // Arrange
        _context.EmploymentHistory.Add(
            new Domain.Entities.Employment
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
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllEmploymentQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].EndDate.Should().BeNull();
        result[0].CompanyName.Should().Be("Current Company");
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
