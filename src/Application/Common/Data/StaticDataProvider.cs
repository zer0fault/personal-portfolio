using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Data;

/// <summary>
/// Provides hardcoded data for the portfolio application
/// </summary>
public static class StaticDataProvider
{
    /// <summary>
    /// Hero section settings
    /// </summary>
    public static Dictionary<string, string> GetHeroSettings() => new()
    {
        ["Name"] = "Austin Little",
        ["Title"] = "Software Engineer",
        ["Tagline"] = "Building innovative solutions with .NET and modern web technologies",
        ["GitHubUrl"] = "https://github.com/zer0fault",
        ["LinkedInUrl"] = "https://www.linkedin.com/in/austin-little-200676169/"
    };

    /// <summary>
    /// About section settings
    /// </summary>
    public static Dictionary<string, string> GetAboutSettings() => new()
    {
        ["Bio"] = "Passionate software engineer with expertise in .NET development, cloud computing, and modern web technologies. I specialize in building scalable, maintainable applications using Clean Architecture principles and industry best practices.\n\nWith a strong foundation in both backend and frontend development, I enjoy solving complex problems and delivering high-quality software solutions that make a real impact.",
        ["Education"] = "Bachelor of Science in Computer Science, Southern New Hampshire University",
        ["Certifications"] = "Microsoft Certified: Azure Fundamentals"
    };

    /// <summary>
    /// Project information without database bloat
    /// </summary>
    public record ProjectInfo(
        string Title,
        string ShortDescription,
        string FullDescription,
        List<string> Technologies,
        string? GitHubUrl = null,
        string? LiveDemoUrl = null
    );

    public static List<ProjectInfo> GetProjectsData() => new()
    {
        new ProjectInfo(
            Title: "Personal Portfolio Website",
            ShortDescription: "Professional portfolio showcasing .NET expertise",
            FullDescription: "A modern, responsive portfolio website built with Blazor WebAssembly and Azure Functions, demonstrating Clean Architecture principles and cloud-native development practices.",
            Technologies: new() { "C#", "Blazor", "Azure Functions", "EF Core", "SQL Server" },
            GitHubUrl: "https://github.com/zer0fault/personal-portfolio"
        ),
        new ProjectInfo(
            Title: "Pomodoro TUI",
            ShortDescription: "Terminal-based productivity timer with customizable themes",
            FullDescription: "An aesthetic terminal application implementing the Pomodoro Technique with fully customizable work/break intervals. Features five built-in color schemes (Catppuccin Mocha, Nord, Gruvbox, Tokyo Night, Textual Dark), color-coded timer borders, keyboard-driven controls, and persistent TOML configuration. Built with Python's Textual framework for a responsive terminal interface.",
            Technologies: new() { "Python", "Textual", "TOML", "TUI" },
            GitHubUrl: "https://github.com/zer0fault/pomodoro-tui"
        )
    };

    /// <summary>
    /// Employment information without database bloat or JSON strings
    /// </summary>
    public record EmploymentInfo(
        string CompanyName,
        string JobTitle,
        DateTime StartDate,
        DateTime? EndDate,
        List<string> Responsibilities,
        List<string> Achievements,
        List<string> Technologies
    );

    public static List<EmploymentInfo> GetEmploymentData() => new()
    {
        new EmploymentInfo(
            CompanyName: "Duck Creek Technologies",
            JobTitle: "Sr. Associate Software Engineer",
            StartDate: DateTime.Parse("2022-09-01"),
            EndDate: null,
            Responsibilities: new()
            {
                "Designed, developed, and maintained web applications using .NET MVC/.NET Core frameworks and modern front-end technologies including HTML5, JavaScript, TypeScript, jQuery, and AngularJS, delivering responsive, user-focused interfaces",
                "Built and integrated RESTful APIs and Web APIs, enabling secure and scalable communication between services and enhancing application interoperability",
                "Utilized C# and ASP.NET to build robust, object-oriented solutions aligned with enterprise architecture and coding best practices",
                "Managed and optimized relational databases (SQL Server) through well-structured queries and effective relational database design, improving data performance and reliability",
                "Participated in CI/CD pipeline implementation and automation using PowerShell scripting, streamlining deployments and reducing system downtime",
                "Conducted thorough debugging, code reviews, and performance optimization efforts to improve application quality and maintainability",
                "Leveraged industry-standard software engineering practices across the SDLC including version control, testing, and build management, ensuring code stability and maintainability",
                "Demonstrated working knowledge of IIS concepts, secure hosting, and deployment strategies for enterprise applications"
            },
            Achievements: new(),
            Technologies: new() { "Git", "C#", "SQL", "PowerShell", "GitHub", "Cucumber", "SpecFlow", "MSSQL", "Debugging", "Unit Testing", "Acceptance Testing", "Agile", "Microsoft Azure", "NuGet", "ADO", ".NET Framework", "Nightwatch.js", "CI/CD" }
        ),
        new EmploymentInfo(
            CompanyName: "Duck Creek Technologies",
            JobTitle: "Software Configuration Specialist - Policy",
            StartDate: DateTime.Parse("2022-04-01"),
            EndDate: DateTime.Parse("2022-09-01"),
            Responsibilities: new()
            {
                "Collaborated cross-functionally with developers, designers, and architects to configure and customize software solutions aligned with business process designs and application requirements, ensuring high performance and scalability",
                "Provided critical pre-sales and post-sales support to Sales, Product Management, and Client Delivery teams by leveraging deep product knowledge to inform solution planning, enhance client presentations, and contribute to successful product implementations",
                "Participated in end-to-end software delivery, including code reviews, defect resolution, and performance tuning, resulting in smoother handoffs to QA/testing teams and improved application stability",
                "Engaged directly with internal teams and began independent customer interactions, demonstrating growing leadership and communication skills in client-facing environments",
                "Analyzed the functional impacts of configuration decisions to balance performance, user experience, and business needs, ultimately supporting more strategic, outcome-driven implementations"
            },
            Achievements: new()
            {
                "Played a key role in delivering tailored solutions that met client requirements, accelerating implementation timelines and contributing to increased customer satisfaction and retention",
                "Supported the sales pipeline by enabling more accurate scoping and solution alignment, helping to drive new business opportunities and product adoption"
            },
            Technologies: new() { "PowerShell", "Unit Testing", "XML", "Performance Testing", "Duck Creek Author" }
        ),
        new EmploymentInfo(
            CompanyName: "Freelance",
            JobTitle: "Freelance Software Engineer",
            StartDate: DateTime.Parse("2019-01-01"),
            EndDate: DateTime.Parse("2022-04-01"),
            Responsibilities: new()
            {
                "Developed secure Java-based web applications with robust test coverage and scalable architecture",
                "Built responsive Single Page Applications (SPAs) tailored to client needs, ensuring seamless user experiences",
                "Consulted with clients to evaluate technology stacks and recommend solutions aligned with business goals and technical requirements",
                "Managed end-to-end project lifecycles, from requirement gathering and scoping to delivery and support",
                "Delivered specialized solutions including OpenGL lighting renderers for CAD software"
            },
            Achievements: new(),
            Technologies: new() { "Java", "C++", "Product Requirement Definition", "OpenGL", "JavaScript" }
        )
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
