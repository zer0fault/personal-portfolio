using Application.Common.Data;
using Application.Employment.Queries.DTOs;
using Application.Employment.Queries.GetEmploymentByIdForAdmin;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Employment.Queries.GetEmploymentByIdForAdmin;

public class GetEmploymentByIdForAdminQueryHandlerTests
{
    private readonly GetEmploymentByIdForAdminQueryHandler _handler;

    public GetEmploymentByIdForAdminQueryHandlerTests()
    {
        _handler = new GetEmploymentByIdForAdminQueryHandler();
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
