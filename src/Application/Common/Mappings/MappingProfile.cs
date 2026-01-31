using Application.Contact.Commands;
using Application.Contact.Queries.DTOs;
using Application.Employment.Queries.DTOs;
using Application.Projects.Queries.DTOs;
using Application.Settings.Queries.DTOs;
using Application.Skills.Queries.DTOs;
using AutoMapper;
using Domain.Entities;
using System.Text.Json;

namespace Application.Common.Mappings;

/// <summary>
/// AutoMapper configuration profile for entity-to-DTO mappings
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Project mappings
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => DeserializeJsonArray(src.Technologies)))
            .ForMember(dest => dest.ThumbnailPath, opt => opt.MapFrom(src =>
                src.Images.FirstOrDefault(i => i.IsThumbnail) != null
                    ? src.Images.FirstOrDefault(i => i.IsThumbnail)!.ImagePath
                    : string.Empty));

        CreateMap<Project, ProjectDetailDto>()
            .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => DeserializeJsonArray(src.Technologies)));

        CreateMap<ProjectImage, ProjectImageDto>();

        // Employment mappings
        CreateMap<Domain.Entities.Employment, EmploymentDto>()
            .ForMember(dest => dest.Responsibilities, opt => opt.MapFrom(src => DeserializeJsonArray(src.Responsibilities)))
            .ForMember(dest => dest.Achievements, opt => opt.MapFrom(src => DeserializeJsonArray(src.Achievements)))
            .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => DeserializeJsonArray(src.Technologies)));

        // Skill mappings
        CreateMap<Skill, SkillDto>();

        // Settings mappings
        CreateMap<Domain.Entities.Settings, SettingsDto>();

        // Contact mappings
        CreateMap<Application.Contact.Commands.ContactSubmissionDto, ContactSubmission>()
            .ForMember(dest => dest.SubmittedDate, opt => opt.Ignore())
            .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedDate, opt => opt.Ignore());

        // Contact admin query mapping
        CreateMap<ContactSubmission, Application.Contact.Queries.DTOs.ContactSubmissionDto>();
    }

    /// <summary>
    /// Deserializes a JSON array string into a List of strings
    /// </summary>
    private static List<string> DeserializeJsonArray(string jsonArray)
    {
        if (string.IsNullOrWhiteSpace(jsonArray))
            return new List<string>();

        try
        {
            return JsonSerializer.Deserialize<List<string>>(jsonArray) ?? new List<string>();
        }
        catch (JsonException)
        {
            return new List<string>();
        }
    }
}
