using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Contact.Queries.DTOs;
using Application.Contact.Queries.GetAllContactSubmissions;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Contact.Queries.GetAllContactSubmissions;

public class GetAllContactSubmissionsQueryHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetAllContactSubmissionsQueryHandler _handler;

    public GetAllContactSubmissionsQueryHandlerTests()
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

        _handler = new GetAllContactSubmissionsQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_All_Contact_Submissions()
    {
        // Arrange
        _context.ContactSubmissions.AddRange(
            new ContactSubmission
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com",
                Subject = "Inquiry",
                Message = "Hello",
                IsRead = false,
                SubmittedDate = DateTime.UtcNow.AddDays(-2)
            },
            new ContactSubmission
            {
                Id = 2,
                Name = "Jane Smith",
                Email = "jane@example.com",
                Subject = "Question",
                Message = "Hi there",
                IsRead = true,
                SubmittedDate = DateTime.UtcNow.AddDays(-1)
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllContactSubmissionsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_Should_Order_Submissions_By_SubmittedDate_Descending()
    {
        // Arrange
        var oldestDate = DateTime.UtcNow.AddDays(-5);
        var middleDate = DateTime.UtcNow.AddDays(-2);
        var newestDate = DateTime.UtcNow;

        _context.ContactSubmissions.AddRange(
            new ContactSubmission
            {
                Id = 1,
                Name = "Oldest",
                Email = "old@example.com",
                Message = "Old message",
                IsRead = false,
                SubmittedDate = oldestDate
            },
            new ContactSubmission
            {
                Id = 2,
                Name = "Newest",
                Email = "new@example.com",
                Message = "New message",
                IsRead = false,
                SubmittedDate = newestDate
            },
            new ContactSubmission
            {
                Id = 3,
                Name = "Middle",
                Email = "middle@example.com",
                Message = "Middle message",
                IsRead = false,
                SubmittedDate = middleDate
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllContactSubmissionsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result[0].Name.Should().Be("Newest");
        result[1].Name.Should().Be("Middle");
        result[2].Name.Should().Be("Oldest");
    }

    [Fact]
    public async Task Handle_Should_Return_Both_Read_And_Unread_Submissions()
    {
        // Arrange
        _context.ContactSubmissions.AddRange(
            new ContactSubmission
            {
                Id = 1,
                Name = "Unread User",
                Email = "unread@example.com",
                Message = "Unread message",
                IsRead = false,
                SubmittedDate = DateTime.UtcNow.AddDays(-1)
            },
            new ContactSubmission
            {
                Id = 2,
                Name = "Read User",
                Email = "read@example.com",
                Message = "Read message",
                IsRead = true,
                SubmittedDate = DateTime.UtcNow
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllContactSubmissionsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(s => s.IsRead == true);
        result.Should().Contain(s => s.IsRead == false);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List_When_No_Submissions()
    {
        // Arrange
        // No submissions in database

        // Act
        var query = new GetAllContactSubmissionsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_Include_Submissions_With_Subject()
    {
        // Arrange
        _context.ContactSubmissions.Add(
            new ContactSubmission
            {
                Id = 1,
                Name = "User",
                Email = "user@example.com",
                Subject = "Important Subject",
                Message = "Message with subject",
                IsRead = false,
                SubmittedDate = DateTime.UtcNow
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllContactSubmissionsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Subject.Should().Be("Important Subject");
    }

    [Fact]
    public async Task Handle_Should_Include_Submissions_Without_Subject()
    {
        // Arrange
        _context.ContactSubmissions.Add(
            new ContactSubmission
            {
                Id = 1,
                Name = "User",
                Email = "user@example.com",
                Subject = null,
                Message = "Message without subject",
                IsRead = false,
                SubmittedDate = DateTime.UtcNow
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllContactSubmissionsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Subject.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var submittedDate = new DateTime(2024, 1, 15, 10, 30, 0);
        _context.ContactSubmissions.Add(
            new ContactSubmission
            {
                Id = 5,
                Name = "Test User",
                Email = "test@example.com",
                Subject = "Test Subject",
                Message = "Test Message Content",
                IsRead = true,
                SubmittedDate = submittedDate
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetAllContactSubmissionsQuery();
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(5);
        result[0].Name.Should().Be("Test User");
        result[0].Email.Should().Be("test@example.com");
        result[0].Subject.Should().Be("Test Subject");
        result[0].Message.Should().Be("Test Message Content");
        result[0].IsRead.Should().BeTrue();
        result[0].SubmittedDate.Should().Be(submittedDate);
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
