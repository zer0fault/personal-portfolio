using Application.Common.Data;
using Application.Settings.Queries.DTOs;
using Application.Settings.Queries.GetAllSettings;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Settings.Queries.GetAllSettings;

public class GetAllSettingsQueryHandlerTests
{
    private readonly GetAllSettingsQueryHandler _handler;

    public GetAllSettingsQueryHandlerTests()
    {
        _handler = new GetAllSettingsQueryHandler();
    }

    [Fact]
    public async Task Handle_Should_Return_All_Settings_From_StaticData()
    {
        // Arrange
        var query = new GetAllSettingsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(8); // StaticDataProvider has 8 settings
    }

    [Fact]
    public async Task Handle_Should_Return_Settings_Ordered_By_Category()
    {
        // Arrange
        var query = new GetAllSettingsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        var categories = result.Select(s => s.Category).ToList();
        categories.Should().ContainInConsecutiveOrder("Hero", "About");
    }
}
