using Application.Common.Data;
using Application.Common.Mappings;
using Application.Employment.Queries.DTOs;
using Application.Employment.Queries.GetEmploymentByIdForAdmin;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Employment.Queries.GetEmploymentByIdForAdmin;

public class GetEmploymentByIdForAdminQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly GetEmploymentByIdForAdminQueryHandler _handler;

    public GetEmploymentByIdForAdminQueryHandlerTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();
        _handler = new GetEmploymentByIdForAdminQueryHandler(_mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Employment_When_Exists()
    {
        // Arrange
        var query = new GetEmploymentByIdForAdminQuery(1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_NotFound()
    {
        // Arrange
        var query = new GetEmploymentByIdForAdminQuery(999);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
