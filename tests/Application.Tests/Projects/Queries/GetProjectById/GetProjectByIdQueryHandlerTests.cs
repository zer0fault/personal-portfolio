using Application.Common.Data;
using Application.Common.Mappings;
using Application.Projects.Queries.DTOs;
using Application.Projects.Queries.GetProjectById;
using AutoMapper;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Projects.Queries.GetProjectById;

public class GetProjectByIdQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly GetProjectByIdQueryHandler _handler;

    public GetProjectByIdQueryHandlerTests()
    {
        // Use the actual MappingProfile
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();

        _handler = new GetProjectByIdQueryHandler(_mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Published_Project_When_Exists()
    {
        // Arrange - StaticDataProvider has project with Id = 1
        var query = new GetProjectByIdQuery(1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Title.Should().Be("Personal Portfolio Website");
        result.Status.Should().Be(ProjectStatus.Published);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Project_Not_Found()
    {
        // Arrange
        var query = new GetProjectByIdQuery(999);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_Return_Published_Projects_Only()
    {
        // Arrange - All projects in StaticDataProvider are published
        var query = new GetProjectByIdQuery(1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Status.Should().Be(ProjectStatus.Published);
    }

    [Fact]
    public async Task Handle_Should_Return_Second_Project()
    {
        // Arrange - StaticDataProvider has project with Id = 2
        var query = new GetProjectByIdQuery(2);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(2);
        result.Title.Should().Be("E-Commerce Platform");
    }
}
