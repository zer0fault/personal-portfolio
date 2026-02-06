using Application.Projects.Commands.CreateProject;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Projects.Commands.CreateProject;

public class CreateProjectCommandHandlerTests
{
    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        _handler = new CreateProjectCommandHandler();
    }

    [Fact]
    public async Task Handle_Should_CreateProject_WithValidData()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Title = "Test Project",
            ShortDescription = "A test project description",
            FullDescription = "This is a full description of the test project",
            Technologies = new List<string> { "C#", ".NET", "Azure" },
            GitHubUrl = "https://github.com/test/repo",
            LiveDemoUrl = "https://test.com",
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WithTechnologies()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Title = "Tech Test",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = new List<string> { "React", "TypeScript", "Node.js" },
            DisplayOrder = 1,
            Status = ProjectStatus.Published
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_WhenTitleIsEmpty()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
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
        var command = new CreateProjectCommand
        {
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
        var command = new CreateProjectCommand
        {
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
    public async Task Handle_Should_ReturnSuccess_WithoutOptionalUrls()
    {
        // Arrange
        var command = new CreateProjectCommand
        {
            Title = "Minimal Project",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = new List<string> { "C#" },
            GitHubUrl = null,
            LiveDemoUrl = null,
            DisplayOrder = 1,
            Status = ProjectStatus.Draft
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
    }
}
