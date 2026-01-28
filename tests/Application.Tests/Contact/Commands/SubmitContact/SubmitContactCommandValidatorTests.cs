using Application.Contact.Commands.SubmitContact;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Contact.Commands.SubmitContact;

public class SubmitContactCommandValidatorTests
{
    private readonly SubmitContactCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        // Arrange
        var command = new SubmitContactCommand("", "test@example.com", "Subject", "This is a test message");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        // Arrange
        var command = new SubmitContactCommand(new string('a', 101), "test@example.com", "Subject", "This is a test message");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        // Arrange
        var command = new SubmitContactCommand("John Doe", "", "Subject", "This is a test message");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        // Arrange
        var command = new SubmitContactCommand("John Doe", "invalid-email", "Subject", "This is a test message");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Should_Have_Error_When_Subject_Is_Empty()
    {
        // Arrange
        var command = new SubmitContactCommand("John Doe", "test@example.com", "", "This is a test message");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Subject");
    }

    [Fact]
    public void Should_Have_Error_When_Message_Is_Too_Short()
    {
        // Arrange
        var command = new SubmitContactCommand("John Doe", "test@example.com", "Subject", "Short");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Message");
    }

    [Fact]
    public void Should_Have_Error_When_Message_Exceeds_MaxLength()
    {
        // Arrange
        var command = new SubmitContactCommand("John Doe", "test@example.com", "Subject", new string('a', 2001));

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Message");
    }

    [Fact]
    public void Should_Pass_Validation_With_Valid_Command()
    {
        // Arrange
        var command = new SubmitContactCommand(
            "John Doe",
            "john.doe@example.com",
            "Test Subject",
            "This is a valid test message with sufficient length.");

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
