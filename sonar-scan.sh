#!/bin/bash

# SonarQube Analysis Script
# This script runs a complete SonarQube scan on the AutomotiveApp project

echo "🔍 Starting SonarQube Analysis for AutomotiveApp..."
echo "=============================================="

# Configuration
PROJECT_KEY="project-automotive"
PROJECT_NAME="project-automotive"
SONAR_HOST="http://localhost:9000"
SONAR_TOKEN="sqp_b7979e39f01799d6094296e5a9e5b5ec30d6559e" # Read from environment or export earlier

# Colors for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Check if SonarQube is running
echo ""
echo "📡 Checking SonarQube server..."
if curl -s "$SONAR_HOST/api/system/status" > /dev/null; then
    echo -e "${GREEN}✓ SonarQube server is running${NC}"
else
    echo -e "${RED}✗ SonarQube server is not accessible at $SONAR_HOST${NC}"
    echo "Please start SonarQube using: docker-compose up -d"
    exit 1
fi

# Ensure SONAR_TOKEN is set
if [ -z "$SONAR_TOKEN" ]; then
    echo -e "${YELLOW}⚠ SONAR_TOKEN is not set. Export your token or set it in this script before running.${NC}"
    echo "Example: export SONAR_TOKEN=your_token_here"
    exit 1
fi

# Check if dotnet-sonarscanner is installed
echo ""
echo "🔧 Checking dotnet-sonarscanner..."
if dotnet tool list -g | grep -q "dotnet-sonarscanner"; then
    echo -e "${GREEN}✓ dotnet-sonarscanner is installed${NC}"
else
    echo -e "${YELLOW}⚠ dotnet-sonarscanner not found. Installing...${NC}"
    dotnet tool install --global dotnet-sonarscanner
fi

# Clean previous build
echo ""
echo "🧹 Cleaning previous build..."
dotnet clean AutomotiveApp.sln

# Begin SonarQube analysis
echo ""
echo "🚀 Starting SonarQube scanner..."
dotnet sonarscanner begin \
    /k:"$PROJECT_KEY" \
    /n:"$PROJECT_NAME" \
    /d:sonar.host.url="$SONAR_HOST" \
    /d:sonar.login="$SONAR_TOKEN" \
    /d:sonar.cs.opencover.reportsPaths="**/coverage.opencover.xml" \
    /d:sonar.cs.vstest.reportsPaths="**/*.trx" \
    /d:sonar.coverage.exclusions="**/Migrations/**,**/wwwroot/**,**/*.cshtml,**/Program.cs" \
    /d:sonar.exclusions="**/wwwroot/**,**/obj/**,**/bin/**,**/*.sh,**/*.ps1,**/sonar-scan.sh,**/sonar-scan.ps1,**/appsettings.json"
    
if [ $? -ne 0 ]; then
    echo -e "${RED}✗ Failed to start SonarQube scanner${NC}"
    exit 1
fi

# Build the project
echo ""
echo "🔨 Building project..."
dotnet build AutomotiveApp.sln --configuration Release

if [ $? -ne 0 ]; then
    echo -e "${RED}✗ Build failed${NC}"
    exit 1
fi

# Run tests with coverage (optional but recommended)
echo ""
echo "🧪 Running tests with coverage..."
dotnet test AutomotiveApp.sln \
    --configuration Release \
    --no-build \
    --logger "trx" \
    /p:CollectCoverage=true \
    /p:CoverletOutputFormat=opencover \
    /p:CoverletOutput=./TestResults/

# End SonarQube analysis
echo ""
echo "📤 Uploading results to SonarQube..."
dotnet sonarscanner end /d:sonar.login="$SONAR_TOKEN"

if [ $? -eq 0 ]; then
    echo ""
    echo -e "${GREEN}✓ SonarQube analysis completed successfully!${NC}"
    echo ""
    echo "📊 View results at: $SONAR_HOST/dashboard?id=$PROJECT_KEY"
    echo ""
    echo "💡 Tips:"
    echo "  - Check 'Issues' tab for code smells, bugs, and vulnerabilities"
    echo "  - Review 'Security Hotspots' for potential security issues"
    echo "  - Monitor 'Coverage' to see test coverage metrics"
    echo ""
else
    echo -e "${RED}✗ SonarQube analysis failed${NC}"
    exit 1
fi
