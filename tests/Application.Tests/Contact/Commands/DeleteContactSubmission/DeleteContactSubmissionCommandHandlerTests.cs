using Application.Common.Interfaces;
using Application.Contact.Commands.DeleteContactSubmission;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.Tests.Contact.Commands.DeleteContactSubmission;

public class DeleteContactSubmissionCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly DeleteContactSubmissionCommandHandler _handler;
    private readonly Mock<ILogger<DeleteContactSubmissionCommandHandler>> _loggerMock;

    public DeleteContactSubmissionCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _loggerMock = new Mock<ILogger<DeleteContactSubmissionCommandHandler>>();
        _handler = new DeleteContactSubmissionCommandHandler(_context, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_DeleteContactSubmission()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Name = "John Doe",
            Email = "john@example.com",
            Subject = "Test Subject",
            Message = "Test message content",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        var command = new DeleteContactSubmissionCommand { Id = submission.Id };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var deletedSubmission = await _context.ContactSubmissions.FindAsync(submission.Id);
        deletedSubmission.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenContactNotFound()
    {
        // Arrange
        var command = new DeleteContactSubmissionCommand { Id = 999 };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Contact submission not found");
    }

    [Fact]
    public async Task Handle_Should_PhysicallyDeleteSubmission()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Name = "Jane Smith",
            Email = "jane@example.com",
            Subject = "Physical Delete Test",
            Message = "Testing physical deletion",
            IsRead = true,
            SubmittedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        var command = new DeleteContactSubmissionCommand { Id = submission.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedSubmission = await _context.ContactSubmissions
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.Id == submission.Id);

        deletedSubmission.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_LogDeletion_ForAuditTrail()
    {
        // Arrange
        var submittedDate = DateTime.UtcNow.AddDays(-1);
        var submission = new ContactSubmission
        {
            Name = "Audit Test",
            Email = "audit@example.com",
            Subject = "Audit Subject",
            Message = "Audit message content",
            IsRead = true,
            SubmittedDate = submittedDate,
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            ModifiedDate = DateTime.UtcNow
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        var command = new DeleteContactSubmissionCommand { Id = submission.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Deleting contact submission {submission.Id}")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_DeleteReadSubmission()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Name = "Read Submission",
            Email = "read@example.com",
            Subject = "Read Subject",
            Message = "This submission was already read",
            IsRead = true,
            SubmittedDate = DateTime.UtcNow.AddDays(-5),
            CreatedDate = DateTime.UtcNow.AddDays(-5),
            ModifiedDate = DateTime.UtcNow.AddDays(-4)
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        var command = new DeleteContactSubmissionCommand { Id = submission.Id };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var deletedSubmission = await _context.ContactSubmissions.FindAsync(submission.Id);
        deletedSubmission.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_DeleteUnreadSubmission()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Name = "Unread Submission",
            Email = "unread@example.com",
            Subject = "Unread Subject",
            Message = "This submission was never read",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        var command = new DeleteContactSubmissionCommand { Id = submission.Id };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var deletedSubmission = await _context.ContactSubmissions.FindAsync(submission.Id);
        deletedSubmission.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_NotAffectOtherSubmissions_WhenDeletingOne()
    {
        // Arrange
        var submission1 = new ContactSubmission
        {
            Name = "User 1",
            Email = "user1@example.com",
            Subject = "Subject 1",
            Message = "Message 1",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        var submission2 = new ContactSubmission
        {
            Name = "User 2",
            Email = "user2@example.com",
            Subject = "Subject 2",
            Message = "Message 2",
            IsRead = true,
            SubmittedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _context.ContactSubmissions.AddRange(submission1, submission2);
        await _context.SaveChangesAsync();

        var command = new DeleteContactSubmissionCommand { Id = submission1.Id };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedSubmission = await _context.ContactSubmissions.FindAsync(submission1.Id);
        deletedSubmission.Should().BeNull();

        var remainingSubmission = await _context.ContactSubmissions.FindAsync(submission2.Id);
        remainingSubmission.Should().NotBeNull();
        remainingSubmission!.Name.Should().Be("User 2");
        remainingSubmission.Email.Should().Be("user2@example.com");
    }

    [Fact]
    public async Task Handle_Should_DeleteMultipleSubmissions()
    {
        // Arrange
        var submission1 = new ContactSubmission
        {
            Name = "Delete 1",
            Email = "delete1@example.com",
            Subject = "Subject 1",
            Message = "Message 1",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        var submission2 = new ContactSubmission
        {
            Name = "Delete 2",
            Email = "delete2@example.com",
            Subject = "Subject 2",
            Message = "Message 2",
            IsRead = true,
            SubmittedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _context.ContactSubmissions.AddRange(submission1, submission2);
        await _context.SaveChangesAsync();

        // Act
        await _handler.Handle(new DeleteContactSubmissionCommand { Id = submission1.Id }, CancellationToken.None);
        await _handler.Handle(new DeleteContactSubmissionCommand { Id = submission2.Id }, CancellationToken.None);

        // Assert
        var remainingSubmissions = await _context.ContactSubmissions.ToListAsync();
        remainingSubmissions.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_Should_DeleteOldSubmission()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Name = "Old Submission",
            Email = "old@example.com",
            Subject = "Old Subject",
            Message = "This is an old submission",
            IsRead = true,
            SubmittedDate = DateTime.UtcNow.AddMonths(-6),
            CreatedDate = DateTime.UtcNow.AddMonths(-6),
            ModifiedDate = DateTime.UtcNow.AddMonths(-6)
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        var command = new DeleteContactSubmissionCommand { Id = submission.Id };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var deletedSubmission = await _context.ContactSubmissions.FindAsync(submission.Id);
        deletedSubmission.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_ReturnTrue_OnSuccessfulDeletion()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Name = "Success Test",
            Email = "success@example.com",
            Subject = "Success Subject",
            Message = "Success message",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        var command = new DeleteContactSubmissionCommand { Id = submission.Id };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
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
}
