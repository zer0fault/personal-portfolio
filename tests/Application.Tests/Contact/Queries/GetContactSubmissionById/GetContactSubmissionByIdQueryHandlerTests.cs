using Application.Common.Data;
using Application.Common.Mappings;
using Application.Contact.Queries.DTOs;
using Application.Contact.Queries.GetContactSubmissionById;
using AutoMapper;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Contact.Queries.GetContactSubmissionById;

public class GetContactSubmissionByIdQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly GetContactSubmissionByIdQueryHandler _handler;

    public GetContactSubmissionByIdQueryHandlerTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = configuration.CreateMapper();

        _handler = new GetContactSubmissionByIdQueryHandler(_mapper);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Not_Found()
    {
        // Arrange
        var query = new GetContactSubmissionByIdQuery(1);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull(); // StaticDataProvider returns empty list for contact submissions
    }

    [Fact]
    public async Task Handle_Should_Return_Null_For_AnyId()
    {
        // Arrange
        var query = new GetContactSubmissionByIdQuery(999);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}
