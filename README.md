# Personal Portfolio - Austin Little

Professional portfolio website showcasing .NET expertise and software engineering skills.

## 🌐 Live Site

- **Portfolio:** [Coming Soon - GitHub Pages]
- **Admin Panel:** [Coming Soon - GitHub Pages/admin]

## 🚀 Tech Stack

### Frontend
- **Blazor WebAssembly** - .NET 9 SPA framework
- **C# 13** - Latest language features
- **CSS3** - Custom responsive design with dark/light mode

### Backend
- **Azure Functions** - Serverless HTTP triggers (.NET 9 Isolated)
- **Entity Framework Core** - ORM for database access
- **ASP.NET Core Identity** - Authentication & authorization

### Database
- **Azure SQL Database** - Free tier (32 GB, 100 DTUs)
- **SQLite** - Local development

### Infrastructure
- **GitHub Pages** - Static frontend hosting (free)
- **Azure Functions Consumption Plan** - API backend (free tier)
- **GitHub Actions** - CI/CD pipelines

## 💰 Cost

**Monthly Cost: $0** (100% free tier usage)

This portfolio is designed to operate entirely within free tier limits:
- GitHub Pages: Unlimited for public repos
- Azure Functions: 1M executions/month (expected usage <10K)
- Azure SQL Free: 32 GB storage, 100 DTUs

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
│   Free Tier (32 GB)            │
└────────────────────────────────┘
```

**Clean Architecture Layers:**
- **Domain** - Core entities and business rules
- **Application** - Use cases, DTOs, interfaces
- **Infrastructure** - Data access, EF Core, external services
- **Functions.API** - Azure Functions HTTP triggers
- **BlazorApp** - Blazor WebAssembly frontend

## ✨ Features

### Public Site
- **Responsive Design** - Mobile, tablet, desktop optimized
- **Dark/Light Mode** - Toggle with localStorage persistence
- **Hero Section** - Professional headshot and call-to-action
- **About Section** - Professional bio, education, certifications
- **Skills Section** - Categorized technical competencies
- **Projects Portfolio** - Dynamic grid with detail modals
- **Employment Timeline** - Professional experience visualization
- **Contact Form** - Validated submission with database storage

### Admin Panel
- **Secure Login** - JWT authentication
- **Project Management** - CRUD operations for portfolio projects
- **Employment Management** - Manage work history
- **Contact Submissions** - View and manage form submissions
- **Settings** - Update hero text, bio, social links

## 🛠️ Local Development Setup

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Azure Functions Core Tools v4](https://docs.microsoft.com/azure/azure-functions/functions-run-local)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### Clone Repository
```bash
git clone https://github.com/yourusername/personal-portfolio.git
cd personal-portfolio
```

### Restore Dependencies
```bash
dotnet restore
```

### Database Setup (Local)
```bash
cd src/Infrastructure
dotnet ef database update --startup-project ../Functions.API/
```

### Run Azure Functions API (Terminal 1)
```bash
cd src/Functions.API
func start
```
API will be available at `http://localhost:7071`

### Run Blazor App (Terminal 2)
```bash
cd src/BlazorApp
dotnet run
```
App will be available at `http://localhost:5000`

### Default Admin Credentials (Local Only)
- **Username:** `admin`
- **Password:** `Admin123!`

**⚠️ Change these in production!**

## 🧪 Testing

### Run All Tests
```bash
dotnet test
```

### Run with Coverage
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Integration Tests
```bash
dotnet test --filter "Category=Integration"
```

## 📦 Deployment

### Prerequisites
- Azure account (free tier)
- GitHub account

### Azure Resources
1. Create Azure SQL Free Database
2. Create Azure Function App (Consumption Plan)
3. Configure Function App settings (connection strings, JWT secret)

### GitHub Actions
Push to `main` branch triggers automatic deployment:
- **Build & Test** - Runs on every push/PR
- **Deploy Blazor** - Deploys to GitHub Pages
- **Deploy Functions** - Deploys to Azure Functions

## 🔒 Security

See [SECURITY.md](./SECURITY.md) for detailed security guidelines.

**Key Points:**
- No secrets in code (use Azure App Settings & GitHub Secrets)
- Planning documents are git-ignored
- HTTPS enforced
- JWT authentication for admin
- Passwords hashed with ASP.NET Core Identity

## 📝 Project Structure

```
Personal Portfolio/
├── .github/
│   └── workflows/           # CI/CD pipelines
├── src/
│   ├── Domain/              # Core entities
│   ├── Application/         # Business logic
│   ├── Infrastructure/      # Data access
│   ├── Functions.API/       # Azure Functions
│   └── BlazorApp/           # Blazor WebAssembly
├── tests/
│   ├── Domain.Tests/
│   ├── Application.Tests/
│   ├── Infrastructure.Tests/
│   └── Functions.API.Tests/
├── docs/
│   └── architecture/        # Architecture Decision Records
├── .gitignore
├── README.md
└── SECURITY.md
```

## 🎯 Development Principles

- **SOLID Principles** - Applied throughout all layers
- **Clean Architecture** - Clear separation of concerns
- **Test-Driven Development** - >80% code coverage goal
- **Zero-Cost Architecture** - All services within free tiers
- **Security-First** - No secrets in code, secure authentication

## 📊 Code Quality

- **Code Coverage:** Target >80%
- **Lighthouse Score:** Target >90
- **Accessibility:** WCAG 2.1 Level AA compliant
- **Browser Support:** Chrome, Firefox, Safari, Edge (latest 2 versions)

## 🤝 Contributing

This is a personal portfolio project. If you'd like to use it as a template:

1. Fork the repository
2. Update personal information (name, LinkedIn, etc.)
3. Replace images in `wwwroot/assets/`
4. Update `appsettings.json` with your API URLs
5. Deploy to your own GitHub Pages and Azure

## 📄 License

MIT License - See [LICENSE](./LICENSE) file for details

## 📞 Contact

- **LinkedIn:** [Austin Little](https://linkedin.com/in/austin-little-200676169/)
- **Email:** [Contact via portfolio site]
- **GitHub:** [Your GitHub]

## 🙏 Acknowledgments

- Built with [.NET 9](https://dotnet.microsoft.com/)
- Hosted on [GitHub Pages](https://pages.github.com/)
- Backend powered by [Azure Functions](https://azure.microsoft.com/services/functions/)
- Architecture inspired by [Jason Taylor's Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture)

---

**Status:** 🚧 In Development
**Last Updated:** January 28, 2026
**Version:** 1.0.0-alpha
