using Application.Contact.Commands.SubmitContact;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Contact.Commands.SubmitContact;

public class SubmitContactCommandHandlerTests
{
    private readonly SubmitContactCommandHandler _handler;

    public SubmitContactCommandHandlerTests()
    {
        _handler = new SubmitContactCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithValidData()
    {
        // Arrange
        var command = new SubmitContactCommand(
            Name: "John Doe",
            Email: "john.doe@example.com",
            Subject: "Inquiry",
            Message: "I would like to know more about your services."
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithLongMessage()
    {
        // Arrange
        var longMessage = new string('a', 1500);
        var command = new SubmitContactCommand(
            Name: "Test User",
            Email: "test@example.com",
            Subject: "Long message",
            Message: longMessage
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithValidEmail()
    {
        // Arrange
        var command = new SubmitContactCommand(
            Name: "Email Test",
            Email: "valid.email+tag@example.co.uk",
            Subject: "Email validation",
            Message: "Testing email validation."
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
    }
}
