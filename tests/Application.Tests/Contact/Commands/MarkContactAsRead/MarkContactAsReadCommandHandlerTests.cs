using Application.Common.Interfaces;
using Application.Contact.Commands.MarkContactAsRead;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Application.Tests.Contact.Commands.MarkContactAsRead;

public class MarkContactAsReadCommandHandlerTests : IDisposable
{
    private readonly TestDbContext _context;
    private readonly MarkContactAsReadCommandHandler _handler;

    public MarkContactAsReadCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestDbContext(options);
        _handler = new MarkContactAsReadCommandHandler(_context);
    }

    [Fact]
    public async Task Handle_Should_MarkContactAsRead()
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

        var command = new MarkContactAsReadCommand
        {
            Id = submission.Id,
            IsRead = true
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var updatedSubmission = await _context.ContactSubmissions.FindAsync(submission.Id);
        updatedSubmission.Should().NotBeNull();
        updatedSubmission!.IsRead.Should().BeTrue();
        updatedSubmission.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenContactNotFound()
    {
        // Arrange
        var command = new MarkContactAsReadCommand
        {
            Id = 999,
            IsRead = true
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage("Contact submission not found");
    }

    [Fact]
    public async Task Handle_Should_UpdateModifiedDate()
    {
        // Arrange
        var originalModifiedDate = DateTime.UtcNow.AddDays(-1);
        var submission = new ContactSubmission
        {
            Name = "Test User",
            Email = "test@example.com",
            Subject = "Test Subject",
            Message = "Test message content",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow.AddDays(-1),
            CreatedDate = DateTime.UtcNow.AddDays(-1),
            ModifiedDate = originalModifiedDate
        };
        _context.ContactSubmissions.Add(submission);
        await _context.SaveChangesAsync();

        var command = new MarkContactAsReadCommand
        {
            Id = submission.Id,
            IsRead = true
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedSubmission = await _context.ContactSubmissions.FindAsync(submission.Id);
        updatedSubmission.Should().NotBeNull();
        updatedSubmission!.ModifiedDate.Should().BeAfter(originalModifiedDate);
        updatedSubmission.ModifiedDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Fact]
    public async Task Handle_Should_NotAffectOtherSubmissions()
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
            IsRead = false,
            SubmittedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        _context.ContactSubmissions.AddRange(submission1, submission2);
        await _context.SaveChangesAsync();

        var command = new MarkContactAsReadCommand
        {
            Id = submission1.Id,
            IsRead = true
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedSubmission1 = await _context.ContactSubmissions.FindAsync(submission1.Id);
        var updatedSubmission2 = await _context.ContactSubmissions.FindAsync(submission2.Id);

        updatedSubmission1!.IsRead.Should().BeTrue();
        updatedSubmission2!.IsRead.Should().BeFalse();
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
