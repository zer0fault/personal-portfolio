using Domain.Entities;
using FluentAssertions;

namespace Domain.Tests.Entities;

public class ContactSubmissionTests
{
    [Fact]
    public void ContactSubmission_ShouldInitialize_WithDefaultValues()
    {
        // Arrange & Act
        var submission = new ContactSubmission();

        // Assert
        submission.Id.Should().Be(0);
        submission.Name.Should().BeEmpty();
        submission.Email.Should().BeEmpty();
        submission.Subject.Should().BeNull();
        submission.Message.Should().BeEmpty();
        submission.IsRead.Should().BeFalse();
        submission.SubmittedDate.Should().Be(default);
    }

    [Fact]
    public void ContactSubmission_ShouldAllowSettingProperties()
    {
        // Arrange
        var submission = new ContactSubmission();
        var submittedDate = DateTime.UtcNow;
        var createdDate = DateTime.UtcNow;
        var modifiedDate = DateTime.UtcNow.AddMinutes(5);

        // Act
        submission.Id = 1;
        submission.Name = "John Doe";
        submission.Email = "john.doe@example.com";
        submission.Subject = "Job Opportunity";
        submission.Message = "I would like to discuss a position at our company.";
        submission.IsRead = false;
        submission.SubmittedDate = submittedDate;
        submission.CreatedDate = createdDate;
        submission.ModifiedDate = modifiedDate;

        // Assert
        submission.Id.Should().Be(1);
        submission.Name.Should().Be("John Doe");
        submission.Email.Should().Be("john.doe@example.com");
        submission.Subject.Should().Be("Job Opportunity");
        submission.Message.Should().Be("I would like to discuss a position at our company.");
        submission.IsRead.Should().BeFalse();
        submission.SubmittedDate.Should().Be(submittedDate);
        submission.CreatedDate.Should().Be(createdDate);
        submission.ModifiedDate.Should().Be(modifiedDate);
    }

    [Fact]
    public void ContactSubmission_IsRead_ShouldDefaultToFalse()
    {
        // Arrange & Act
        var submission = new ContactSubmission
        {
            Name = "Jane Smith",
            Email = "jane@example.com",
            Subject = "Question",
            Message = "I have a question about your work."
        };

        // Assert
        submission.IsRead.Should().BeFalse();
    }

    [Fact]
    public void ContactSubmission_CanBeMarkedAsRead()
    {
        // Arrange
        var submission = new ContactSubmission
        {
            Name = "Test User",
            Email = "test@example.com",
            Subject = "Test",
            Message = "Test message",
            IsRead = false
        };

        // Act
        submission.IsRead = true;

        // Assert
        submission.IsRead.Should().BeTrue();
    }

    [Fact]
    public void ContactSubmission_ShouldStoreSubmittedDate()
    {
        // Arrange
        var expectedDate = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        var submission = new ContactSubmission();

        // Act
        submission.SubmittedDate = expectedDate;

        // Assert
        submission.SubmittedDate.Should().Be(expectedDate);
    }

    [Theory]
    [InlineData("Short message")]
    [InlineData("This is a much longer message that contains multiple sentences and spans across several lines to test that the entity can handle larger text content without any issues.")]
    public void ContactSubmission_ShouldHandleVariousMessageLengths(string message)
    {
        // Arrange
        var submission = new ContactSubmission();

        // Act
        submission.Message = message;

        // Assert
        submission.Message.Should().Be(message);
    }
}
