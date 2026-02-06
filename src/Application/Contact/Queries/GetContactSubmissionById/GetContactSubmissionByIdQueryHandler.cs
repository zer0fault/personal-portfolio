using Application.Common.Data;
using Application.Contact.Queries.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Contact.Queries.GetContactSubmissionById;

/// <summary>
/// Handler for GetContactSubmissionByIdQuery
/// </summary>
public class GetContactSubmissionByIdQueryHandler : IRequestHandler<GetContactSubmissionByIdQuery, ContactSubmissionDto?>
{
    private readonly IMapper _mapper;

    public GetContactSubmissionByIdQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<ContactSubmissionDto?> Handle(GetContactSubmissionByIdQuery request, CancellationToken cancellationToken)
    {
        var submission = StaticDataProvider.GetContactSubmissions()
            .FirstOrDefault(c => c.Id == request.Id);

        return await Task.FromResult(submission == null ? null : _mapper.Map<ContactSubmissionDto>(submission));
    }
}
