using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Contact.Queries.DTOs;
using Application.Contact.Queries.GetContactSubmissionById;
using AutoMapper;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Contact.Queries.GetContactSubmissionById;

public class GetContactSubmissionByIdQueryHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly IMapper _mapper;
    private readonly GetContactSubmissionByIdQueryHandler _handler;

    public GetContactSubmissionByIdQueryHandlerTests()
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

        _handler = new GetContactSubmissionByIdQueryHandler(_context, _mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Contact_Submission_When_Exists()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject",
            Message = "Test Message",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetContactSubmissionByIdQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("john@example.com");
        result.Subject.Should().Be("Test Subject");
        result.Message.Should().Be("Test Message");
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Submission_Not_Found()
    {
        // Arrange
        // No submissions in database

        // Act
        var query = new GetContactSubmissionByIdQuery(999);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Return_Submission_With_Subject()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Id = 1,
            Name = "User",
            Email = "user@example.com",
            Subject = "Important Subject",
            Message = "Message",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetContactSubmissionByIdQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Subject.Should().Be("Important Subject");
    }

    [Fact]
    public async Task Handle_Should_Return_Submission_Without_Subject()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Id = 1,
            Name = "User",
            Email = "user@example.com",
            Subject = null,
            Message = "Message",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetContactSubmissionByIdQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Subject.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Return_Unread_Submission()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Id = 1,
            Name = "User",
            Email = "user@example.com",
            Message = "Message",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetContactSubmissionByIdQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.IsRead.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_Should_Return_Read_Submission()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Id = 1,
            Name = "User",
            Email = "user@example.com",
            Message = "Message",
            IsRead = true,
            SubmittedDate = DateTime.UtcNow
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetContactSubmissionByIdQuery(1);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.IsRead.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_Map_All_Properties_Correctly()
    {
        // Arrange
        var submittedDate = new DateTime(2024, 3, 15, 14, 30, 0);
        var submission = new ContactSubmission
        {
            Id = 10,
            Name = "Test User",
            Email = "test@example.com",
            Subject = "Test Subject Line",
            Message = "Detailed test message content",
            IsRead = true,
            SubmittedDate = submittedDate
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        // Act
        var query = new GetContactSubmissionByIdQuery(10);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(10);
        result.Name.Should().Be("Test User");
        result.Email.Should().Be("test@example.com");
        result.Subject.Should().Be("Test Subject Line");
        result.Message.Should().Be("Detailed test message content");
        result.IsRead.Should().BeTrue();
        result.SubmittedDate.Should().Be(submittedDate);
    }

    [Fact]
    public async Task Handle_Should_Return_Correct_Submission_When_Multiple_Exist()
    {
        // Arrange
        _context.ContactSubmissions.AddRange(
            new ContactSubmission
            {
                Id = 1,
                Name = "User 1",
                Email = "user1@example.com",
                Message = "Message 1",
                IsRead = false,
                SubmittedDate = DateTime.UtcNow.AddDays(-2)
            },
            new ContactSubmission
            {
                Id = 2,
                Name = "User 2",
                Email = "user2@example.com",
                Message = "Message 2",
                IsRead = true,
                SubmittedDate = DateTime.UtcNow.AddDays(-1)
            },
            new ContactSubmission
            {
                Id = 3,
                Name = "User 3",
                Email = "user3@example.com",
                Message = "Message 3",
                IsRead = false,
                SubmittedDate = DateTime.UtcNow
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var query = new GetContactSubmissionByIdQuery(2);
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(2);
        result.Name.Should().Be("User 2");
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
