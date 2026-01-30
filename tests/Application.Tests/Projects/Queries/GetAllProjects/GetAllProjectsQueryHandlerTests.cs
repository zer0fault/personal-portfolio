using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Projects.Queries.DTOs;
using Application.Projects.Queries.GetAllProjects;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Projects.Queries.GetAllProjects;

public class GetAllProjectsQueryHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetAllProjectsQueryHandler _handler;

    public GetAllProjectsQueryHandlerTests()
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

        _handler = new GetAllProjectsQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Published_Projects_Only()
    {
        // Arrange
        _context.Projects.AddRange(
            new Project
            {
                Id = 1,
                Title = "Published Project",
                Status = ProjectStatus.Published,
                IsDeleted = false,
                DisplayOrder = 1,
                Technologies = "[\"C#\", \".NET\"]",
                ShortDescription = "Test",
                FullDescription = "Test",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new Project
            {
                Id = 2,
                Title = "Draft Project",
                Status = ProjectStatus.Draft,
                IsDeleted = false,
                DisplayOrder = 2,
                Technologies = "[\"Python\"]",
                ShortDescription = "Test",
                FullDescription = "Test",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new Project
            {
                Id = 3,
                Title = "Deleted Project",
                Status = ProjectStatus.Published,
                IsDeleted = true,
                DisplayOrder = 3,
                Technologies = "[\"Java\"]",
                ShortDescription = "Test",
                FullDescription = "Test",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllProjectsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Title.Should().Be("Published Project");
        result[0].Status.Should().Be(ProjectStatus.Published);
    }

    [Fact]
    public async Task Handle_Should_Order_Projects_By_DisplayOrder()
    {
        // Arrange
        _context.Projects.AddRange(
            new Project
            {
                Id = 1,
                Title = "Project 3",
                Status = ProjectStatus.Published,
                IsDeleted = false,
                DisplayOrder = 3,
                Technologies = "[\"Angular\"]",
                ShortDescription = "Test",
                FullDescription = "Test",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new Project
            {
                Id = 2,
                Title = "Project 1",
                Status = ProjectStatus.Published,
                IsDeleted = false,
                DisplayOrder = 1,
                Technologies = "[\"React\"]",
                ShortDescription = "Test",
                FullDescription = "Test",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new Project
            {
                Id = 3,
                Title = "Project 2",
                Status = ProjectStatus.Published,
                IsDeleted = false,
                DisplayOrder = 2,
                Technologies = "[\"Vue\"]",
                ShortDescription = "Test",
                FullDescription = "Test",
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllProjectsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result[0].Title.Should().Be("Project 1");
        result[1].Title.Should().Be("Project 2");
        result[2].Title.Should().Be("Project 3");
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
