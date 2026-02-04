# Build and Publish Script with Auto-Increment Version
# This script automatically increments the build version and then builds the project

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("major", "minor", "build")]
    [string]$VersionComponent = "build",
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipVersionIncrement,
    
    [Parameter(Mandatory=$false)]
    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"

Write-Host "=====================================" -ForegroundColor Cyan
Write-Host "  CrushEase Build Script (Auto-Version)" -ForegroundColor Cyan
Write-Host "=====================================" -ForegroundColor Cyan
Write-Host ""

$projectPath = Join-Path $PSScriptRoot "CrushEase\CrushEase.csproj"
$solutionPath = Join-Path $PSScriptRoot "CrushEase.sln"

# Step 1: Increment version (unless skipped)
if (-not $SkipVersionIncrement) {
    Write-Host "[1/3] Incrementing version ($VersionComponent)..." -ForegroundColor Yellow
    $newVersion = & (Join-Path $PSScriptRoot "increment-version.ps1") -Component $VersionComponent
    Write-Host ""
} else {
    Write-Host "[1/3] Skipping version increment (as requested)" -ForegroundColor Gray
    Write-Host ""
}

# Step 2: Clean previous build
Write-Host "[2/3] Cleaning previous build..." -ForegroundColor Yellow
try {
    dotnet clean $solutionPath --configuration $Configuration --verbosity quiet
    Write-Host "✓ Clean completed" -ForegroundColor Green
    Write-Host ""
} catch {
    Write-Warning "Clean failed, continuing anyway..."
}

# Step 3: Build project
Write-Host "[3/3] Building project ($Configuration)..." -ForegroundColor Yellow
try {
    dotnet build $solutionPath --configuration $Configuration --no-incremental
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "=====================================" -ForegroundColor Green
        Write-Host "  BUILD SUCCESSFUL!" -ForegroundColor Green
        Write-Host "=====================================" -ForegroundColor Green
        
        # Show output location
        $outputPath = Join-Path $PSScriptRoot "CrushEase\bin\$Configuration\net8.0-windows"
        Write-Host ""
        Write-Host "Output location:" -ForegroundColor Cyan
        Write-Host "  $outputPath" -ForegroundColor Gray
    } else {
        Write-Host ""
        Write-Host "=====================================" -ForegroundColor Red
        Write-Host "  BUILD FAILED!" -ForegroundColor Red
        Write-Host "=====================================" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Error "Build failed: $_"
    exit 1
}
