using Application.Common.Data;
using Application.Projects.Queries.DTOs;
using Application.Projects.Queries.GetAllProjects;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Projects.Queries.GetAllProjects;

public class GetAllProjectsQueryHandlerTests
{
    private readonly GetAllProjectsQueryHandler _handler;

    public GetAllProjectsQueryHandlerTests()
    {
        _handler = new GetAllProjectsQueryHandler();
    }

    [Fact]
    public async Task Handle_Should_Return_Published_Projects_Only()
    {
        // Arrange
        var query = new GetAllProjectsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2); // StaticDataProvider has 2 published projects
        result.Should().AllSatisfy(p => p.Status.Should().Be(ProjectStatus.Published));
    }

    [Fact]
    public async Task Handle_Should_Order_Projects_By_DisplayOrder()
    {
        // Arrange
        var query = new GetAllProjectsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].DisplayOrder.Should().Be(1);
        result[1].DisplayOrder.Should().Be(2);
        result[0].Title.Should().Be("Personal Portfolio Website");
        result[1].Title.Should().Be("Pomodoro TUI");
    }
}
