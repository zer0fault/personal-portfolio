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
        ),
        new ProjectInfo(
            Title: "eBird Heatmap",
            ShortDescription: "Interactive global bird observation heatmap powered by the eBird API",
            FullDescription: "An interactive mapping application that visualizes bird observation data worldwide. Supports three visualization modes — biodiversity (species density), individual species distribution, and notable/rare sightings — with real-time eBird API data. Includes global location search via OpenStreetMap geocoding, adjustable radius (10–50 km) and time window (1–30 days) filters, and 24-hour IndexedDB caching with manual refresh.",
            Technologies: new() { "TypeScript", "Next.js", "MapLibre GL JS", "Tailwind CSS", "eBird API" },
            GitHubUrl: "https://github.com/zer0fault/ebird-heatmap"
        ),
        new ProjectInfo(
            Title: "Terminal Pokédex",
            ShortDescription: "Feature-rich TUI for browsing Pokémon data in the terminal",
            FullDescription: "A terminal user interface for exploring Pokémon data via the PokeAPI. Features search and filtering by name, ID, generation, or type; color-coded stat bars; terminal-rendered sprite images; evolution chain visualization; complete ability descriptions; sortable move tables; and support for alternate forms (Mega, Alolan variants). Uses SQLite caching and LRU sprite caching for performance, with a comprehensive test suite covering parsers, validation, and cache logic.",
            Technologies: new() { "Python", "Textual", "PokeAPI", "SQLite", "Rich", "Pydantic" },
            GitHubUrl: "https://github.com/zer0fault/Terminal-Pokedex"
        ),
        new ProjectInfo(
            Title: "AI News Feed",
            ShortDescription: "Dark-themed RSS dashboard aggregating AI news from major sources",
            FullDescription: "A streamlined, dark-themed RSS dashboard that aggregates AI news from Anthropic, OpenAI, Google DeepMind, Hugging Face, Mistral, and major tech publications. Stories are presented in reverse chronological order with per-source filtering. Built as a static site with RSS fetched at build time, deployed on Cloudflare Pages.",
            Technologies: new() { "TypeScript", "Astro", "CSS", "Cloudflare Pages" },
            GitHubUrl: "https://github.com/zer0fault/ai-feed"
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
                "Co-developed an internal Python-based AI developer orchestration framework automating SDLC workflows for 55 Claims engineers via specialized AI agents with structured workflow execution and validation layers ensuring generated code accuracy",
                "Engineered fully automated developer environment provisioning reducing machine setup from 2–3 days to ~3.5 hours (~80% reduction) for 55 engineers; orchestrates software installation, repository configuration, local services, and credential management with minimal human input",
                "Built AI-powered NightwatchJS test script generator reducing QA script authoring from 2–3 days to ~1 hour (~95% reduction) for 55 engineers via template-driven generation validated against department QA and coding standards",
                "Co-led two internal AI tools at company hackathons: a GitHub PR summarizer (OpenAI GPT + webhooks, ~60% review time reduction, 15–20 PRs/week) and a GPT-4 RAG documentation assistant (~50% search time reduction); both adopted post-hackathon by engineering team",
                "Designed and implemented secure ASP.NET Core RESTful APIs for critical payments and financial functions serving 5,000+ daily users, achieving 20% performance improvement over legacy system",
                "Migrated claims application from JavaScript to TypeScript, re-engineered the build process for a 40% performance boost, patched critical jQuery security vulnerabilities, and introduced Jest unit tests achieving 90% code coverage",
                "Optimized C# codebase by resolving deferred LINQ execution and correcting inefficient data types, improving performance by ~30%",
                "Audited and refactored high-defect codebase areas, reducing customer defect inflow by 15% and cutting defect backlog by 10%"
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
            "C#", "JavaScript", "TypeScript", "PowerShell", "SQL"
        },
        [SkillCategory.AI] = new()
        {
            "Claude AI", "Anthropic API", "MCP (Model Context Protocol)", "AI Agent Orchestration", "AI Workflow Automation"
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
