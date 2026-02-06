using Application.Common.Data;
using Application.Contact.Queries.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Contact.Queries.GetAllContactSubmissions;

/// <summary>
/// Handler for GetAllContactSubmissionsQuery
/// </summary>
public class GetAllContactSubmissionsQueryHandler : IRequestHandler<GetAllContactSubmissionsQuery, List<ContactSubmissionDto>>
{
    private readonly IMapper _mapper;

    public GetAllContactSubmissionsQueryHandler(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task<List<ContactSubmissionDto>> Handle(GetAllContactSubmissionsQuery request, CancellationToken cancellationToken)
    {
        var submissions = StaticDataProvider.GetContactSubmissions()
            .OrderByDescending(c => c.SubmittedDate)
            .ToList();

        return await Task.FromResult(_mapper.Map<List<ContactSubmissionDto>>(submissions));
    }
}
