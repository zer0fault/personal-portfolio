using Application.Common.Data;
using Application.Common.Mappings;
using Application.Projects.Queries.DTOs;
using Application.Projects.Queries.GetAllProjectsForAdmin;
using AutoMapper;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Projects.Queries.GetAllProjectsForAdmin;

public class GetAllProjectsForAdminQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly GetAllProjectsForAdminQueryHandler _handler;

    public GetAllProjectsForAdminQueryHandlerTests()
    {
        // Use the actual MappingProfile
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();

        _handler = new GetAllProjectsForAdminQueryHandler(_mapper);
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
        result[1].Title.Should().Be("E-Commerce Platform");
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
