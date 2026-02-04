# CrushEase Ledger - Automated Installer Build Script with Auto-Version

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("major", "minor", "build")]
    [string]$VersionComponent = "build",
    
    [Parameter(Mandatory=$false)]
    [switch]$SkipVersionIncrement
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  CrushEase Ledger - Build Installer   " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Step 0: Increment version (unless skipped)
if (-not $SkipVersionIncrement) {
    Write-Host "Step 0: Auto-incrementing version ($VersionComponent)..." -ForegroundColor Yellow
    $newVersion = & (Join-Path $PSScriptRoot "increment-version.ps1") -Component $VersionComponent
    Write-Host ""
} else {
    Write-Host "Step 0: Version increment skipped" -ForegroundColor Gray
    Write-Host ""
}

# Extract version from .csproj for installer naming
$projectFile = Join-Path $PSScriptRoot "CrushEase\CrushEase.csproj"
$projectContent = Get-Content $projectFile -Raw
if ($projectContent -match '<Version>(\d+\.\d+\.\d+)</Version>') {
    $version = $matches[1]
    Write-Host "Building version: $version" -ForegroundColor Cyan
    Write-Host ""
} else {
    $version = "1.0.0"
    Write-Warning "Could not extract version, using default: $version"
}

# Step 1: Build Release Version (Self-Contained - includes .NET runtime)
Write-Host "Step 1: Building self-contained Release version (includes .NET)..." -ForegroundColor Yellow
dotnet publish CrushEase\CrushEase.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -o CrushEase\bin\Release\net8.0-windows

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host ""

# Step 2: Check if Inno Setup is installed
Write-Host "Step 2: Checking for Inno Setup..." -ForegroundColor Yellow

$innoSetupPath = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
if (-not (Test-Path $innoSetupPath)) {
    Write-Host "Inno Setup not found!" -ForegroundColor Red
    Write-Host "Please download and install from: https://jrsoftware.org/isdl.php" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "After installation, run this script again." -ForegroundColor Yellow
    exit 1
}

Write-Host "Inno Setup found!" -ForegroundColor Green
Write-Host ""

# Step 3: Compile Installer
Write-Host "Step 3: Compiling installer..." -ForegroundColor Yellow
& $innoSetupPath "installer\CrushEaseInstaller.iss" "/DMyAppVersion=$version"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Installer compilation failed!" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  Installer created successfully!      " -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Version: $version" -ForegroundColor Cyan
Write-Host "Installer location:" -ForegroundColor Cyan
Write-Host "  installer\Output\CrushEaseLedger-Setup-v$version.exe" -ForegroundColor White
Write-Host ""
Write-Host "You can now share this file with your clients!" -ForegroundColor Yellow
