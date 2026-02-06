using Application.Common.Data;
using Application.Common.Mappings;
using Application.Settings.Queries.DTOs;
using Application.Settings.Queries.GetSettingsByCategory;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Settings.Queries.GetSettingsByCategory;

public class GetSettingsByCategoryQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly GetSettingsByCategoryQueryHandler _handler;

    public GetSettingsByCategoryQueryHandlerTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();
        _handler = new GetSettingsByCategoryQueryHandler(_mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Settings_For_Hero_Category()
    {
        // Arrange
        var query = new GetSettingsByCategoryQuery("Hero");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(5); // StaticDataProvider has 5 Hero settings
        result.Should().OnlyContain(s => s.Category == "Hero");
    }

    [Fact]
    public async Task Handle_Should_Return_Settings_For_About_Category()
    {
        // Arrange
        var query = new GetSettingsByCategoryQuery("About");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(3); // StaticDataProvider has 3 About settings
        result.Should().OnlyContain(s => s.Category == "About");
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_For_Nonexistent_Category()
    {
        // Arrange
        var query = new GetSettingsByCategoryQuery("NonexistentCategory");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
