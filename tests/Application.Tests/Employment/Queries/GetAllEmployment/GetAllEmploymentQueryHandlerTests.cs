using Application.Common.Data;
using Application.Common.Mappings;
using Application.Employment.Queries.DTOs;
using Application.Employment.Queries.GetAllEmployment;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Employment.Queries.GetAllEmployment;

public class GetAllEmploymentQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly GetAllEmploymentQueryHandler _handler;

    public GetAllEmploymentQueryHandlerTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();
        _handler = new GetAllEmploymentQueryHandler(_mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_All_Employment_From_StaticData()
    {
        // Arrange
        var query = new GetAllEmploymentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3); // 2 Duck Creek positions + 1 Freelance
    }

    [Fact]
    public async Task Handle_Should_OrderBy_DisplayOrder()
    {
        // Arrange
        var query = new GetAllEmploymentQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeInAscendingOrder(e => e.DisplayOrder);
    }
}
