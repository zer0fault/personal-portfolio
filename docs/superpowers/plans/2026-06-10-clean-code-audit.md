# Clean Code Audit Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Eliminate four concrete DRY/KISS violations found in the codebase audit without changing any observable behaviour.

**Architecture:** All changes are internal refactors — extract shared helpers, consolidate duplicated logic, remove dead domain property. The static data pipeline (StaticDataProvider → Static*Service → Blazor component) is unchanged; only implementation details move.

**Tech Stack:** .NET 9 / C# 13, Blazor WASM, xUnit + FluentAssertions

---

## File Map

| Action | File |
|--------|------|
| Modify | `src/Application/Common/Data/StaticDataProvider.cs` |
| Modify | `src/Application/Settings/Queries/GetAllSettings/GetAllSettingsQueryHandler.cs` |
| Modify | `src/Application/Settings/Queries/GetSettingById/GetSettingByIdQueryHandler.cs` |
| Modify | `src/Application/Settings/Queries/GetSettingsByCategory/GetSettingsByCategoryQueryHandler.cs` |
| Modify | `src/BlazorApp/Services/Static/StaticSettingsService.cs` |
| Create | `src/BlazorApp/Helpers/DateTimeHelper.cs` |
| Modify | `src/BlazorApp/Components/EmploymentSection.razor` |
| Modify | `src/BlazorApp/Pages/Admin/Employment/Index.razor` |
| Create | `src/BlazorApp/Helpers/SkillCategoryExtensions.cs` |
| Modify | `src/BlazorApp/Components/SkillsSection.razor` |
| Modify | `src/BlazorApp/Pages/Admin/Skills/Index.razor` |
| Modify | `src/BlazorApp/Pages/Admin/Skills/CreateEdit.razor` |
| Modify | `src/Domain/Entities/Skill.cs` |
| Delete | `src/Domain/Enums/ProficiencyLevel.cs` |
| Modify | `tests/Domain.Tests/Entities/SkillTests.cs` |
| Modify | `tests/Application.Tests/Common/Mappings/MappingProfileTests.cs` |

---

## Task 1: Extract Settings DTO Builder (DRY — 4 consumers)

`GetAllSettingsQueryHandler`, `GetSettingByIdQueryHandler`, `GetSettingsByCategoryQueryHandler`, and `StaticSettingsService` all independently build the same `List<SettingsDto>` from `StaticDataProvider.GetHeroSettings()` + `GetAboutSettings()`.  Fix: add `GetAllSettingsDtos()` to `StaticDataProvider` and replace all four duplicate bodies.

**Files:**
- Modify: `src/Application/Common/Data/StaticDataProvider.cs`
- Modify: `src/Application/Settings/Queries/GetAllSettings/GetAllSettingsQueryHandler.cs`
- Modify: `src/Application/Settings/Queries/GetSettingById/GetSettingByIdQueryHandler.cs`
- Modify: `src/Application/Settings/Queries/GetSettingsByCategory/GetSettingsByCategoryQueryHandler.cs`
- Modify: `src/BlazorApp/Services/Static/StaticSettingsService.cs`
- Test: `tests/Application.Tests/Settings/` (existing tests, no new tests needed — behaviour is identical)

- [ ] **Step 1: Add `GetAllSettingsDtos()` to `StaticDataProvider.cs`**

Add this `using` at the top of the file:
```csharp
using Application.Settings.Queries.DTOs;
```

Add this method to the `StaticDataProvider` class (after `GetAboutSettings()`):
```csharp
public static List<SettingsDto> GetAllSettingsDtos()
{
    var settings = new List<SettingsDto>();
    var currentId = 1;

    foreach (var (key, value) in GetHeroSettings())
        settings.Add(new SettingsDto { Id = currentId++, Key = key, Value = value, Category = "Hero", LastModified = DateTime.UtcNow });

    foreach (var (key, value) in GetAboutSettings())
        settings.Add(new SettingsDto { Id = currentId++, Key = key, Value = value, Category = "About", LastModified = DateTime.UtcNow });

    return settings;
}
```

- [ ] **Step 2: Simplify `GetAllSettingsQueryHandler.cs`**

Replace the entire `Handle` method body with:
```csharp
public async Task<List<SettingsDto>> Handle(GetAllSettingsQuery request, CancellationToken cancellationToken)
{
    return await Task.FromResult(StaticDataProvider.GetAllSettingsDtos());
}
```

- [ ] **Step 3: Simplify `GetSettingByIdQueryHandler.cs`**

Replace the entire `Handle` method body with:
```csharp
public async Task<SettingsDto> Handle(GetSettingByIdQuery request, CancellationToken cancellationToken)
{
    var setting = StaticDataProvider.GetAllSettingsDtos().FirstOrDefault(s => s.Id == request.Id);
    return await Task.FromResult(setting!);
}
```

- [ ] **Step 4: Simplify `GetSettingsByCategoryQueryHandler.cs`**

Replace the entire `Handle` method body (removing the if/else branches) with:
```csharp
public async Task<List<SettingsDto>> Handle(GetSettingsByCategoryQuery request, CancellationToken cancellationToken)
{
    var settings = StaticDataProvider.GetAllSettingsDtos()
        .Where(s => s.Category == request.Category)
        .ToList();
    return await Task.FromResult(settings);
}
```

Also remove the unused `using MediatR;` if the compiler warns (it's needed for `IRequestHandler` — keep it).

- [ ] **Step 5: Simplify `StaticSettingsService.cs`**

Replace `GetAllSettingsAsync`, `GetSettingsByCategoryAsync`, and `GetSettingByIdAsync` with:
```csharp
public Task<List<SettingsDto>> GetAllSettingsAsync()
{
    return Task.FromResult(StaticDataProvider.GetAllSettingsDtos());
}

public Task<List<SettingsDto>> GetSettingsByCategoryAsync(string category)
{
    var filtered = StaticDataProvider.GetAllSettingsDtos()
        .Where(s => s.Category == category)
        .ToList();
    return Task.FromResult(filtered);
}

public Task<SettingsDto?> GetSettingByIdAsync(int id)
{
    var setting = StaticDataProvider.GetAllSettingsDtos().FirstOrDefault(s => s.Id == id);
    return Task.FromResult(setting);
}
```

- [ ] **Step 6: Build and run tests**

```
dotnet build src/BlazorApp/BlazorApp.csproj -v q
dotnet test --no-restore --logger "console;verbosity=minimal"
```

Expected: `Build succeeded. 0 Warning(s). 0 Error(s)` and `Failed: 0, Passed: 173`.

- [ ] **Step 7: Commit**

```
git add src/Application/Common/Data/StaticDataProvider.cs
git add src/Application/Settings/Queries/GetAllSettings/GetAllSettingsQueryHandler.cs
git add src/Application/Settings/Queries/GetSettingById/GetSettingByIdQueryHandler.cs
git add src/Application/Settings/Queries/GetSettingsByCategory/GetSettingsByCategoryQueryHandler.cs
git add src/BlazorApp/Services/Static/StaticSettingsService.cs
git commit -m "refactor: extract settings DTO builder to StaticDataProvider (DRY)"
```

---

## Task 2: Extract `GetDuration` to `DateTimeHelper` (DRY — 2 components)

`EmploymentSection.razor` and `Admin/Employment/Index.razor` both implement the same date-range-to-string logic independently with slightly inconsistent output formats. Fix: extract to a single static helper, standardise on abbreviated output ("yr/yrs", "mo/mos") for all cases.

**Files:**
- Create: `src/BlazorApp/Helpers/DateTimeHelper.cs`
- Modify: `src/BlazorApp/Components/EmploymentSection.razor`
- Modify: `src/BlazorApp/Pages/Admin/Employment/Index.razor`

- [ ] **Step 1: Create `DateTimeHelper.cs`**

Create `src/BlazorApp/Helpers/DateTimeHelper.cs`:
```csharp
namespace BlazorApp.Helpers;

public static class DateTimeHelper
{
    public static string GetDuration(DateTime startDate, DateTime? endDate)
    {
        var end = endDate ?? DateTime.UtcNow;
        var duration = end - startDate;
        var years = duration.Days / 365;
        var months = (duration.Days % 365) / 30;

        if (years > 0 && months > 0)
            return $"{years} yr{(years != 1 ? "s" : "")} {months} mo{(months != 1 ? "s" : "")}";
        if (years > 0)
            return $"{years} yr{(years != 1 ? "s" : "")}";
        if (months > 0)
            return $"{months} mo{(months != 1 ? "s" : "")}";
        return "Less than 1 month";
    }
}
```

- [ ] **Step 2: Update `EmploymentSection.razor`**

In the `@code` block, replace the entire `GetDuration` method with a one-line delegate:
```csharp
private string GetDuration(DateTime startDate, DateTime? endDate) =>
    DateTimeHelper.GetDuration(startDate, endDate);
```

Add `@using BlazorApp.Helpers` at the top of the file (after the existing `@inject` line).

- [ ] **Step 3: Update `Admin/Employment/Index.razor`**

Same change — replace the `GetDuration` method with:
```csharp
private string GetDuration(DateTime startDate, DateTime? endDate) =>
    DateTimeHelper.GetDuration(startDate, endDate);
```

Add `@using BlazorApp.Helpers` near the top of the file with the other using directives.

- [ ] **Step 4: Build and run tests**

```
dotnet build src/BlazorApp/BlazorApp.csproj -v q
dotnet test --no-restore --logger "console;verbosity=minimal"
```

Expected: `Build succeeded. 0 Warning(s). 0 Error(s)` and `Failed: 0, Passed: 173`.

- [ ] **Step 5: Commit**

```
git add src/BlazorApp/Helpers/DateTimeHelper.cs
git add src/BlazorApp/Components/EmploymentSection.razor
git add "src/BlazorApp/Pages/Admin/Employment/Index.razor"
git commit -m "refactor: extract GetDuration to DateTimeHelper (DRY)"
```

---

## Task 3: Extract `GetCategoryName` to `SkillCategoryExtensions` (DRY — 3 components)

`SkillsSection.razor`, `Admin/Skills/Index.razor`, and `Admin/Skills/CreateEdit.razor` each have a private `GetCategoryName(SkillCategory)` switch expression. The admin versions also omit the `AI` case, producing inconsistent display names. Fix: single extension method with canonical display names.

**Files:**
- Create: `src/BlazorApp/Helpers/SkillCategoryExtensions.cs`
- Modify: `src/BlazorApp/Components/SkillsSection.razor`
- Modify: `src/BlazorApp/Pages/Admin/Skills/Index.razor`
- Modify: `src/BlazorApp/Pages/Admin/Skills/CreateEdit.razor`

- [ ] **Step 1: Create `SkillCategoryExtensions.cs`**

Create `src/BlazorApp/Helpers/SkillCategoryExtensions.cs`:
```csharp
using Domain.Enums;

namespace BlazorApp.Helpers;

public static class SkillCategoryExtensions
{
    public static string GetDisplayName(this SkillCategory category) => category switch
    {
        SkillCategory.Language => "Languages",
        SkillCategory.Framework => "Frameworks & Libraries",
        SkillCategory.Cloud => "Cloud & DevOps",
        SkillCategory.Architecture => "Architecture & Design",
        SkillCategory.Practice => "Practices & Tools",
        SkillCategory.Domain => "Domain Knowledge",
        SkillCategory.AI => "AI / LLM Integration",
        _ => category.ToString()
    };
}
```

- [ ] **Step 2: Update `SkillsSection.razor`**

Add `@using BlazorApp.Helpers` after the existing `@using Domain.Enums` directive.

Replace the `GetCategoryName` method in the `@code` block with:
```csharp
private string GetCategoryName(SkillCategory category) => category.GetDisplayName();
```

- [ ] **Step 3: Update `Admin/Skills/Index.razor`**

Add `@using BlazorApp.Helpers` with the other using directives.

Replace the `GetCategoryName` method with:
```csharp
private string GetCategoryName(SkillCategory category) => category.GetDisplayName();
```

- [ ] **Step 4: Update `Admin/Skills/CreateEdit.razor`**

Add `@using BlazorApp.Helpers` with the other using directives.

Replace the `GetCategoryName` method with:
```csharp
private string GetCategoryName(SkillCategory category) => category.GetDisplayName();
```

- [ ] **Step 5: Build and run tests**

```
dotnet build src/BlazorApp/BlazorApp.csproj -v q
dotnet test --no-restore --logger "console;verbosity=minimal"
```

Expected: `Build succeeded. 0 Warning(s). 0 Error(s)` and `Failed: 0, Passed: 173`.

- [ ] **Step 6: Commit**

```
git add src/BlazorApp/Helpers/SkillCategoryExtensions.cs
git add src/BlazorApp/Components/SkillsSection.razor
git add "src/BlazorApp/Pages/Admin/Skills/Index.razor"
git add "src/BlazorApp/Pages/Admin/Skills/CreateEdit.razor"
git commit -m "refactor: extract GetCategoryName to SkillCategoryExtensions (DRY)"
```

---

## Task 4: Remove `ProficiencyLevel` from `Skill` (YAGNI)

`Skill.ProficiencyLevel` exists in the Domain entity, tests, and a dedicated enum but is absent from `SkillDto`, never populated by any handler or `StaticDataProvider`, and never rendered in any UI component. It is dead weight.

**Files:**
- Modify: `src/Domain/Entities/Skill.cs`
- Delete: `src/Domain/Enums/ProficiencyLevel.cs`
- Modify: `tests/Domain.Tests/Entities/SkillTests.cs`
- Modify: `tests/Application.Tests/Common/Mappings/MappingProfileTests.cs`

- [ ] **Step 1: Remove `ProficiencyLevel` property from `Skill.cs`**

In `src/Domain/Entities/Skill.cs`, remove these lines:
```csharp
    /// <summary>
    /// Proficiency level for this skill
    /// </summary>
    public ProficiencyLevel ProficiencyLevel { get; set; }
```

Also remove the `using Domain.Enums;` line only if `ProficiencyLevel` was the sole enum used from that namespace — `SkillCategory` also uses it, so keep the using.

- [ ] **Step 2: Delete `ProficiencyLevel.cs`**

```powershell
Remove-Item "src/Domain/Enums/ProficiencyLevel.cs"
```

- [ ] **Step 3: Update `SkillTests.cs`** — remove the two ProficiencyLevel tests and assertion

In `tests/Domain.Tests/Entities/SkillTests.cs`:

Remove the `Skill_ShouldSupportAllProficiencyLevels` theory test entirely (lines 72–87).

In `Skill_ShouldInitialize_WithDefaultValues`, remove this assertion:
```csharp
skill.ProficiencyLevel.Should().Be(ProficiencyLevel.Beginner);
```

In `Skill_ShouldAllowSettingProperties`, remove this line from the Act block:
```csharp
skill.ProficiencyLevel = ProficiencyLevel.Advanced;
```
And remove this assertion from the Assert block:
```csharp
skill.ProficiencyLevel.Should().Be(ProficiencyLevel.Advanced);
```

Remove the `using Domain.Enums;` line only if `ProficiencyLevel` was the only enum used — `SkillCategory` is also used there, so keep it.

- [ ] **Step 4: Update `MappingProfileTests.cs`** — remove `ProficiencyLevel` from Skill object initialisers

In `tests/Application.Tests/Common/Mappings/MappingProfileTests.cs`, remove `ProficiencyLevel = ProficiencyLevel.Expert` from the Skill initialiser in `Should_Map_Skill_To_SkillDto` (line ~486).

Remove `ProficiencyLevel = ProficiencyLevel.Advanced` from the Skill initialiser in `Should_Map_Skill_To_SkillDto_With_Null_IconUrl` (line ~514).

Remove `ProficiencyLevel = ProficiencyLevel.Intermediate` from the Skill initialiser in `Should_Map_Skill_To_SkillDto_For_All_Categories` (line ~543).

- [ ] **Step 5: Build and run tests**

```
dotnet build src/BlazorApp/BlazorApp.csproj -v q
dotnet test --no-restore --logger "console;verbosity=minimal"
```

Expected: `Build succeeded. 0 Warning(s). 0 Error(s)` and `Failed: 0, Passed: 169` (4 fewer — the 3 `ProficiencyLevel` test variants + 1 assertion removed from the default-values test).

- [ ] **Step 6: Commit**

```
git add src/Domain/Entities/Skill.cs
git rm src/Domain/Enums/ProficiencyLevel.cs
git add tests/Domain.Tests/Entities/SkillTests.cs
git add tests/Application.Tests/Common/Mappings/MappingProfileTests.cs
git commit -m "refactor: remove unused ProficiencyLevel from Skill entity (YAGNI)"
```

---

## Final Verification

- [ ] **Run full test suite one more time**

```
dotnet test --no-restore --logger "console;verbosity=minimal"
```

Expected: `Failed: 0, Passed: 169`.

- [ ] **Run Playwright smoke test** — navigate to `http://localhost:5218`, click through About / Skills / Projects / Experience and confirm all sections render correctly with no regressions.
