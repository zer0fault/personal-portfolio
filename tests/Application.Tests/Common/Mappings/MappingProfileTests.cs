using Application.Common.Mappings;
using Application.Contact.Commands;
using Application.Employment.Queries.DTOs;
using Application.Projects.Queries.DTOs;
using Application.Settings.Queries.DTOs;
using Application.Skills.Queries.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Common.Mappings;

public class MappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configuration;

    public MappingProfileTests()
    {
        _configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public void MappingProfile_Should_Have_Valid_Configuration()
    {
        // Act & Assert
        _configuration.AssertConfigurationIsValid();
    }

    #region Project Mappings

    [Fact]
    public void Should_Map_Project_To_ProjectDto()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Test Project",
            ShortDescription = "A test project",
            FullDescription = "Full description",
            Technologies = "[\"C#\", \"ASP.NET Core\", \"Azure\"]",
            GitHubUrl = "https://github.com/test/repo",
            LiveDemoUrl = "https://demo.example.com",
            DisplayOrder = 1,
            Status = ProjectStatus.Published,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            Images = new List<ProjectImage>
            {
                new ProjectImage
                {
                    Id = 1,
                    ProjectId = 1,
                    ImagePath = "/images/thumbnail.jpg",
                    AltText = "Thumbnail",
                    DisplayOrder = 1,
                    IsThumbnail = true
                },
                new ProjectImage
                {
                    Id = 2,
                    ProjectId = 1,
                    ImagePath = "/images/detail.jpg",
                    AltText = "Detail",
                    DisplayOrder = 2,
                    IsThumbnail = false
                }
            }
        };

        // Act
        var result = _mapper.Map<ProjectDto>(project);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Title.Should().Be("Test Project");
        result.ShortDescription.Should().Be("A test project");
        result.Technologies.Should().HaveCount(3);
        result.Technologies.Should().Contain("C#");
        result.Technologies.Should().Contain("ASP.NET Core");
        result.Technologies.Should().Contain("Azure");
        result.GitHubUrl.Should().Be("https://github.com/test/repo");
        result.LiveDemoUrl.Should().Be("https://demo.example.com");
        result.ThumbnailPath.Should().Be("/images/thumbnail.jpg");
        result.Status.Should().Be(ProjectStatus.Published);
        result.DisplayOrder.Should().Be(1);
    }

    [Fact]
    public void Should_Map_Project_To_ProjectDto_With_Empty_ThumbnailPath_When_No_Thumbnail_Exists()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Test Project",
            ShortDescription = "A test project",
            Technologies = "[\"C#\"]",
            DisplayOrder = 1,
            Status = ProjectStatus.Published,
            Images = new List<ProjectImage>
            {
                new ProjectImage
                {
                    Id = 1,
                    ProjectId = 1,
                    ImagePath = "/images/detail.jpg",
                    AltText = "Detail",
                    IsThumbnail = false
                }
            }
        };

        // Act
        var result = _mapper.Map<ProjectDto>(project);

        // Assert
        result.Should().NotBeNull();
        result.ThumbnailPath.Should().Be(string.Empty);
    }

    [Fact]
    public void Should_Map_Project_To_ProjectDto_With_Empty_ThumbnailPath_When_Images_Collection_Is_Empty()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Test Project",
            ShortDescription = "A test project",
            Technologies = "[]",
            DisplayOrder = 1,
            Status = ProjectStatus.Draft,
            Images = new List<ProjectImage>()
        };

        // Act
        var result = _mapper.Map<ProjectDto>(project);

        // Assert
        result.Should().NotBeNull();
        result.ThumbnailPath.Should().Be(string.Empty);
        result.Technologies.Should().BeEmpty();
    }

    [Fact]
    public void Should_Map_Project_To_ProjectDto_With_Empty_Technologies_When_Json_Is_Empty()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Test Project",
            ShortDescription = "A test project",
            Technologies = "[]",
            DisplayOrder = 1,
            Status = ProjectStatus.Published,
            Images = new List<ProjectImage>()
        };

        // Act
        var result = _mapper.Map<ProjectDto>(project);

        // Assert
        result.Should().NotBeNull();
        result.Technologies.Should().NotBeNull();
        result.Technologies.Should().BeEmpty();
    }

    [Fact]
    public void Should_Map_Project_To_ProjectDto_With_Empty_Technologies_When_Json_Is_Null()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Test Project",
            ShortDescription = "A test project",
            Technologies = null!,
            DisplayOrder = 1,
            Status = ProjectStatus.Published,
            Images = new List<ProjectImage>()
        };

        // Act
        var result = _mapper.Map<ProjectDto>(project);

        // Assert
        result.Should().NotBeNull();
        result.Technologies.Should().NotBeNull();
        result.Technologies.Should().BeEmpty();
    }

    [Fact]
    public void Should_Map_Project_To_ProjectDto_With_Empty_Technologies_When_Json_Is_Invalid()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Test Project",
            ShortDescription = "A test project",
            Technologies = "invalid json",
            DisplayOrder = 1,
            Status = ProjectStatus.Published,
            Images = new List<ProjectImage>()
        };

        // Act
        var result = _mapper.Map<ProjectDto>(project);

        // Assert
        result.Should().NotBeNull();
        result.Technologies.Should().NotBeNull();
        result.Technologies.Should().BeEmpty();
    }

    [Fact]
    public void Should_Map_Project_To_ProjectDetailDto()
    {
        // Arrange
        var project = new Project
        {
            Id = 2,
            Title = "Detailed Project",
            ShortDescription = "Short desc",
            FullDescription = "This is a comprehensive description of the project.",
            Technologies = "[\"React\", \"TypeScript\", \"Node.js\"]",
            GitHubUrl = "https://github.com/test/detailed",
            LiveDemoUrl = null,
            DisplayOrder = 2,
            Status = ProjectStatus.Draft,
            Images = new List<ProjectImage>
            {
                new ProjectImage
                {
                    Id = 3,
                    ProjectId = 2,
                    ImagePath = "/images/screenshot1.jpg",
                    AltText = "Screenshot 1",
                    DisplayOrder = 1,
                    IsThumbnail = false
                },
                new ProjectImage
                {
                    Id = 4,
                    ProjectId = 2,
                    ImagePath = "/images/screenshot2.jpg",
                    AltText = "Screenshot 2",
                    DisplayOrder = 2,
                    IsThumbnail = false
                }
            }
        };

        // Act
        var result = _mapper.Map<ProjectDetailDto>(project);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(2);
        result.Title.Should().Be("Detailed Project");
        result.ShortDescription.Should().Be("Short desc");
        result.FullDescription.Should().Be("This is a comprehensive description of the project.");
        result.Technologies.Should().HaveCount(3);
        result.Technologies.Should().Contain("React");
        result.Technologies.Should().Contain("TypeScript");
        result.Technologies.Should().Contain("Node.js");
        result.GitHubUrl.Should().Be("https://github.com/test/detailed");
        result.LiveDemoUrl.Should().BeNull();
        result.Status.Should().Be(ProjectStatus.Draft);
        result.Images.Should().HaveCount(2);
        result.Images[0].ImagePath.Should().Be("/images/screenshot1.jpg");
        result.Images[1].ImagePath.Should().Be("/images/screenshot2.jpg");
    }

    [Fact]
    public void Should_Map_Project_To_ProjectDetailDto_With_Empty_Technologies()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Test Project",
            ShortDescription = "Short",
            FullDescription = "Full",
            Technologies = "[]",
            DisplayOrder = 1,
            Status = ProjectStatus.Published,
            Images = new List<ProjectImage>()
        };

        // Act
        var result = _mapper.Map<ProjectDetailDto>(project);

        // Assert
        result.Should().NotBeNull();
        result.Technologies.Should().NotBeNull();
        result.Technologies.Should().BeEmpty();
        result.Images.Should().BeEmpty();
    }

    [Fact]
    public void Should_Map_ProjectImage_To_ProjectImageDto()
    {
        // Arrange
        var projectImage = new ProjectImage
        {
            Id = 5,
            ProjectId = 1,
            ImagePath = "/images/project-image.webp",
            AltText = "Beautiful project screenshot",
            DisplayOrder = 3,
            IsThumbnail = true
        };

        // Act
        var result = _mapper.Map<ProjectImageDto>(projectImage);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(5);
        result.ImagePath.Should().Be("/images/project-image.webp");
        result.AltText.Should().Be("Beautiful project screenshot");
        result.DisplayOrder.Should().Be(3);
        result.IsThumbnail.Should().BeTrue();
    }

    #endregion

    #region Employment Mappings

    [Fact]
    public void Should_Map_Employment_To_EmploymentDto()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            Id = 1,
            CompanyName = "Tech Corp",
            JobTitle = "Senior Software Engineer",
            StartDate = new DateTime(2020, 1, 15),
            EndDate = new DateTime(2023, 6, 30),
            Responsibilities = "[\"Lead development team\", \"Code review\", \"Mentoring\"]",
            Achievements = "[\"Improved performance by 50%\", \"Reduced bugs by 30%\"]",
            Technologies = "[\"C#\", \"Azure\", \"SQL Server\"]",
            DisplayOrder = 1,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<EmploymentDto>(employment);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.CompanyName.Should().Be("Tech Corp");
        result.JobTitle.Should().Be("Senior Software Engineer");
        result.StartDate.Should().Be(new DateTime(2020, 1, 15));
        result.EndDate.Should().Be(new DateTime(2023, 6, 30));
        result.Responsibilities.Should().HaveCount(3);
        result.Responsibilities.Should().Contain("Lead development team");
        result.Responsibilities.Should().Contain("Code review");
        result.Responsibilities.Should().Contain("Mentoring");
        result.Achievements.Should().HaveCount(2);
        result.Achievements.Should().Contain("Improved performance by 50%");
        result.Achievements.Should().Contain("Reduced bugs by 30%");
        result.Technologies.Should().HaveCount(3);
        result.Technologies.Should().Contain("C#");
        result.Technologies.Should().Contain("Azure");
        result.Technologies.Should().Contain("SQL Server");
        result.DisplayOrder.Should().Be(1);
    }

    [Fact]
    public void Should_Map_Employment_To_EmploymentDto_With_Null_EndDate()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            Id = 2,
            CompanyName = "Current Company",
            JobTitle = "Tech Lead",
            StartDate = new DateTime(2023, 7, 1),
            EndDate = null,
            Responsibilities = "[\"Architecture design\"]",
            Achievements = "[]",
            Technologies = "[\"Kubernetes\"]",
            DisplayOrder = 0
        };

        // Act
        var result = _mapper.Map<EmploymentDto>(employment);

        // Assert
        result.Should().NotBeNull();
        result.EndDate.Should().BeNull();
        result.Responsibilities.Should().HaveCount(1);
        result.Achievements.Should().BeEmpty();
        result.Technologies.Should().HaveCount(1);
    }

    [Fact]
    public void Should_Map_Employment_To_EmploymentDto_With_Empty_Json_Arrays()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            Id = 3,
            CompanyName = "Test Corp",
            JobTitle = "Developer",
            StartDate = new DateTime(2022, 1, 1),
            EndDate = null,
            Responsibilities = "[]",
            Achievements = "[]",
            Technologies = "[]",
            DisplayOrder = 1
        };

        // Act
        var result = _mapper.Map<EmploymentDto>(employment);

        // Assert
        result.Should().NotBeNull();
        result.Responsibilities.Should().NotBeNull();
        result.Responsibilities.Should().BeEmpty();
        result.Achievements.Should().NotBeNull();
        result.Achievements.Should().BeEmpty();
        result.Technologies.Should().NotBeNull();
        result.Technologies.Should().BeEmpty();
    }

    [Fact]
    public void Should_Map_Employment_To_EmploymentDto_With_Invalid_Json()
    {
        // Arrange
        var employment = new Domain.Entities.Employment
        {
            Id = 4,
            CompanyName = "Test Corp",
            JobTitle = "Developer",
            StartDate = new DateTime(2022, 1, 1),
            Responsibilities = "invalid json",
            Achievements = "{not an array}",
            Technologies = "null",
            DisplayOrder = 1
        };

        // Act
        var result = _mapper.Map<EmploymentDto>(employment);

        // Assert
        result.Should().NotBeNull();
        result.Responsibilities.Should().NotBeNull();
        result.Responsibilities.Should().BeEmpty();
        result.Achievements.Should().NotBeNull();
        result.Achievements.Should().BeEmpty();
        result.Technologies.Should().NotBeNull();
        result.Technologies.Should().BeEmpty();
    }

    #endregion

    #region Skill Mappings

    [Fact]
    public void Should_Map_Skill_To_SkillDto()
    {
        // Arrange
        var skill = new Skill
        {
            Id = 1,
            Name = "C#",
            Category = SkillCategory.Language,
            ProficiencyLevel = ProficiencyLevel.Expert,
            DisplayOrder = 1,
            IconUrl = "/icons/csharp.svg",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<SkillDto>(skill);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("C#");
        result.Category.Should().Be(SkillCategory.Language);
        result.DisplayOrder.Should().Be(1);
        result.IconUrl.Should().Be("/icons/csharp.svg");
    }

    [Fact]
    public void Should_Map_Skill_To_SkillDto_With_Null_IconUrl()
    {
        // Arrange
        var skill = new Skill
        {
            Id = 2,
            Name = "Problem Solving",
            Category = SkillCategory.Practice,
            ProficiencyLevel = ProficiencyLevel.Advanced,
            DisplayOrder = 10,
            IconUrl = null,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<SkillDto>(skill);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(2);
        result.Name.Should().Be("Problem Solving");
        result.Category.Should().Be(SkillCategory.Practice);
        result.IconUrl.Should().BeNull();
    }

    [Fact]
    public void Should_Map_Skill_To_SkillDto_For_All_Categories()
    {
        // Arrange & Act & Assert
        foreach (SkillCategory category in Enum.GetValues(typeof(SkillCategory)))
        {
            var skill = new Skill
            {
                Id = (int)category,
                Name = $"Test Skill {category}",
                Category = category,
                ProficiencyLevel = ProficiencyLevel.Intermediate,
                DisplayOrder = 1
            };

            var result = _mapper.Map<SkillDto>(skill);

            result.Should().NotBeNull();
            result.Category.Should().Be(category);
        }
    }

    #endregion

    #region Settings Mappings

    [Fact]
    public void Should_Map_Settings_To_SettingsDto()
    {
        // Arrange
        var lastModified = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        var settings = new Domain.Entities.Settings
        {
            Id = 1,
            Key = "HeroHeadline",
            Value = "Full Stack Developer",
            Category = "Hero",
            LastModified = lastModified,
            IsDeleted = false,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<SettingsDto>(settings);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Key.Should().Be("HeroHeadline");
        result.Value.Should().Be("Full Stack Developer");
        result.Category.Should().Be("Hero");
        result.LastModified.Should().Be(lastModified);
    }

    [Fact]
    public void Should_Map_Settings_To_SettingsDto_With_Empty_Values()
    {
        // Arrange
        var settings = new Domain.Entities.Settings
        {
            Id = 2,
            Key = "EmptyKey",
            Value = "",
            Category = "",
            LastModified = DateTime.MinValue
        };

        // Act
        var result = _mapper.Map<SettingsDto>(settings);

        // Assert
        result.Should().NotBeNull();
        result.Key.Should().Be("EmptyKey");
        result.Value.Should().Be("");
        result.Category.Should().Be("");
        result.LastModified.Should().Be(DateTime.MinValue);
    }

    #endregion

    #region Contact Mappings

    [Fact]
    public void Should_Map_ContactSubmissionDto_To_ContactSubmission()
    {
        // Arrange
        var dto = new ContactSubmissionDto
        {
            Name = "John Doe",
            Email = "john.doe@example.com",
            Subject = "Inquiry about services",
            Message = "I would like to know more about your services."
        };

        // Act
        var result = _mapper.Map<ContactSubmission>(dto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("John Doe");
        result.Email.Should().Be("john.doe@example.com");
        result.Subject.Should().Be("Inquiry about services");
        result.Message.Should().Be("I would like to know more about your services.");
        result.IsRead.Should().BeFalse();
        result.Id.Should().Be(0);
        result.CreatedDate.Should().Be(default);
        result.ModifiedDate.Should().Be(default);
        result.SubmittedDate.Should().Be(default);
    }

    [Fact]
    public void Should_Map_ContactSubmissionDto_To_ContactSubmission_With_Empty_Subject()
    {
        // Arrange
        var dto = new ContactSubmissionDto
        {
            Name = "Jane Smith",
            Email = "jane@example.com",
            Subject = "",
            Message = "Quick question"
        };

        // Act
        var result = _mapper.Map<ContactSubmission>(dto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Jane Smith");
        result.Email.Should().Be("jane@example.com");
        result.Subject.Should().Be("");
        result.Message.Should().Be("Quick question");
        result.IsRead.Should().BeFalse();
    }

    [Fact]
    public void Should_Map_ContactSubmission_To_ContactSubmissionDto()
    {
        // Arrange
        var submittedDate = new DateTime(2024, 1, 20, 14, 30, 0, DateTimeKind.Utc);
        var contactSubmission = new ContactSubmission
        {
            Id = 1,
            Name = "Alice Johnson",
            Email = "alice@example.com",
            Subject = "Project collaboration",
            Message = "I have a project proposal.",
            IsRead = true,
            SubmittedDate = submittedDate,
            CreatedDate = submittedDate,
            ModifiedDate = submittedDate
        };

        // Act
        var result = _mapper.Map<Application.Contact.Queries.DTOs.ContactSubmissionDto>(contactSubmission);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("Alice Johnson");
        result.Email.Should().Be("alice@example.com");
        result.Subject.Should().Be("Project collaboration");
        result.Message.Should().Be("I have a project proposal.");
        result.IsRead.Should().BeTrue();
        result.SubmittedDate.Should().Be(submittedDate);
    }

    [Fact]
    public void Should_Map_ContactSubmission_To_ContactSubmissionDto_With_Null_Subject()
    {
        // Arrange
        var contactSubmission = new ContactSubmission
        {
            Id = 2,
            Name = "Bob Williams",
            Email = "bob@example.com",
            Subject = null,
            Message = "Hello!",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<Application.Contact.Queries.DTOs.ContactSubmissionDto>(contactSubmission);

        // Assert
        result.Should().NotBeNull();
        result.Subject.Should().BeNull();
        result.IsRead.Should().BeFalse();
    }

    [Fact]
    public void Should_Map_ContactSubmission_To_ContactSubmissionDto_With_Unread_Status()
    {
        // Arrange
        var contactSubmission = new ContactSubmission
        {
            Id = 3,
            Name = "Test User",
            Email = "test@example.com",
            Subject = "Test",
            Message = "Test message",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<Application.Contact.Queries.DTOs.ContactSubmissionDto>(contactSubmission);

        // Assert
        result.Should().NotBeNull();
        result.IsRead.Should().BeFalse();
    }

    #endregion

    #region Edge Cases and Complex Scenarios

    [Fact]
    public void Should_Handle_Project_With_Multiple_Thumbnails_By_Using_First()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Test Project",
            ShortDescription = "Test",
            Technologies = "[]",
            DisplayOrder = 1,
            Status = ProjectStatus.Published,
            Images = new List<ProjectImage>
            {
                new ProjectImage
                {
                    Id = 1,
                    ImagePath = "/images/thumb1.jpg",
                    AltText = "First Thumbnail",
                    IsThumbnail = true,
                    DisplayOrder = 1
                },
                new ProjectImage
                {
                    Id = 2,
                    ImagePath = "/images/thumb2.jpg",
                    AltText = "Second Thumbnail",
                    IsThumbnail = true,
                    DisplayOrder = 2
                }
            }
        };

        // Act
        var result = _mapper.Map<ProjectDto>(project);

        // Assert
        result.Should().NotBeNull();
        result.ThumbnailPath.Should().Be("/images/thumb1.jpg");
    }

    [Fact]
    public void Should_Map_Collection_Of_Projects_To_Collection_Of_ProjectDtos()
    {
        // Arrange
        var projects = new List<Project>
        {
            new Project
            {
                Id = 1,
                Title = "Project 1",
                ShortDescription = "Desc 1",
                Technologies = "[\"C#\"]",
                DisplayOrder = 1,
                Status = ProjectStatus.Published,
                Images = new List<ProjectImage>()
            },
            new Project
            {
                Id = 2,
                Title = "Project 2",
                ShortDescription = "Desc 2",
                Technologies = "[\"Java\"]",
                DisplayOrder = 2,
                Status = ProjectStatus.Draft,
                Images = new List<ProjectImage>()
            }
        };

        // Act
        var result = _mapper.Map<List<ProjectDto>>(projects);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Title.Should().Be("Project 1");
        result[1].Title.Should().Be("Project 2");
    }

    [Fact]
    public void Should_Map_Collection_Of_Employment_To_Collection_Of_EmploymentDtos()
    {
        // Arrange
        var employments = new List<Domain.Entities.Employment>
        {
            new Domain.Entities.Employment
            {
                Id = 1,
                CompanyName = "Company A",
                JobTitle = "Developer",
                StartDate = DateTime.UtcNow,
                Responsibilities = "[]",
                Achievements = "[]",
                Technologies = "[]",
                DisplayOrder = 1
            },
            new Domain.Entities.Employment
            {
                Id = 2,
                CompanyName = "Company B",
                JobTitle = "Senior Developer",
                StartDate = DateTime.UtcNow,
                Responsibilities = "[]",
                Achievements = "[]",
                Technologies = "[]",
                DisplayOrder = 2
            }
        };

        // Act
        var result = _mapper.Map<List<EmploymentDto>>(employments);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].CompanyName.Should().Be("Company A");
        result[1].CompanyName.Should().Be("Company B");
    }

    [Fact]
    public void Should_Map_Collection_Of_Skills_To_Collection_Of_SkillDtos()
    {
        // Arrange
        var skills = new List<Skill>
        {
            new Skill
            {
                Id = 1,
                Name = "Skill 1",
                Category = SkillCategory.Framework,
                DisplayOrder = 1
            },
            new Skill
            {
                Id = 2,
                Name = "Skill 2",
                Category = SkillCategory.Cloud,
                DisplayOrder = 2
            }
        };

        // Act
        var result = _mapper.Map<List<SkillDto>>(skills);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Name.Should().Be("Skill 1");
        result[1].Name.Should().Be("Skill 2");
    }

    [Fact]
    public void Should_Map_Empty_Collections()
    {
        // Arrange
        var emptyProjects = new List<Project>();
        var emptyEmployments = new List<Domain.Entities.Employment>();
        var emptySkills = new List<Skill>();

        // Act
        var projectResults = _mapper.Map<List<ProjectDto>>(emptyProjects);
        var employmentResults = _mapper.Map<List<EmploymentDto>>(emptyEmployments);
        var skillResults = _mapper.Map<List<SkillDto>>(emptySkills);

        // Assert
        projectResults.Should().NotBeNull().And.BeEmpty();
        employmentResults.Should().NotBeNull().And.BeEmpty();
        skillResults.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void Should_Handle_Whitespace_Json_Strings()
    {
        // Arrange
        var project = new Project
        {
            Id = 1,
            Title = "Test",
            ShortDescription = "Test",
            Technologies = "   ",
            DisplayOrder = 1,
            Status = ProjectStatus.Published,
            Images = new List<ProjectImage>()
        };

        // Act
        var result = _mapper.Map<ProjectDto>(project);

        // Assert
        result.Should().NotBeNull();
        result.Technologies.Should().NotBeNull();
        result.Technologies.Should().BeEmpty();
    }

    [Fact]
    public void Should_Preserve_Special_Characters_In_String_Properties()
    {
        // Arrange
        var contactSubmission = new ContactSubmission
        {
            Id = 1,
            Name = "Test <User>",
            Email = "test+alias@example.com",
            Subject = "Question about \"quotes\" & symbols",
            Message = "Message with special chars: !@#$%^&*()",
            IsRead = false,
            SubmittedDate = DateTime.UtcNow
        };

        // Act
        var result = _mapper.Map<Application.Contact.Queries.DTOs.ContactSubmissionDto>(contactSubmission);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test <User>");
        result.Email.Should().Be("test+alias@example.com");
        result.Subject.Should().Be("Question about \"quotes\" & symbols");
        result.Message.Should().Be("Message with special chars: !@#$%^&*()");
    }

    #endregion
}
