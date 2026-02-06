using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Data;

/// <summary>
/// Provides hardcoded data for the portfolio application
/// </summary>
public static class StaticDataProvider
{
    public static List<Domain.Entities.Settings> GetSettings() => new()
    {
        new Domain.Entities.Settings { Id = 1, Key = "Name", Value = "Austin Little", Category = "Hero", LastModified = DateTime.Parse("2026-02-02T21:03:53.8656786") },
        new Domain.Entities.Settings { Id = 2, Key = "Title", Value = "Software Engineer", Category = "Hero", LastModified = DateTime.Parse("2026-02-02T21:03:53.8656912") },
        new Domain.Entities.Settings { Id = 3, Key = "Tagline", Value = "Building innovative solutions with .NET and modern web technologies", Category = "Hero", LastModified = DateTime.Parse("2026-02-02T21:03:53.8656914") },
        new Domain.Entities.Settings { Id = 4, Key = "GitHubUrl", Value = "https://github.com/zer0fault", Category = "Hero", LastModified = DateTime.Parse("2026-02-02T21:03:53.8656917") },
        new Domain.Entities.Settings { Id = 5, Key = "LinkedInUrl", Value = "https://www.linkedin.com/in/austin-little", Category = "Hero", LastModified = DateTime.Parse("2026-02-06T11:30:00.0000000") },
        new Domain.Entities.Settings { Id = 6, Key = "Bio", Value = "Passionate software engineer with expertise in .NET development, cloud computing, and modern web technologies. I specialize in building scalable, maintainable applications using Clean Architecture principles and industry best practices.\n\nWith a strong foundation in both backend and frontend development, I enjoy solving complex problems and delivering high-quality software solutions that make a real impact.", Category = "About", LastModified = DateTime.Parse("2026-02-02T21:03:53.8656918") },
        new Domain.Entities.Settings { Id = 7, Key = "Education", Value = "Bachelor of Science in Computer Science; Additional coursework in Software Engineering and Cloud Architecture", Category = "About", LastModified = DateTime.Parse("2026-02-02T21:03:53.865692") },
        new Domain.Entities.Settings { Id = 8, Key = "Certifications", Value = "Microsoft Certified: Azure Developer Associate; AWS Certified Solutions Architect", Category = "About", LastModified = DateTime.Parse("2026-02-02T21:03:53.8656921") }
    };

    public static List<Project> GetProjects() => new()
    {
        new Project
        {
            Id = 1,
            Title = "Personal Portfolio Website",
            ShortDescription = "Professional portfolio showcasing .NET expertise",
            FullDescription = "A modern, responsive portfolio website built with Blazor WebAssembly and Azure Functions, demonstrating Clean Architecture principles and cloud-native development practices.",
            Technologies = "[\"C#\",\"Blazor\",\"Azure Functions\",\"EF Core\",\"SQL Server\"]",
            GitHubUrl = "https://github.com/zer0fault/personal-portfolio",
            LiveDemoUrl = null,
            Status = ProjectStatus.Published,
            DisplayOrder = 1,
            CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"),
            ModifiedDate = DateTime.Parse("2026-02-02T21:03:53.8656786")
        },
        new Project
        {
            Id = 2,
            Title = "E-Commerce Platform",
            ShortDescription = "Microservices-based e-commerce solution",
            FullDescription = "Scalable e-commerce platform built with microservices architecture, featuring distributed transactions, event-driven communication, and containerized deployments.",
            Technologies = "[\"ASP.NET Core\",\"Docker\",\"RabbitMQ\",\"Redis\",\"PostgreSQL\"]",
            GitHubUrl = null,
            LiveDemoUrl = null,
            Status = ProjectStatus.Published,
            DisplayOrder = 2,
            CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"),
            ModifiedDate = DateTime.Parse("2026-02-02T21:03:53.8656786")
        }
    };

    public static List<Domain.Entities.Employment> GetEmployment() => new()
    {
        new Domain.Entities.Employment
        {
            Id = 1,
            CompanyName = "Tech Solutions Inc",
            JobTitle = "Senior Software Engineer",
            StartDate = DateTime.Parse("2022-01-15"),
            EndDate = null,
            Responsibilities = "[\"Architected and implemented microservices\",\"Led team of 5 developers\",\"Reduced deployment time by 60%\"]",
            Achievements = "[\"Promoted from Mid-Level in 1 year\",\"AWS Solutions Architect certification\"]",
            Technologies = "[\"C#\",\"ASP.NET Core\",\"Azure\",\"Docker\",\"Kubernetes\"]",
            DisplayOrder = 1,
            CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"),
            ModifiedDate = DateTime.Parse("2026-02-02T21:03:53.8656786")
        },
        new Domain.Entities.Employment
        {
            Id = 2,
            CompanyName = "Digital Innovations LLC",
            JobTitle = "Software Developer",
            StartDate = DateTime.Parse("2020-06-01"),
            EndDate = DateTime.Parse("2021-12-31"),
            Responsibilities = "[\"Developed REST APIs\",\"Implemented CI/CD pipelines\",\"Conducted code reviews\"]",
            Achievements = "[\"Implemented automated testing (95% coverage)\",\"Optimized database queries (40% faster)\"]",
            Technologies = "[\"C#\",\".NET 6\",\"SQL Server\",\"Git\",\"Azure DevOps\"]",
            DisplayOrder = 2,
            CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"),
            ModifiedDate = DateTime.Parse("2026-02-02T21:03:53.8656786")
        }
    };

    public static List<Skill> GetSkills() => new()
    {
        new Skill { Id = 1, Name = "C#", Category = SkillCategory.Language, DisplayOrder = 1, IconUrl = null, CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"), ModifiedDate = DateTime.Parse("2026-02-02T21:03:53.8656786") },
        new Skill { Id = 2, Name = "JavaScript", Category = SkillCategory.Language, DisplayOrder = 2, IconUrl = null, CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"), ModifiedDate = DateTime.Parse("2026-02-02T21:03:53.8656786") },
        new Skill { Id = 3, Name = "ASP.NET Core", Category = SkillCategory.Framework, DisplayOrder = 1, IconUrl = null, CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"), ModifiedDate = DateTime.Parse("2026-02-02T21:03:53.8656786") },
        new Skill { Id = 4, Name = "Blazor", Category = SkillCategory.Framework, DisplayOrder = 2, IconUrl = null, CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"), ModifiedDate = DateTime.Parse("2026-02-02T21:03:53.8656786") },
        new Skill { Id = 5, Name = "Azure", Category = SkillCategory.Cloud, DisplayOrder = 1, IconUrl = null, CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"), ModifiedDate = DateTime.Parse("2026-02-02T21:03:53.8656786") },
        new Skill { Id = 6, Name = "Clean Architecture", Category = SkillCategory.Architecture, DisplayOrder = 1, IconUrl = null, CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"), ModifiedDate = DateTime.Parse("2026-02-02T21:03:53.8656786") },
        new Skill { Id = 7, Name = "Unit Testing", Category = SkillCategory.Practice, DisplayOrder = 1, IconUrl = null, CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"), ModifiedDate = DateTime.Parse("2026-02-02T21:03:53.8656786") }
    };

    public static List<ContactSubmission> GetContactSubmissions() => new();
}
