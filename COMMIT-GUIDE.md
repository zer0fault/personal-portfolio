# Commit Safety Guide

## Quick Reference: How to Commit Safely

This guide shows you how to commit changes without accidentally leaking sensitive information.

---

## Before Every Commit: Run Verification Script

### On Windows (PowerShell):
```powershell
.\verify-commit.ps1
```

### On Linux/Mac/Git Bash:
```bash
./verify-commit.sh
```

**The script will check:**
- ✅ No planning documents staged
- ✅ No config files with secrets staged
- ✅ No database files staged
- ✅ No hardcoded secrets in code
- ✅ .gitignore is working
- ✅ No build artifacts staged

---

## Safe Commit Workflow

### 1. Make Your Changes
```bash
# Edit your code
# Test your changes locally
```

### 2. Check What Will Be Committed
```bash
git status
```

**Expected output (safe files only):**
```
Changes not staged for commit:
  modified:   src/Domain/Entities/Project.cs
  modified:   src/BlazorApp/Pages/Home.razor
```

**⚠️ WARNING - If you see these files, DO NOT COMMIT:**
```
❌ broad_requirements.txt
❌ REQUIREMENTS.md
❌ ZERO-COST-CHECKLIST.md
❌ IMPLEMENTATION-PLAN.md
❌ local.settings.json
❌ appsettings.Development.json
❌ *.db files
❌ *.sqlite files
```

### 3. Stage Your Changes
```bash
# Stage specific files (RECOMMENDED)
git add src/Domain/Entities/Project.cs
git add src/BlazorApp/Pages/Home.razor

# OR stage all (be careful!)
git add .
```

### 4. Run Verification Script
```bash
# Windows PowerShell
.\verify-commit.ps1

# Linux/Mac/Git Bash
./verify-commit.sh
```

### 5. Review Output
**✅ If all checks pass:**
```
✅ All checks passed! Safe to commit.
```
Proceed to commit.

**❌ If errors found:**
```
❌ COMMIT BLOCKED - Fix errors before committing!
```
Fix the issues before committing.

**⚠️ If warnings found:**
```
⚠️ WARNINGS DETECTED - Please review before committing
```
Review warnings and decide if safe to proceed.

### 6. Commit
```bash
git commit -m "Add project detail modal component"
```

### 7. Push
```bash
git push origin main
```

---

## Common Scenarios

### Scenario 1: Accidentally Staged Sensitive File

**Problem:**
```bash
git status
# Shows: broad_requirements.txt staged
```

**Solution:**
```bash
# Unstage the file
git reset HEAD broad_requirements.txt

# Verify it's unstaged
git status
```

### Scenario 2: Planning Document Showing in Git Status

**Problem:**
```bash
git status
# Shows: REQUIREMENTS.md as untracked
```

**Solution:**
This shouldn't happen if .gitignore is working. Check:
```bash
# Verify file is ignored
git check-ignore -v REQUIREMENTS.md

# Output should be:
# .gitignore:6:REQUIREMENTS.md    REQUIREMENTS.md
```

If not working, ensure .gitignore has these lines:
```
broad_requirements.txt
REQUIREMENTS.md
ZERO-COST-CHECKLIST.md
IMPLEMENTATION-PLAN.md
```

### Scenario 3: Need to Update Planning Documents

**Safe workflow:**
1. Edit planning documents (they're never committed)
2. Make code changes based on planning
3. Stage ONLY code files
4. Run verification script
5. Commit code changes

Planning documents stay local only.

### Scenario 4: Need to Share Planning with Team

**Option 1: Private Wiki/Notion**
Copy planning docs to private wiki/Notion (not Git).

**Option 2: Private Repo**
Create separate private repository for planning docs.

**Option 3: Encrypted Share**
Use encrypted file sharing service.

**❌ DO NOT: Push to GitHub**

---

## Manual Verification (If Scripts Fail)

### Check 1: Review Staged Files
```bash
git diff --cached --name-only
```

None of these should appear:
- broad_requirements.txt
- REQUIREMENTS.md
- ZERO-COST-CHECKLIST.md
- IMPLEMENTATION-PLAN.md
- local.settings.json
- appsettings.Development.json
- *.db, *.sqlite

### Check 2: Search for Secrets
```bash
# Windows PowerShell
Select-String -Path src -Pattern "password|secret|key" -Recurse -Include *.cs

# Linux/Mac/Git Bash
grep -r "password\|secret\|key" src/ --include="*.cs"
```

Should return no hardcoded secrets.

### Check 3: Verify .gitignore
```bash
git check-ignore -v broad_requirements.txt
# Should show: .gitignore:5:broad_requirements.txt
```

---

## Emergency: Already Committed Secret

### If NOT Yet Pushed:
```bash
# Remove file from staging
git reset HEAD <file>

# Amend the commit
git commit --amend
```

### If Already Pushed:
**🚨 CRITICAL: Rotate ALL secrets immediately! 🚨**

1. **Change all passwords/keys:**
   - Database passwords
   - JWT secret keys
   - API keys
   - Azure credentials

2. **Remove from history:**
   ```bash
   # Install git-filter-repo
   pip install git-filter-repo

   # Remove file from history
   git filter-repo --path <sensitive-file> --invert-paths

   # Force push (ONLY if you're the only developer)
   git push --force
   ```

3. **Update all systems:**
   - Azure App Settings
   - GitHub Secrets
   - Local configurations

---

## Best Practices

### ✅ DO:
- Run verification script before EVERY commit
- Review `git status` before staging
- Stage files individually (not `git add .` blindly)
- Use `git diff --cached` to review changes before commit
- Keep planning docs local only
- Use GitHub Secrets for CI/CD
- Use Azure App Settings for production secrets

### ❌ DON'T:
- Commit planning documents
- Commit local.settings.json
- Commit database files
- Commit *.Development.json files
- Hardcode secrets in code
- Use `git add .` without reviewing
- Skip the verification script
- Ignore warnings

---

## Quick Commands Reference

```bash
# Check what's staged
git status
git diff --cached --name-only

# Unstage file
git reset HEAD <file>

# Unstage all
git reset HEAD

# Run verification
.\verify-commit.ps1              # Windows
./verify-commit.sh               # Linux/Mac

# Check if file is ignored
git check-ignore -v <filename>

# View ignored files
git status --ignored

# Search for potential secrets
grep -r "password" src/ --include="*.cs"
```

---

## Troubleshooting

### "Script execution disabled" (PowerShell)
```powershell
# Allow script execution (one time)
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

# Run script
.\verify-commit.ps1
```

### ".gitignore not working"
```bash
# Clear Git cache
git rm -r --cached .
git add .
git commit -m "Fix .gitignore"
```

### "Planning doc showing in git status"
```bash
# Check .gitignore content
cat .gitignore | grep -E "broad_requirements|REQUIREMENTS|IMPLEMENTATION"

# Should show all planning files listed
```

---

## Summary Checklist

Before every commit:
- [ ] Run `git status` to see what will be committed
- [ ] Run `.\verify-commit.ps1` (Windows) or `./verify-commit.sh` (Linux/Mac)
- [ ] Review any warnings
- [ ] Verify no planning documents staged
- [ ] Verify no secrets in code
- [ ] Commit with descriptive message
- [ ] Push to GitHub

**Remember: It's better to be cautious than to leak secrets!**

---

## Quick Test

Test your setup right now:

```bash
# This should return the file (means it's ignored)
git check-ignore broad_requirements.txt

# This should NOT list planning documents
git status

# This should pass all checks (once you have files staged)
./verify-commit.sh
```

If all three work, you're protected! ✅

---

**Last Updated:** January 28, 2026
**Next Review:** Before first commit
