# Pre-Commit Verification Script
# Run this before committing to check for potential security issues

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Pre-Commit Security Verification" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

$errors = 0
$warnings = 0

# Check 1: Planning documents should NOT be in staged files
Write-Host "[1/6] Checking for planning documents..." -ForegroundColor Yellow
$staged = git diff --cached --name-only
$sensitiveFiles = @(
    "broad_requirements.txt",
    "REQUIREMENTS.md",
    "ZERO-COST-CHECKLIST.md",
    "IMPLEMENTATION-PLAN.md"
)

foreach ($file in $sensitiveFiles) {
    if ($staged -contains $file) {
        Write-Host "  ❌ ERROR: $file is staged for commit!" -ForegroundColor Red
        $errors++
    }
}

if ($errors -eq 0) {
    Write-Host "  ✅ No planning documents staged" -ForegroundColor Green
}
Write-Host ""

# Check 2: Configuration files with potential secrets
Write-Host "[2/6] Checking for configuration files..." -ForegroundColor Yellow
$configFiles = @(
    "local.settings.json",
    "appsettings.Development.json",
    "appsettings.Local.json",
    "connectionstrings.json",
    "secrets.json",
    ".env"
)

foreach ($file in $configFiles) {
    if ($staged -match $file) {
        Write-Host "  ❌ ERROR: $file is staged!" -ForegroundColor Red
        $errors++
    }
}

if ($errors -eq 0) {
    Write-Host "  ✅ No sensitive config files staged" -ForegroundColor Green
}
Write-Host ""

# Check 3: Database files
Write-Host "[3/6] Checking for database files..." -ForegroundColor Yellow
$dbExtensions = @("*.db", "*.sqlite", "*.mdf", "*.ldf")

foreach ($ext in $dbExtensions) {
    if ($staged -match $ext.Replace("*", "")) {
        Write-Host "  ❌ ERROR: Database file staged!" -ForegroundColor Red
        $errors++
    }
}

if ($errors -eq 0) {
    Write-Host "  ✅ No database files staged" -ForegroundColor Green
}
Write-Host ""

# Check 4: Search for hardcoded secrets in staged files
Write-Host "[4/6] Searching for potential hardcoded secrets..." -ForegroundColor Yellow
$patterns = @(
    "password\s*=",
    "secret\s*=",
    "api[_-]?key",
    "connection\s*string\s*=",
    "Server=.*Password=",
    "User Id=.*Password="
)

$foundSecrets = $false
foreach ($file in $staged) {
    if ($file -match "\.(cs|json|config)$") {
        foreach ($pattern in $patterns) {
            $matches = Select-String -Path $file -Pattern $pattern -CaseSensitive:$false -ErrorAction SilentlyContinue
            if ($matches) {
                Write-Host "  ⚠️  WARNING: Potential secret in $file" -ForegroundColor Yellow
                Write-Host "     Pattern: $pattern" -ForegroundColor Gray
                $warnings++
                $foundSecrets = $true
            }
        }
    }
}

if (-not $foundSecrets) {
    Write-Host "  ✅ No obvious secrets found" -ForegroundColor Green
}
Write-Host ""

# Check 5: Verify .gitignore is working
Write-Host "[5/6] Verifying .gitignore is working..." -ForegroundColor Yellow
$ignoredCheck = git check-ignore broad_requirements.txt REQUIREMENTS.md ZERO-COST-CHECKLIST.md IMPLEMENTATION-PLAN.md 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "  ✅ .gitignore is working correctly" -ForegroundColor Green
} else {
    Write-Host "  ❌ ERROR: .gitignore may not be working!" -ForegroundColor Red
    $errors++
}
Write-Host ""

# Check 6: Build artifacts check
Write-Host "[6/6] Checking for build artifacts..." -ForegroundColor Yellow
$artifacts = @("bin/", "obj/", ".vs/")

foreach ($artifact in $artifacts) {
    if ($staged -match $artifact) {
        Write-Host "  ⚠️  WARNING: Build artifact staged: $artifact" -ForegroundColor Yellow
        $warnings++
    }
}

if ($warnings -eq 0) {
    Write-Host "  ✅ No build artifacts staged" -ForegroundColor Green
}
Write-Host ""

# Summary
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Verification Summary" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "Errors:   $errors" -ForegroundColor $(if ($errors -eq 0) { "Green" } else { "Red" })
Write-Host "Warnings: $warnings" -ForegroundColor $(if ($warnings -eq 0) { "Green" } else { "Yellow" })
Write-Host ""

if ($errors -gt 0) {
    Write-Host "❌ COMMIT BLOCKED - Fix errors before committing!" -ForegroundColor Red
    Write-Host ""
    Write-Host "Actions to take:" -ForegroundColor Yellow
    Write-Host "1. Review staged files: git status" -ForegroundColor White
    Write-Host "2. Unstage sensitive files: git reset HEAD <file>" -ForegroundColor White
    Write-Host "3. Add to .gitignore if needed" -ForegroundColor White
    exit 1
} elseif ($warnings -gt 0) {
    Write-Host "⚠️  WARNINGS DETECTED - Please review before committing" -ForegroundColor Yellow
    Write-Host ""
    $response = Read-Host "Do you want to proceed anyway? (yes/no)"
    if ($response -ne "yes") {
        Write-Host "Commit cancelled." -ForegroundColor Yellow
        exit 1
    }
}

Write-Host "✅ All checks passed! Safe to commit." -ForegroundColor Green
Write-Host ""
exit 0
