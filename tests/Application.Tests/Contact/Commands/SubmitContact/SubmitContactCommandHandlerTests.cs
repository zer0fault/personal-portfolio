using Application.Common.Interfaces;
using Application.Contact.Commands.SubmitContact;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Contact.Commands.SubmitContact;

public class SubmitContactCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly SubmitContactCommandHandler _handler;
    private readonly TestDateTime _dateTime;

    public SubmitContactCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _dateTime = new TestDateTime();
        _handler = new SubmitContactCommandHandler(_context, _dateTime);
    }

    [Fact]
    public async Task Handle_Should_CreateContactSubmission_WithValidData()
    {
        // Arrange
        var command = new SubmitContactCommand(
            Name: "John Doe",
            Email: "john.doe@example.com",
            Subject: "Inquiry about your services",
            Message: "I would like to know more about your services and availability."
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
        var submission = await _context.ContactSubmissions.FindAsync(result);
        submission.Should().NotBeNull();
        submission!.Name.Should().Be("John Doe");
        submission.Email.Should().Be("john.doe@example.com");
        submission.Subject.Should().Be("Inquiry about your services");
        submission.Message.Should().Be("I would like to know more about your services and availability.");
        submission.IsRead.Should().BeFalse();
        submission.SubmittedDate.Should().Be(_dateTime.UtcNow);
        submission.CreatedDate.Should().Be(_dateTime.UtcNow);
        submission.ModifiedDate.Should().Be(_dateTime.UtcNow);
    }

    [Fact]
    public async Task Handle_Should_SetIsReadToFalse_ByDefault()
    {
        // Arrange
        var command = new SubmitContactCommand(
            Name: "Jane Smith",
            Email: "jane.smith@example.com",
            Subject: "Question",
            Message: "This is a test message with enough characters."
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var submission = await _context.ContactSubmissions.FindAsync(result);
        submission.Should().NotBeNull();
        submission!.IsRead.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_Should_SetSubmittedDate_ToCurrentUtcTime()
    {
        // Arrange
        var testTime = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        _dateTime.SetUtcNow(testTime);

        var command = new SubmitContactCommand(
            Name: "Test User",
            Email: "test@example.com",
            Subject: "Test Subject",
            Message: "Test message with sufficient length for validation."
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var submission = await _context.ContactSubmissions.FindAsync(result);
        submission.Should().NotBeNull();
        submission!.SubmittedDate.Should().Be(testTime);
        submission.CreatedDate.Should().Be(testTime);
        submission.ModifiedDate.Should().Be(testTime);
    }

    [Fact]
    public async Task Handle_Should_CreateContactSubmission_WithLongMessage()
    {
        // Arrange
        var longMessage = new string('a', 1500);
        var command = new SubmitContactCommand(
            Name: "Test User",
            Email: "test@example.com",
            Subject: "Long message test",
            Message: longMessage
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
        var submission = await _context.ContactSubmissions.FindAsync(result);
        submission.Should().NotBeNull();
        submission!.Message.Should().HaveLength(1500);
        submission.Message.Should().Be(longMessage);
    }

    [Fact]
    public async Task Handle_Should_CreateContactSubmission_WithValidEmail()
    {
        // Arrange
        var command = new SubmitContactCommand(
            Name: "Email Test",
            Email: "valid.email+tag@example.co.uk",
            Subject: "Email validation test",
            Message: "Testing email validation with complex email address."
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
        var submission = await _context.ContactSubmissions.FindAsync(result);
        submission.Should().NotBeNull();
        submission!.Email.Should().Be("valid.email+tag@example.co.uk");
    }

    [Fact]
    public async Task Handle_Should_CreateMultipleContactSubmissions()
    {
        // Arrange
        var command1 = new SubmitContactCommand(
            Name: "User One",
            Email: "user1@example.com",
            Subject: "First submission",
            Message: "This is the first contact submission."
        );

        var command2 = new SubmitContactCommand(
            Name: "User Two",
            Email: "user2@example.com",
            Subject: "Second submission",
            Message: "This is the second contact submission."
        );

        // Act
        var result1 = await _handler.Handle(command1, CancellationToken.None);
        var result2 = await _handler.Handle(command2, CancellationToken.None);

        // Assert
        result1.Should().BeGreaterThan(0);
        result2.Should().BeGreaterThan(0);
        result2.Should().NotBe(result1);

        var submissions = await _context.ContactSubmissions.ToListAsync();
        submissions.Should().HaveCount(2);
        submissions.Should().Contain(s => s.Name == "User One");
        submissions.Should().Contain(s => s.Name == "User Two");
    }

    [Fact]
    public async Task Handle_Should_CreateContactSubmission_WithMinimumValidMessage()
    {
        // Arrange
        var command = new SubmitContactCommand(
            Name: "Test User",
            Email: "test@example.com",
            Subject: "Short message test",
            Message: "Ten chars!" // Exactly 10 characters
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
        var submission = await _context.ContactSubmissions.FindAsync(result);
        submission.Should().NotBeNull();
        submission!.Message.Should().HaveLength(10);
    }

    [Fact]
    public async Task Handle_Should_PersistContactSubmission_ToDatabase()
    {
        // Arrange
        var command = new SubmitContactCommand(
            Name: "Persistence Test",
            Email: "persist@example.com",
            Subject: "Testing persistence",
            Message: "This message should be persisted to the database."
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var submission = await _context.ContactSubmissions
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == result);

        submission.Should().NotBeNull();
        submission!.Name.Should().Be("Persistence Test");
        submission.Email.Should().Be("persist@example.com");
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

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

    private class TestDateTime : IDateTime
    {
        private DateTime _utcNow = DateTime.UtcNow;

        public DateTime UtcNow => _utcNow;

        public void SetUtcNow(DateTime dateTime)
        {
            _utcNow = dateTime;
        }
    }
}
