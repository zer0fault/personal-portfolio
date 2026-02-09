using Application.Common.Data;
using Application.Projects.Queries.DTOs;
using Application.Projects.Queries.GetAllProjectsForAdmin;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Projects.Queries.GetAllProjectsForAdmin;

public class GetAllProjectsForAdminQueryHandlerTests
{
    private readonly GetAllProjectsForAdminQueryHandler _handler;

    public GetAllProjectsForAdminQueryHandlerTests()
    {
        _handler = new GetAllProjectsForAdminQueryHandler();
    }

    [Fact]
    public async Task Handle_Should_Return_All_Projects()
    {
        // Arrange
        var query = new GetAllProjectsForAdminQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2); // StaticDataProvider has 2 projects
        result.Should().AllSatisfy(p => p.Status.Should().Be(ProjectStatus.Published));
    }

    [Fact]
    public async Task Handle_Should_Order_Projects_By_DisplayOrder()
    {
        // Arrange
        var query = new GetAllProjectsForAdminQuery();

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

    [Fact]
    public async Task Handle_Should_Include_All_Project_Details()
    {
        // Arrange
        var query = new GetAllProjectsForAdminQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().AllSatisfy(p =>
        {
            p.Id.Should().BeGreaterThan(0);
            p.Title.Should().NotBeNullOrEmpty();
            p.ShortDescription.Should().NotBeNullOrEmpty();
            p.Status.Should().Be(ProjectStatus.Published);
        });
    }
}
