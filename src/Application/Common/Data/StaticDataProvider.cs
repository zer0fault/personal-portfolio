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
        ["Bio"] = "I am a Software Engineer with hands-on experience in .NET (C#), AI-driven engineering solutions, and enterprise application development within high-availability environments. I specialize in building scalable backend systems, RESTful APIs, and automation-driven platforms that improve developer productivity and system performance.\n\nMy work spans AI and Generative AI integration, including agent-based workflows, RAG systems, and LLM-powered development tools that streamline SDLC processes and enhance engineering efficiency. I enjoy solving complex technical problems, improving system reliability, and delivering secure, maintainable code aligned with modern engineering and compliance standards.\n\nI have strong experience supporting production systems, including Tier 3 defect triage and Severity 1 incident resolution under strict SLAs, where I focus on rapid diagnosis, stability, and root-cause resolution. I collaborate effectively with cross-functional teams including engineering, QA, and product to deliver high-quality features and continuous improvements.\n\nCore expertise includes .NET/C#, Python, JavaScript, Azure, RESTful API design, CI/CD pipelines, SQL Server, test automation, and AI-powered development workflows. I am passionate about leveraging emerging AI technologies to enhance software delivery and build more intelligent engineering systems.",
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
            Title: "Azure Document Intelligence",
            ShortDescription: "End-to-end RAG pipeline for natural language document querying with citations",
            FullDescription: "An end-to-end retrieval-augmented generation (RAG) pipeline that enables natural language querying of enterprise documents with answers grounded in source material and direct citations. Implements semantic retrieval using Azure OpenAI embeddings (text-embedding-3-small) and Azure AI Search as the vector store, with gpt-4.1-mini for answer generation. Features idempotent ingestion with overlap chunking, server-side credential isolation, and a citation system tracing every answer to exact document chunks. Comprehensive test coverage with 28 passing tests across TypeScript and Python.",
            Technologies: new() { "Azure OpenAI", "Azure AI Search", "Next.js", "TypeScript", "Python", "Tailwind CSS", "Vitest", "pytest" },
            GitHubUrl: "https://github.com/zer0fault/azure-docs-intelligence-showcase"
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
            JobTitle: "Senior Associate Software Engineer",
            StartDate: DateTime.Parse("2022-09-01"),
            EndDate: null,
            Responsibilities: new()
            {
                "Improve application performance by enhancing secure ASP.NET Core RESTful APIs that support critical financial operations, resulting in faster and more reliable system behavior for end users.",
                "Strengthen front-end reliability and security by migrating legacy JavaScript components to TypeScript, modernizing build processes, fixing critical vulnerabilities, and implementing automated testing practices with high coverage.",
                "Optimize backend performance by refactoring C# application logic, eliminating inefficient query patterns, and improving system responsiveness for enterprise-scale usage."
            },
            Achievements: new()
            {
                "Lowered customer-reported defects by 15% and reduced backlog volume by 10% through targeted audits and refactoring efforts focused on high-risk code areas, improving overall application stability and maintainability.",
                "Accelerated SDLC delivery by co-developing a Python-based AI orchestration framework that automated developer workflows through agent-driven execution pipelines and validation layers, improving code quality and consistency across engineering processes.",
                "Reduced developer environment setup time from days to hours by building a fully automated provisioning solution that streamlined software installation, repository configuration, local services, and credential setup.",
                "Cut QA automation script creation time from days to about an hour by developing an AI-powered NightwatchJS test generation system that produced standards-aligned, validated test scripts for consistent engineering use."
            },
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
            "C#", "Python", "JavaScript", "TypeScript"
        },
        [SkillCategory.AI] = new()
        {
            "Claude AI", "Anthropic API", "Model Context Protocol (MCP)", "AI Agent Orchestration",
            "AI Workflow Automation", "Retrieval-Augmented Generation (RAG)", "Prompt Engineering",
            "LLM-Powered Application Development"
        },
        [SkillCategory.Framework] = new()
        {
            "ASP.NET Core", "RESTful APIs", "Microservices", "Distributed Systems", "Object-Oriented Programming"
        },
        [SkillCategory.Cloud] = new()
        {
            "Microsoft Azure", "Azure Pipelines", "CI/CD", "PowerShell", "Infrastructure & Environment Automation"
        },
        [SkillCategory.Architecture] = new()
        {
            "Clean Architecture"
        },
        [SkillCategory.Practice] = new()
        {
            "MSSQL", "Data Modeling", "Query Performance Optimization",
            "Nightwatch.js", "SpecFlow", "Jest", "Unit & Integration Testing",
            "Snyk", "SonarQube", "Secure SDLC", "Static Code Analysis", "Vulnerability Remediation",
            "Git", "GitHub", "Agile", "ADO"
        }
    };

    public static List<ContactSubmission> GetContactSubmissions() => new();
}
