using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// Seeds initial data for development/testing
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Check if already seeded
        if (await context.Projects.AnyAsync())
        {
            return;
        }

        // Seed Projects
        var projects = new[]
        {
            new Project
            {
                Title = "Personal Portfolio Website",
                ShortDescription = "Professional portfolio showcasing .NET expertise",
                FullDescription = "A modern portfolio website built with Clean Architecture, Blazor WebAssembly, and Azure Functions. Features include project showcase, employment timeline, skills display, and contact form with database persistence.",
                Technologies = "[\"C#\",\"Blazor\",\"Azure Functions\",\"EF Core\",\"SQL Server\"]",
                GitHubUrl = "https://github.com/zer0fault/personal-portfolio",
                Status = ProjectStatus.Published,
                DisplayOrder = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new Project
            {
                Title = "E-Commerce Platform",
                ShortDescription = "Microservices-based e-commerce solution",
                FullDescription = "A scalable e-commerce platform using microservices architecture with Docker containers, RabbitMQ message bus, and Redis caching.",
                Technologies = "[\"ASP.NET Core\",\"Docker\",\"RabbitMQ\",\"Redis\",\"PostgreSQL\"]",
                Status = ProjectStatus.Published,
                DisplayOrder = 2,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            }
        };

        context.Projects.AddRange(projects);

        // Seed Employment
        var employment = new[]
        {
            new Employment
            {
                CompanyName = "Tech Solutions Inc",
                JobTitle = "Senior Software Engineer",
                StartDate = new DateTime(2022, 1, 15),
                Responsibilities = "[\"Architected and implemented microservices\",\"Led team of 5 developers\",\"Reduced deployment time by 60%\"]",
                Achievements = "[\"Promoted from Mid-Level in 1 year\",\"AWS Solutions Architect certification\"]",
                Technologies = "[\"C#\",\"ASP.NET Core\",\"Azure\",\"Docker\",\"Kubernetes\"]",
                DisplayOrder = 1,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new Employment
            {
                CompanyName = "Digital Innovations LLC",
                JobTitle = "Software Developer",
                StartDate = new DateTime(2020, 6, 1),
                EndDate = new DateTime(2021, 12, 31),
                Responsibilities = "[\"Developed REST APIs\",\"Implemented CI/CD pipelines\",\"Conducted code reviews\"]",
                Achievements = "[\"Implemented automated testing (95% coverage)\",\"Optimized database queries (40% faster)\"]",
                Technologies = "[\"C#\",\".NET 6\",\"SQL Server\",\"Git\",\"Azure DevOps\"]",
                DisplayOrder = 2,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            }
        };

        context.EmploymentHistory.AddRange(employment);

        // Seed Skills
        var skills = new[]
        {
            new Skill { Name = "C#", Category = SkillCategory.Language, ProficiencyLevel = ProficiencyLevel.Expert, DisplayOrder = 1, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new Skill { Name = "JavaScript", Category = SkillCategory.Language, ProficiencyLevel = ProficiencyLevel.Advanced, DisplayOrder = 2, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new Skill { Name = "ASP.NET Core", Category = SkillCategory.Framework, ProficiencyLevel = ProficiencyLevel.Expert, DisplayOrder = 1, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new Skill { Name = "Blazor", Category = SkillCategory.Framework, ProficiencyLevel = ProficiencyLevel.Advanced, DisplayOrder = 2, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new Skill { Name = "Azure", Category = SkillCategory.Cloud, ProficiencyLevel = ProficiencyLevel.Advanced, DisplayOrder = 1, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new Skill { Name = "Clean Architecture", Category = SkillCategory.Architecture, ProficiencyLevel = ProficiencyLevel.Expert, DisplayOrder = 1, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow },
            new Skill { Name = "Unit Testing", Category = SkillCategory.Practice, ProficiencyLevel = ProficiencyLevel.Expert, DisplayOrder = 1, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow }
        };

        context.Skills.AddRange(skills);

        // Seed Settings
        var settings = new[]
        {
            // Hero Section Settings
            new Settings
            {
                Key = "Name",
                Value = "Austin Little",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new Settings
            {
                Key = "Title",
                Value = "Software Engineer",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new Settings
            {
                Key = "Tagline",
                Value = "Building innovative solutions with .NET and modern web technologies",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new Settings
            {
                Key = "CTAText",
                Value = "Get In Touch",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new Settings
            {
                Key = "GitHubUrl",
                Value = "https://github.com/zer0fault",
                Category = "Hero",
                LastModified = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },

            // About Section Settings
            new Settings
            {
                Key = "Bio",
                Value = "Passionate software engineer with expertise in .NET development, cloud computing, and modern web technologies. I specialize in building scalable, maintainable applications using Clean Architecture principles and industry best practices.\n\nWith a strong foundation in both backend and frontend development, I enjoy solving complex problems and delivering high-quality software solutions that make a real impact.",
                Category = "About",
                LastModified = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new Settings
            {
                Key = "Education",
                Value = "Bachelor of Science in Computer Science; Additional coursework in Software Engineering and Cloud Architecture",
                Category = "About",
                LastModified = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new Settings
            {
                Key = "Certifications",
                Value = "Microsoft Certified: Azure Developer Associate; AWS Certified Solutions Architect",
                Category = "About",
                LastModified = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            }
        };

        context.Settings.AddRange(settings);

        await context.SaveChangesAsync();
    }
}
