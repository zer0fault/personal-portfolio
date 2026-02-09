using Application.Common.Data;
using Application.Employment.Queries.DTOs;
using Application.Employment.Queries.GetAllEmployment;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Employment.Queries.GetAllEmployment;

public class GetAllEmploymentQueryHandlerTests
{
    private readonly GetAllEmploymentQueryHandler _handler;

    public GetAllEmploymentQueryHandlerTests()
    {
        _handler = new GetAllEmploymentQueryHandler();
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
        result.Should().HaveCount(3);
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
