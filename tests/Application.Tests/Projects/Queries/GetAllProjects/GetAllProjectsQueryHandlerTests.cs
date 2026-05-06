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
        var query = new GetAllProjectsQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(StaticDataProvider.GetProjectsData().Count);
        result.Should().AllSatisfy(p => p.Status.Should().Be(ProjectStatus.Published));
    }

    [Fact]
    public async Task Handle_Should_Order_Projects_By_DisplayOrder()
    {
        var query = new GetAllProjectsQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        var projectsData = StaticDataProvider.GetProjectsData();
        result.Should().NotBeNull();
        result.Should().HaveCount(projectsData.Count);
        result.Select(p => p.DisplayOrder).Should().Equal(Enumerable.Range(1, projectsData.Count));
        result.Select(p => p.Title).Should().Equal(projectsData.Select(p => p.Title));
    }
}
