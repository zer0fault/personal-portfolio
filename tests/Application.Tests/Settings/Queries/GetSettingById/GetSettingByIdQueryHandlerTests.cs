using Application.Common.Data;
using Application.Settings.Queries.DTOs;
using Application.Settings.Queries.GetSettingById;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Settings.Queries.GetSettingById;

public class GetSettingByIdQueryHandlerTests
{
    private readonly GetSettingByIdQueryHandler _handler;

    public GetSettingByIdQueryHandlerTests()
    {
        _handler = new GetSettingByIdQueryHandler();
    }

    [Fact]
    public async Task Handle_Should_Return_Setting_When_Exists()
    {
        // Arrange
        var query = new GetSettingByIdQuery(1); // ID from StaticDataProvider

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Key.Should().Be("Name");
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_NotFound()
    {
        // Arrange
        var query = new GetSettingByIdQuery(999);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
