# Security Guidelines - Personal Portfolio

## 🔒 Critical: Files That Must NEVER Be Committed

This document outlines security practices to prevent accidental leaking of sensitive information.

---

## Protected Planning Documents

The following planning documents contain sensitive information and are **automatically ignored by .gitignore**:

### ❌ NEVER COMMIT THESE FILES:
- `broad_requirements.txt` - Contains personal info and LinkedIn profile URL
- `REQUIREMENTS.md` - Contains detailed architecture and planning information
- `ZERO-COST-CHECKLIST.md` - Contains Azure setup details
- `IMPLEMENTATION-PLAN.md` - Contains comprehensive implementation details
- `/docs/planning/` - Any planning documents directory

**Reason:** These documents may contain:
- Personal contact information
- LinkedIn profile URLs
- Azure resource naming conventions
- Internal planning notes
- Development strategies

---

## Sensitive Configuration Files

### ❌ NEVER COMMIT:

#### 1. Azure Functions Local Settings
```
local.settings.json
```
**Contains:**
- Azure Storage connection strings
- Database connection strings
- API keys
- Azure Functions runtime configuration

#### 2. App Settings with Secrets
```
appsettings.Development.json
appsettings.Local.json
connectionstrings.json
secrets.json
```
**Contains:**
- SQL Server connection strings
- JWT secret keys
- Third-party API keys

#### 3. Environment Files
```
.env
.env.local
.env.*.local
```
**Contains:**
- Environment-specific secrets
- API keys
- Database credentials

#### 4. Azure Publish Profiles
```
*.PublishSettings
*.pubxml (with passwords)
*.azurePubxml
```
**Contains:**
- Azure deployment credentials
- FTP passwords
- Database connection strings

#### 5. Certificates & Keys
```
*.pfx
*.p12
*.key
*.pem
id_rsa
*.ppk
```
**Contains:**
- SSL/TLS certificates
- Private keys
- SSH keys

#### 6. Database Files
```
*.db
*.sqlite
*.mdf
*.ldf
```
**Contains:**
- Local development data
- Contact form submissions
- Admin credentials (hashed, but still sensitive)

---

## Safe Configuration Practices

### ✅ DO COMMIT:

#### 1. Configuration Templates
```
appsettings.json (without secrets)
appsettings.Production.json (without secrets)
```

**Example Safe `appsettings.json`:**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "ConnectionStrings": {
    "PortfolioDb": "*** REPLACE IN PRODUCTION ***"
  },
  "JwtSettings": {
    "SecretKey": "*** GENERATE 32+ CHAR SECRET ***",
    "Issuer": "PortfolioAPI",
    "Audience": "PortfolioClient",
    "ExpirationMinutes": 60
  }
}
```

#### 2. Local Settings Template
Create `local.settings.template.json` (safe to commit):
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "ConnectionStrings__PortfolioDb": "*** YOUR CONNECTION STRING HERE ***"
  }
}
```

**Then developers copy to `local.settings.json` (git-ignored) and fill in real values.**

---

## GitHub Secrets (Safe Way to Store Secrets)

### For CI/CD Pipeline

Store these as **GitHub Repository Secrets** (Settings → Secrets and variables → Actions):

1. `AZURE_FUNCTIONAPP_PUBLISH_PROFILE` - Azure Functions deployment credentials
2. `AZURE_SQL_CONNECTION_STRING` - Production database connection string
3. `JWT_SECRET_KEY` - JWT signing key for production
4. Any other API keys needed for deployment

**These secrets are encrypted by GitHub and never exposed in logs.**

---

## Pre-Commit Security Checklist

Before committing ANY code, manually verify:

### 1. No Hardcoded Secrets
```bash
# Search for potential secrets in your code
grep -r "password" src/ --include="*.cs" --include="*.json"
grep -r "secret" src/ --include="*.cs" --include="*.json"
grep -r "api_key" src/ --include="*.cs" --include="*.json"
grep -r "connectionstring" src/ --include="*.cs" --include="*.json" -i
```

### 2. Planning Documents Are Ignored
```bash
git status
# Should NOT show:
# - broad_requirements.txt
# - REQUIREMENTS.md
# - ZERO-COST-CHECKLIST.md
# - IMPLEMENTATION-PLAN.md
```

### 3. No Local Config Files
```bash
git status
# Should NOT show:
# - local.settings.json
# - appsettings.Development.json
# - *.db files
```

### 4. No Build Artifacts
```bash
git status
# Should NOT show:
# - bin/
# - obj/
# - *.dll files
```

---

## What To Do If You Accidentally Commit Secrets

### If You Haven't Pushed Yet:

```bash
# Remove the file from staging
git reset HEAD <file>

# Amend the last commit
git commit --amend
```

### If You Already Pushed:

**🚨 CRITICAL: You MUST rotate all secrets immediately! 🚨**

1. **Rotate compromised secrets:**
   - Change database passwords
   - Generate new JWT secret keys
   - Regenerate API keys
   - Create new Azure publish profiles

2. **Remove from Git history:**
   ```bash
   # Use git-filter-repo to remove sensitive file from history
   pip install git-filter-repo
   git filter-repo --path <sensitive-file> --invert-paths

   # Force push (only if repository is private and you're the only developer)
   git push --force
   ```

3. **Update GitHub Secrets:**
   - Go to repository Settings → Secrets
   - Update all affected secrets

4. **Update Azure Configuration:**
   - Azure Portal → Function App → Configuration
   - Update connection strings and app settings

**⚠️ WARNING: If this is a public repository, assume the secrets are compromised forever.**

---

## Security Review Process

### Before Every Commit:
- [ ] Run `git status` and review every file being committed
- [ ] Search for "password", "secret", "key" in changed files
- [ ] Verify no planning documents are staged
- [ ] Verify no `.json` files with secrets are staged
- [ ] Verify no database files are staged

### Before Every Push:
- [ ] Review the commit history: `git log --oneline -5`
- [ ] Double-check no sensitive files in last commit: `git show --name-only`

### Weekly:
- [ ] Audit committed files: `git ls-files | grep -E '\.(json|config|env)$'`
- [ ] Verify .gitignore is working: `git check-ignore -v broad_requirements.txt`

---

## Safe Alternatives to Hardcoding

### Instead of This (UNSAFE):
```csharp
var connectionString = "Server=myserver.database.windows.net;Database=mydb;User Id=admin;Password=MyPassword123!;";
```

### Do This (SAFE):
```csharp
var connectionString = Configuration.GetConnectionString("PortfolioDb");
```

### Instead of This (UNSAFE):
```csharp
var jwtSecret = "my-super-secret-key-12345";
```

### Do This (SAFE):
```csharp
var jwtSecret = Configuration["JwtSettings:SecretKey"];
```

---

## Azure Security Best Practices

### 1. Use Managed Identity (Free)
Instead of connection strings in code, use Azure Managed Identity for Function App to access SQL Database.

### 2. Use Azure App Settings (Free)
Store all secrets in Azure Function App Configuration (Settings → Configuration → Application Settings).
These are encrypted at rest.

### 3. Don't Use Key Vault Initially (Costs Money)
Since we're on a $0 budget, use:
- Azure Function App Settings for secrets (free)
- GitHub Secrets for CI/CD (free)

Key Vault can be added later if budget allows.

---

## Additional Security Measures

### 1. Enable Dependabot (Free on GitHub)
- Settings → Security → Dependabot
- Enable Dependabot alerts for vulnerable dependencies
- Enable Dependabot security updates

### 2. Code Scanning (Free for Public Repos)
- Settings → Security → Code scanning
- Enable CodeQL analysis
- Automatically detects secrets and vulnerabilities

### 3. Secret Scanning (Free on GitHub)
- Automatically enabled for public repos
- GitHub will alert you if secrets are detected in commits

---

## Emergency Contacts

If you discover a security issue:

1. **Immediate Action:**
   - Rotate all potentially compromised secrets
   - Review access logs in Azure Portal
   - Check for unauthorized database access

2. **Containment:**
   - Remove sensitive data from Git history
   - Update all credentials
   - Monitor for suspicious activity

3. **Prevention:**
   - Review this security guide
   - Update .gitignore if needed
   - Add additional checks to CI/CD pipeline

---

## Useful Commands

### Check if a file is ignored:
```bash
git check-ignore -v <filename>
```

### List all tracked files:
```bash
git ls-files
```

### Search for potential secrets in code:
```bash
# Windows PowerShell
Select-String -Path src -Pattern "password|secret|key|connectionstring" -Recurse -Include *.cs,*.json

# Git Bash / Linux / Mac
grep -r -i "password\|secret\|key\|connectionstring" src/ --include="*.cs" --include="*.json"
```

### Verify .gitignore is working:
```bash
git status --ignored
```

---

## Compliance

This portfolio site handles:
- **Contact form submissions** (Name, Email, Message) - Personal data
- **Admin credentials** (Username, Password) - Sensitive authentication data

### GDPR Considerations (if applicable):
- Contact submissions stored in Azure SQL (encrypted at rest)
- No third-party tracking or analytics by default
- Admin can delete submissions on request

### Security Standards:
- Passwords hashed with ASP.NET Core Identity (PBKDF2)
- HTTPS enforced (GitHub Pages + Azure Functions)
- JWT tokens expire after 1 hour
- No sensitive data in logs

---

## Review Schedule

- **Daily:** Before each commit - review staged files
- **Weekly:** Audit .gitignore effectiveness
- **Monthly:** Review Azure access logs
- **Quarterly:** Rotate JWT secret keys
- **Annually:** Full security audit

---

## Status

✅ `.gitignore` configured (January 28, 2026)
✅ Planning documents protected
✅ Sensitive file patterns excluded
✅ Security guidelines documented

---

**Last Updated:** January 28, 2026
**Next Review:** February 28, 2026
