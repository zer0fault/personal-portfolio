#!/bin/bash
# Pre-Commit Verification Script (Linux/Mac/Git Bash)
# Run this before committing to check for potential security issues

echo "====================================="
echo "Pre-Commit Security Verification"
echo "====================================="
echo ""

errors=0
warnings=0

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
CYAN='\033[0;36m'
NC='\033[0m' # No Color

# Check 1: Planning documents should NOT be in staged files
echo -e "${YELLOW}[1/6] Checking for planning documents...${NC}"
staged=$(git diff --cached --name-only)
sensitive_files=(
    "broad_requirements.txt"
    "REQUIREMENTS.md"
    "ZERO-COST-CHECKLIST.md"
    "IMPLEMENTATION-PLAN.md"
)

for file in "${sensitive_files[@]}"; do
    if echo "$staged" | grep -q "$file"; then
        echo -e "  ${RED}❌ ERROR: $file is staged for commit!${NC}"
        ((errors++))
    fi
done

if [ $errors -eq 0 ]; then
    echo -e "  ${GREEN}✅ No planning documents staged${NC}"
fi
echo ""

# Check 2: Configuration files with potential secrets
echo -e "${YELLOW}[2/6] Checking for configuration files...${NC}"
config_files=(
    "local.settings.json"
    "appsettings.Development.json"
    "appsettings.Local.json"
    "connectionstrings.json"
    "secrets.json"
    ".env"
)

for file in "${config_files[@]}"; do
    if echo "$staged" | grep -q "$file"; then
        echo -e "  ${RED}❌ ERROR: $file is staged!${NC}"
        ((errors++))
    fi
done

if [ $errors -eq 0 ]; then
    echo -e "  ${GREEN}✅ No sensitive config files staged${NC}"
fi
echo ""

# Check 3: Database files
echo -e "${YELLOW}[3/6] Checking for database files...${NC}"
db_patterns=("\.db$" "\.sqlite$" "\.mdf$" "\.ldf$")

for pattern in "${db_patterns[@]}"; do
    if echo "$staged" | grep -E "$pattern" > /dev/null; then
        echo -e "  ${RED}❌ ERROR: Database file staged!${NC}"
        ((errors++))
    fi
done

if [ $errors -eq 0 ]; then
    echo -e "  ${GREEN}✅ No database files staged${NC}"
fi
echo ""

# Check 4: Search for hardcoded secrets in staged files
echo -e "${YELLOW}[4/6] Searching for potential hardcoded secrets...${NC}"
patterns=(
    "password\s*="
    "secret\s*="
    "api[_-]?key"
    "connection\s*string\s*="
    "Server=.*Password="
    "User Id=.*Password="
)

found_secrets=false
for file in $staged; do
    if [[ "$file" =~ \.(cs|json|config)$ ]]; then
        for pattern in "${patterns[@]}"; do
            if grep -iE "$pattern" "$file" > /dev/null 2>&1; then
                echo -e "  ${YELLOW}⚠️  WARNING: Potential secret in $file${NC}"
                echo -e "     Pattern: $pattern"
                ((warnings++))
                found_secrets=true
            fi
        done
    fi
done

if [ "$found_secrets" = false ]; then
    echo -e "  ${GREEN}✅ No obvious secrets found${NC}"
fi
echo ""

# Check 5: Verify .gitignore is working
echo -e "${YELLOW}[5/6] Verifying .gitignore is working...${NC}"
git check-ignore broad_requirements.txt REQUIREMENTS.md ZERO-COST-CHECKLIST.md IMPLEMENTATION-PLAN.md > /dev/null 2>&1

if [ $? -eq 0 ]; then
    echo -e "  ${GREEN}✅ .gitignore is working correctly${NC}"
else
    echo -e "  ${RED}❌ ERROR: .gitignore may not be working!${NC}"
    ((errors++))
fi
echo ""

# Check 6: Build artifacts check
echo -e "${YELLOW}[6/6] Checking for build artifacts...${NC}"
artifacts=("bin/" "obj/" ".vs/")

for artifact in "${artifacts[@]}"; do
    if echo "$staged" | grep -q "$artifact"; then
        echo -e "  ${YELLOW}⚠️  WARNING: Build artifact staged: $artifact${NC}"
        ((warnings++))
    fi
done

if [ $warnings -eq 0 ]; then
    echo -e "  ${GREEN}✅ No build artifacts staged${NC}"
fi
echo ""

# Summary
echo -e "${CYAN}=====================================${NC}"
echo -e "${CYAN}Verification Summary${NC}"
echo -e "${CYAN}=====================================${NC}"
if [ $errors -eq 0 ]; then
    echo -e "Errors:   ${GREEN}$errors${NC}"
else
    echo -e "Errors:   ${RED}$errors${NC}"
fi

if [ $warnings -eq 0 ]; then
    echo -e "Warnings: ${GREEN}$warnings${NC}"
else
    echo -e "Warnings: ${YELLOW}$warnings${NC}"
fi
echo ""

if [ $errors -gt 0 ]; then
    echo -e "${RED}❌ COMMIT BLOCKED - Fix errors before committing!${NC}"
    echo ""
    echo -e "${YELLOW}Actions to take:${NC}"
    echo "1. Review staged files: git status"
    echo "2. Unstage sensitive files: git reset HEAD <file>"
    echo "3. Add to .gitignore if needed"
    exit 1
elif [ $warnings -gt 0 ]; then
    echo -e "${YELLOW}⚠️  WARNINGS DETECTED - Please review before committing${NC}"
    echo ""
    read -p "Do you want to proceed anyway? (yes/no): " response
    if [ "$response" != "yes" ]; then
        echo "Commit cancelled."
        exit 1
    fi
fi

echo -e "${GREEN}✅ All checks passed! Safe to commit.${NC}"
echo ""
exit 0
