using Application.Common.Data;
using Application.Common.Mappings;
using Application.Projects.Queries.DTOs;
using Application.Projects.Queries.GetProjectByIdForAdmin;
using AutoMapper;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Projects.Queries.GetProjectByIdForAdmin;

public class GetProjectByIdForAdminQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly GetProjectByIdForAdminQueryHandler _handler;

    public GetProjectByIdForAdminQueryHandlerTests()
    {
        // Use the actual MappingProfile
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();

        _handler = new GetProjectByIdForAdminQueryHandler(_mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Published_Project()
    {
        // Arrange - StaticDataProvider has project with Id = 1
        var query = new GetProjectByIdForAdminQuery(1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Title.Should().Be("Personal Portfolio Website");
        result.Status.Should().Be(ProjectStatus.Published);
    }

    [Fact]
    public async Task Handle_Should_Return_Any_Status_Project()
    {
        // Arrange - All projects in StaticDataProvider are published
        var query = new GetProjectByIdForAdminQuery(2);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(2);
        result.Status.Should().Be(ProjectStatus.Published);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Project_Not_Found()
    {
        // Arrange
        var query = new GetProjectByIdForAdminQuery(999);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Include_All_Project_Details()
    {
        // Arrange
        var query = new GetProjectByIdForAdminQuery(1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Title.Should().NotBeNullOrEmpty();
        result.ShortDescription.Should().NotBeNullOrEmpty();
        result.FullDescription.Should().NotBeNullOrEmpty();
        result.Technologies.Should().NotBeNull();
    }
}
