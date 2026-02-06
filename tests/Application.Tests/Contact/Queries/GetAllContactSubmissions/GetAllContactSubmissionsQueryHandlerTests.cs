using Application.Common.Data;
using Application.Common.Mappings;
using Application.Contact.Queries.DTOs;
using Application.Contact.Queries.GetAllContactSubmissions;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Contact.Queries.GetAllContactSubmissions;

public class GetAllContactSubmissionsQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly GetAllContactSubmissionsQueryHandler _handler;

    public GetAllContactSubmissionsQueryHandlerTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();

        _handler = new GetAllContactSubmissionsQueryHandler(_mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_All_Submissions()
    {
        // Arrange
        var query = new GetAllContactSubmissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(0); // StaticDataProvider returns empty list for contact submissions
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_List()
    {
        // Arrange
        var query = new GetAllContactSubmissionsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
