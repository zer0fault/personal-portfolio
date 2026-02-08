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
        new Domain.Entities.Settings { Id = 5, Key = "LinkedInUrl", Value = "https://www.linkedin.com/in/austin-little-200676169/", Category = "Hero", LastModified = DateTime.Parse("2026-02-06T11:30:00.0000000") },
        new Domain.Entities.Settings { Id = 6, Key = "Bio", Value = "Passionate software engineer with expertise in .NET development, cloud computing, and modern web technologies. I specialize in building scalable, maintainable applications using Clean Architecture principles and industry best practices.\n\nWith a strong foundation in both backend and frontend development, I enjoy solving complex problems and delivering high-quality software solutions that make a real impact.", Category = "About", LastModified = DateTime.Parse("2026-02-02T21:03:53.8656918") },
        new Domain.Entities.Settings { Id = 7, Key = "Education", Value = "Bachelor of Science in Computer Science, Southern New Hampshire University", Category = "About", LastModified = DateTime.Parse("2026-02-08T00:00:00.0000000") },
        new Domain.Entities.Settings { Id = 8, Key = "Certifications", Value = "Microsoft Certified: Azure Fundamentals", Category = "About", LastModified = DateTime.Parse("2026-02-08T00:00:00.0000000") }
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
            Title = "Pomodoro TUI",
            ShortDescription = "Terminal-based productivity timer with customizable themes",
            FullDescription = "An aesthetic terminal application implementing the Pomodoro Technique with fully customizable work/break intervals. Features five built-in color schemes (Catppuccin Mocha, Nord, Gruvbox, Tokyo Night, Textual Dark), color-coded timer borders, keyboard-driven controls, and persistent TOML configuration. Built with Python's Textual framework for a responsive terminal interface.",
            Technologies = "[\"Python\",\"Textual\",\"TOML\",\"TUI\"]",
            GitHubUrl = "https://github.com/zer0fault/pomodoro-tui",
            LiveDemoUrl = null,
            Status = ProjectStatus.Published,
            DisplayOrder = 2,
            CreatedDate = DateTime.Parse("2026-02-08T00:00:00.0000000"),
            ModifiedDate = DateTime.Parse("2026-02-08T00:00:00.0000000")
        }
    };

    public static List<Domain.Entities.Employment> GetEmployment() => new()
    {
        new Domain.Entities.Employment
        {
            Id = 1,
            CompanyName = "Duck Creek Technologies",
            JobTitle = "Sr. Associate Software Engineer",
            StartDate = DateTime.Parse("2022-09-01"),
            EndDate = null,
            Responsibilities = "[\"Designed, developed, and maintained web applications using .NET MVC/.NET Core frameworks and modern front-end technologies including HTML5, JavaScript, TypeScript, jQuery, and AngularJS, delivering responsive, user-focused interfaces\",\"Built and integrated RESTful APIs and Web APIs, enabling secure and scalable communication between services and enhancing application interoperability\",\"Utilized C# and ASP.NET to build robust, object-oriented solutions aligned with enterprise architecture and coding best practices\",\"Managed and optimized relational databases (SQL Server) through well-structured queries and effective relational database design, improving data performance and reliability\",\"Participated in CI/CD pipeline implementation and automation using PowerShell scripting, streamlining deployments and reducing system downtime\",\"Conducted thorough debugging, code reviews, and performance optimization efforts to improve application quality and maintainability\",\"Leveraged industry-standard software engineering practices across the SDLC including version control, testing, and build management, ensuring code stability and maintainability\",\"Demonstrated working knowledge of IIS concepts, secure hosting, and deployment strategies for enterprise applications\"]",
            Achievements = "[]",
            Technologies = "[\"Git\",\"C#\",\"SQL\",\"PowerShell\",\"GitHub\",\"Cucumber\",\"SpecFlow\",\"MSSQL\",\"Debugging\",\"Unit Testing\",\"Acceptance Testing\",\"Agile\",\"Microsoft Azure\",\"NuGet\",\"ADO\",\".NET Framework\",\"Nightwatch.js\",\"CI/CD\"]",
            DisplayOrder = 1,
            CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"),
            ModifiedDate = DateTime.Parse("2026-02-08T00:00:00.0000000")
        },
        new Domain.Entities.Employment
        {
            Id = 2,
            CompanyName = "Duck Creek Technologies",
            JobTitle = "Software Configuration Specialist - Policy",
            StartDate = DateTime.Parse("2022-04-01"),
            EndDate = DateTime.Parse("2022-09-01"),
            Responsibilities = "[\"Collaborated cross-functionally with developers, designers, and architects to configure and customize software solutions aligned with business process designs and application requirements, ensuring high performance and scalability\",\"Provided critical pre-sales and post-sales support to Sales, Product Management, and Client Delivery teams by leveraging deep product knowledge to inform solution planning, enhance client presentations, and contribute to successful product implementations\",\"Participated in end-to-end software delivery, including code reviews, defect resolution, and performance tuning, resulting in smoother handoffs to QA/testing teams and improved application stability\",\"Engaged directly with internal teams and began independent customer interactions, demonstrating growing leadership and communication skills in client-facing environments\",\"Analyzed the functional impacts of configuration decisions to balance performance, user experience, and business needs, ultimately supporting more strategic, outcome-driven implementations\"]",
            Achievements = "[\"Played a key role in delivering tailored solutions that met client requirements, accelerating implementation timelines and contributing to increased customer satisfaction and retention\",\"Supported the sales pipeline by enabling more accurate scoping and solution alignment, helping to drive new business opportunities and product adoption\"]",
            Technologies = "[\"PowerShell\",\"Unit Testing\",\"XML\",\"Performance Testing\",\"Duck Creek Author\"]",
            DisplayOrder = 2,
            CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"),
            ModifiedDate = DateTime.Parse("2026-02-08T00:00:00.0000000")
        },
        new Domain.Entities.Employment
        {
            Id = 3,
            CompanyName = "Freelance",
            JobTitle = "Freelance Software Engineer",
            StartDate = DateTime.Parse("2019-01-01"),
            EndDate = DateTime.Parse("2022-04-01"),
            Responsibilities = "[\"Developed secure Java-based web applications with robust test coverage and scalable architecture\",\"Built responsive Single Page Applications (SPAs) tailored to client needs, ensuring seamless user experiences\",\"Consulted with clients to evaluate technology stacks and recommend solutions aligned with business goals and technical requirements\",\"Managed end-to-end project lifecycles, from requirement gathering and scoping to delivery and support\",\"Delivered specialized solutions including OpenGL lighting renderers for CAD software\"]",
            Achievements = "[]",
            Technologies = "[\"Java\",\"C++\",\"Product Requirement Definition\",\"OpenGL\",\"JavaScript\"]",
            DisplayOrder = 3,
            CreatedDate = DateTime.Parse("2026-02-02T21:03:53.8656786"),
            ModifiedDate = DateTime.Parse("2026-02-08T00:00:00.0000000")
        }
    };

    /// <summary>
    /// Returns skills grouped by category. Order in list determines display order.
    /// </summary>
    public static Dictionary<SkillCategory, List<string>> GetSkillsByCategory() => new()
    {
        [SkillCategory.Language] = new()
        {
            "C#", "JavaScript", "TypeScript", "Java", "C++", "PowerShell", "SQL"
        },
        [SkillCategory.Framework] = new()
        {
            ".NET Framework", ".NET Core", "ASP.NET", "Blazor", "jQuery"
        },
        [SkillCategory.Cloud] = new()
        {
            "Microsoft Azure"
        },
        [SkillCategory.Architecture] = new()
        {
            "Clean Architecture"
        },
        [SkillCategory.Practice] = new()
        {
            "Git", "GitHub", "Unit Testing", "Acceptance Testing", "Cucumber",
            "SpecFlow", "Nightwatch.js", "Performance Testing", "CI/CD", "Agile",
            "Debugging", "ADO", "NuGet", "MSSQL", "XML", "Duck Creek Author", "OpenGL"
        }
    };

    public static List<ContactSubmission> GetContactSubmissions() => new();
}
