using Application.Employment.Commands.UpdateEmployment;
using FluentAssertions;
using MediatR;
using Xunit;

namespace Application.Tests.Employment.Commands.UpdateEmployment;

public class UpdateEmploymentCommandHandlerTests
{
    private readonly UpdateEmploymentCommandHandler _handler;

    public UpdateEmploymentCommandHandlerTests()
    {
        _handler = new UpdateEmploymentCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_Return_Unit()
    {
        // Arrange
        var command = new UpdateEmploymentCommand
        {
            Id = 1,
            CompanyName = "Updated Company",
            JobTitle = "Updated Title",
            StartDate = DateTime.UtcNow,
            Responsibilities = new List<string>(),
            Achievements = new List<string>(),
            Technologies = new List<string>(),
            DisplayOrder = 1
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
}

