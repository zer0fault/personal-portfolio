using Application.Projects.Commands.UpdateProject;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandlerTests
{
    private readonly UpdateProjectCommandHandler _handler;

    public UpdateProjectCommandHandlerTests()
    {
        _handler = new UpdateProjectCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithValidData()
    {
        // Arrange
        var command = new UpdateProjectCommand
        {
            Id = 1,
            Title = "Updated Title",
            ShortDescription = "Updated Short",
            FullDescription = "Updated Full",
            Technologies = new List<string> { "New Tech", "C#" },
            GitHubUrl = "https://github.com/updated",
            LiveDemoUrl = "https://updated.com",
            DisplayOrder = 2,
            Status = ProjectStatus.Published
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenTitleIsEmpty()
    {
        // Arrange
        var command = new UpdateProjectCommand
        {
            Id = 1,
            Title = "",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = new List<string>(),
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Title is required");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenShortDescriptionIsEmpty()
    {
        // Arrange
        var command = new UpdateProjectCommand
        {
            Id = 1,
            Title = "Test",
            ShortDescription = "",
            FullDescription = "Full",
            Technologies = new List<string>(),
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Short description is required");
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenFullDescriptionIsEmpty()
    {
        // Arrange
        var command = new UpdateProjectCommand
        {
            Id = 1,
            Title = "Test",
            ShortDescription = "Short",
            FullDescription = "",
            Technologies = new List<string>(),
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Full description is required");
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithAllFields()
    {
        // Arrange
        var command = new UpdateProjectCommand
        {
            Id = 1,
            Title = "Updated",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = new List<string>(),
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }
}
