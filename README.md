# Personal Portfolio - Austin Little

Professional portfolio website showcasing .NET expertise and software engineering skills.

## 🌐 Live Site

- **Portfolio:** https://zer0fault.github.io/personal-portfolio/

## 🚀 Tech Stack

### Frontend
- **Blazor WebAssembly** - .NET 9 SPA framework
- **C# 13** - Latest language features
- **CSS3** - Custom responsive design with dark/light mode

### Backend
- **Azure Functions** - Serverless HTTP triggers (.NET 9 Isolated)
- **Entity Framework Core 9** - ORM with Code First migrations
- **ASP.NET Core Identity** - Authentication & authorization
- **MediatR** - CQRS pattern implementation

### Database
- **Azure SQL Database** - Cloud-hosted relational database
- **SQLite** - Local development environment

### Infrastructure
- **GitHub Pages** - Static frontend hosting
- **Azure Functions Consumption Plan** - Serverless API backend
- **GitHub Actions** - CI/CD automation

## 🏗️ Architecture

```
┌────────────────────────────────┐
│   GitHub Pages (Frontend)      │
│   Blazor WebAssembly           │
└───────────┬────────────────────┘
            │ HTTPS API calls
            ▼
┌────────────────────────────────┐
│   Azure Functions (Backend)    │
│   .NET 9 HTTP Triggers         │
└───────────┬────────────────────┘
            │ Entity Framework
            ▼
┌────────────────────────────────┐
│   Azure SQL Database           │
│   Relational Data Store        │
└────────────────────────────────┘
```

**Clean Architecture Layers:**
- **Domain** - Core entities, enums, and business rules
- **Application** - Use cases, DTOs, interfaces, CQRS handlers
- **Infrastructure** - Data access, EF Core, external services
- **Functions.API** - Azure Functions HTTP triggers
- **BlazorApp** - Blazor WebAssembly frontend

## ✨ Features

### Public Site
- **Responsive Design** - Mobile, tablet, desktop optimized
- **Dark/Light Mode** - Theme toggle with localStorage persistence
- **Hero Section** - Professional headshot and call-to-action
- **About Section** - Professional bio, education, certifications
- **Skills Section** - Categorized technical competencies with proficiency levels
- **Projects Portfolio** - Dynamic project grid with detail modals
- **Employment Timeline** - Professional experience visualization
- **Contact Form** - Validated form submission with database persistence

### Admin Panel
- **Secure Authentication** - JWT-based authentication
- **Project Management** - Full CRUD operations for portfolio projects
- **Employment Management** - Manage work history and achievements
- **Contact Submissions** - View and respond to contact form submissions
- **Settings Management** - Update hero text, bio, and social links

## 🔒 Security

**Key Points:**
- No secrets in source code (Azure App Settings & GitHub Secrets)
- HTTPS enforced for all communications
- JWT authentication for admin panel
- Passwords hashed with ASP.NET Core Identity (PBKDF2)
- Input validation with FluentValidation
- SQL injection prevention via parameterized queries

## 📝 Project Structure

```
Personal Portfolio/
├── scripts/
│   └── verify-commit.sh     # Pre-commit security checks
├── src/
│   ├── Domain/              # Entities, enums, interfaces
│   ├── Application/         # CQRS handlers, DTOs, validators
│   ├── Infrastructure/      # EF Core, repositories
│   ├── Functions.API/       # Azure Functions endpoints
│   └── BlazorApp/           # Blazor WebAssembly SPA
├── tests/
│   ├── Domain.Tests/
│   ├── Application.Tests/
│   ├── Infrastructure.Tests/
│   └── Functions.API.Tests/
├── CLAUDE.md
├── .gitignore
├── README.md
└── PortfolioWebsite.sln
```

## 🎯 Development Principles

- **SOLID Principles** - Applied throughout all layers
- **Clean Architecture** - Clear separation of concerns with dependency inversion
- **CQRS Pattern** - Command/Query separation via MediatR
- **Repository Pattern** - Abstraction over data access
- **Test-Driven Development** - Comprehensive unit and integration tests
- **Security-First** - Defense in depth, secure by default

## 📊 Code Quality

- **Code Coverage:** Target >80%
- **Lighthouse Score:** Target >90
- **Accessibility:** WCAG 2.1 Level AA compliant
- **Browser Support:** Chrome, Firefox, Safari, Edge (latest 2 versions)

## 📄 License

MIT License - See [LICENSE](./LICENSE) file for details

## 📞 Contact

- **LinkedIn:** [Austin Little](https://linkedin.com/in/austin-little-200676169/)
- **Email:** [Contact via portfolio site]
- **GitHub:** [zer0fault](https://github.com/zer0fault)

## 🙏 Acknowledgments

- Built with [.NET 9](https://dotnet.microsoft.com/)
- Hosted on [GitHub Pages](https://pages.github.com/)
- Backend powered by [Azure Functions](https://azure.microsoft.com/services/functions/)
- Architecture inspired by [Jason Taylor's Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture)

---

**Status:** 🚧 In Development
**Last Updated:** January 28, 2026
**Version:** 1.0.0-alpha
