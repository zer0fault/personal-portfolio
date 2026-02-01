using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Projects.Queries.DTOs;
using Application.Projects.Queries.GetProjectByIdForAdmin;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Projects.Queries.GetProjectByIdForAdmin;

public class GetProjectByIdForAdminQueryHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetProjectByIdForAdminQueryHandler _handler;

    public GetProjectByIdForAdminQueryHandlerTests()
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

        _handler = new GetProjectByIdForAdminQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Published_Project()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Published Project",
            Status = ProjectStatus.Published,
            IsDeleted = false,
            DisplayOrder = 1,
            Technologies = "[\"C#\", \".NET\"]",
            ShortDescription = "Short description",
            FullDescription = "Full description",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetProjectByIdForAdminQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Title.Should().Be("Published Project");
        result.Status.Should().Be(ProjectStatus.Published);
    }

    [Fact]
    public async Task Handle_Should_Return_Draft_Project()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Draft Project",
            Status = ProjectStatus.Draft,
            IsDeleted = false,
            DisplayOrder = 1,
            Technologies = "[\"C#\"]",
            ShortDescription = "Test",
            FullDescription = "Test",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetProjectByIdForAdminQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(ProjectStatus.Draft);
    }

    [Fact]
    public async Task Handle_Should_Return_Archived_Project()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Archived Project",
            Status = ProjectStatus.Archived,
            IsDeleted = false,
            DisplayOrder = 1,
            Technologies = "[\"C#\"]",
            ShortDescription = "Test",
            FullDescription = "Test",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetProjectByIdForAdminQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(ProjectStatus.Archived);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Project_Not_Found()
    {
        // Arrange
        // No projects in database

        // Act
        var query = new GetProjectByIdForAdminQuery(999);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Project_Is_Deleted()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Deleted Project",
            Status = ProjectStatus.Published,
            IsDeleted = true,
            DisplayOrder = 1,
            Technologies = "[\"C#\"]",
            ShortDescription = "Test",
            FullDescription = "Test",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetProjectByIdForAdminQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Include_Project_Images()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Project with Images",
            Status = ProjectStatus.Draft,
            IsDeleted = false,
            DisplayOrder = 1,
            Technologies = "[\"C#\"]",
            ShortDescription = "Test",
            FullDescription = "Test",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.Projects.Add(project);

        var image1 = new ProjectImage
        {
            Id = 1,
            ProjectId = 1,
            ImagePath = "/images/image1.jpg",
            AltText = "Image 1",
            DisplayOrder = 1
        };
        var image2 = new ProjectImage
        {
            Id = 2,
            ProjectId = 1,
            ImagePath = "/images/image2.jpg",
            AltText = "Image 2",
            DisplayOrder = 2
        };
        _context.ProjectImages.AddRange(image1, image2);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetProjectByIdForAdminQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Images.Should().HaveCount(2);
        result.Images.Should().Contain(i => i.ImagePath == "/images/image1.jpg");
        result.Images.Should().Contain(i => i.ImagePath == "/images/image2.jpg");
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
