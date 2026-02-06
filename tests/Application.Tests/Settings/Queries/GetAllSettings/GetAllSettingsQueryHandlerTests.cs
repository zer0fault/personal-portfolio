using Application.Common.Data;
using Application.Common.Mappings;
using Application.Settings.Queries.DTOs;
using Application.Settings.Queries.GetAllSettings;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Settings.Queries.GetAllSettings;

public class GetAllSettingsQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly GetAllSettingsQueryHandler _handler;

    public GetAllSettingsQueryHandlerTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();
        _handler = new GetAllSettingsQueryHandler(_mapper);
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
        categories.Should().ContainInConsecutiveOrder("About", "Hero");
    }
}
