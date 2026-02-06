# Migration to Fully Static Architecture

**Date:** 2026-02-06
**Objective:** Eliminate all Azure costs by migrating to a fully static site hosted on GitHub Pages

## Summary

Successfully migrated from Azure-hosted backend to a **100% static Blazor WebAssembly site** with **zero runtime costs**.

### Previous Architecture
- **Frontend:** Blazor WASM on GitHub Pages
- **Backend:** Azure Functions (Consumption Plan)
- **Database:** Azure SQL Database (later migrated to static data)
- **Storage:** Azure Storage Account
- **Monitoring:** Application Insights
- **Cost:** ~$0.50-1.00/month and growing

### New Architecture
- **Frontend:** Blazor WASM on GitHub Pages
- **Backend:** None - static data embedded in app
- **Database:** None - data hardcoded in `StaticDataProvider`
- **Cost:** **$0/month** 🎉

## Changes Made

### 1. Created Static Service Implementations

Created new service implementations in `src/BlazorApp/Services/Static/`:
- `StaticProjectsService` - Returns projects from `StaticDataProvider`
- `StaticEmploymentService` - Returns employment history from `StaticDataProvider`
- `StaticSkillsService` - Returns skills from `StaticDataProvider`
- `StaticSettingsService` - Returns settings from `StaticDataProvider`
- `StaticContactService` - Logs contact submissions to console (no persistence)

All services use **AutoMapper** to convert entities to DTOs, maintaining the same data contracts as before.

### 2. Updated Blazor App Configuration

**Program.cs changes:**
- Removed Azure Functions API base URL configuration
- Registered AutoMapper with `MappingProfile`
- Switched service registrations from HTTP-based to static implementations
- Removed dependency on external API calls

### 3. Disabled Azure Deployment

- Renamed `.github/workflows/deploy-functions.yml` → `deploy-functions.yml.disabled`
- Kept GitHub Pages deployment workflow (`deploy-pages.yml`) unchanged

### 4. Deleted Azure Resources

Deleted entire `rg-portfolio-prod` resource group containing:
- Function App: `func-portfolio-api-prod`
- Storage Account: `stportfolio20260202`
- App Service Plan: `CentralUSPlan`
- Application Insights: `func-portfolio-api-prod`

### 5. Updated Documentation

Updated `CLAUDE.md` to reflect:
- Fully static architecture
- Removed Azure Functions and database references
- Simplified deployment instructions
- Updated tech stack

## Data Management

### Current Approach
All portfolio data is hardcoded in `src/Application/Common/Data/StaticDataProvider.cs`:
- **Projects:** 2 sample projects
- **Employment:** 2 employment records
- **Skills:** 7 skills across categories
- **Settings:** 8 settings (Hero section, About section)
- **Contact Submissions:** Empty (not persisted)

### Updating Content
To update portfolio content, modify `StaticDataProvider.cs`:

```csharp
public static List<Project> GetProjects() => new()
{
    new Project
    {
        Title = "New Project",
        ShortDescription = "...",
        // ... other properties
    }
};
```

Then rebuild and the changes will be deployed on next push to `master`.

## Contact Form Behavior

The contact form is **non-functional** in static mode:
- Submissions are logged to browser console only
- No data is persisted or sent anywhere
- User sees success message but nothing happens

### Future Options for Contact Form:
1. **Integrate third-party service:**
   - Formspree
   - EmailJS
   - Netlify Forms (if migrating hosts)
   - Google Forms redirect

2. **Remove contact form entirely**

3. **Replace with mailto: link or social media links**

## Testing

All 195 tests still pass:
- Domain.Tests: 39 tests ✅
- Application.Tests: 134 tests ✅
- Infrastructure.Tests: 13 tests ✅
- Functions.API.IntegrationTests: 9 tests ✅

## Cost Savings

| Service | Before | After | Savings |
|---------|--------|-------|---------|
| Azure SQL Database | $0 (already removed) | $0 | $0 |
| Azure Functions | ~$0.10-0.50/month | $0 | ~$0.10-0.50/month |
| Storage Account | ~$0.05-0.10/month | $0 | ~$0.05-0.10/month |
| Application Insights | ~$0.20-0.40/month | $0 | ~$0.20-0.40/month |
| Bandwidth | ~$0.05/month | $0 | ~$0.05/month |
| **Total** | **~$0.40-1.05/month** | **$0** | **~$0.40-1.05/month** |

## Deployment

### Local Development
```bash
cd src/BlazorApp
dotnet run
```

### Production Deployment
Automatic via GitHub Actions on push to `master`:
1. Builds Blazor app in Release mode
2. Updates base href for GitHub Pages
3. Deploys to `https://<username>.github.io/personal-portfolio/`

## Rollback Plan

If needed, the Azure infrastructure can be recreated:
1. Re-enable `deploy-functions.yml.disabled`
2. Provision Azure resources (Functions, Storage)
3. Update Blazor services back to HTTP-based implementations
4. Deploy Functions.API

However, with static data, **rollback should not be necessary**.

## Next Steps (Optional)

1. **Remove unused code:**
   - Delete `src/Functions.API/` folder
   - Delete `src/Infrastructure/` folder
   - Remove related test projects
   - Clean up solution file

2. **Contact form options:**
   - Integrate Formspree or EmailJS
   - Or remove contact form

3. **Optimize Blazor build:**
   - Enable AOT compilation
   - Further reduce bundle size

4. **Add CI/CD improvements:**
   - Run tests before deployment
   - Add lighthouse performance checks

## Conclusion

✅ **Successfully migrated to 100% static architecture**
✅ **Eliminated all Azure costs**
✅ **Maintained all functionality except contact form persistence**
✅ **All tests passing**
✅ **Deployment pipeline functional**

**Result:** A portfolio website with **zero monthly costs** while maintaining professional appearance and functionality.
