# Auto-increment version script for CrushEase
# This script increments the build number in the .csproj file before building

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("major", "minor", "build")]
    [string]$Component = "build"
)

$projectFile = Join-Path $PSScriptRoot "CrushEase\CrushEase.csproj"

if (-not (Test-Path $projectFile)) {
    Write-Error "Project file not found: $projectFile"
    exit 1
}

Write-Host "Reading project file: $projectFile" -ForegroundColor Cyan

# Read the project file
$content = Get-Content $projectFile -Raw

# Extract current version using regex
if ($content -match '<Version>(\d+)\.(\d+)\.(\d+)</Version>') {
    $major = [int]$matches[1]
    $minor = [int]$matches[2]
    $build = [int]$matches[3]
    
    $oldVersion = "$major.$minor.$build"
    Write-Host "Current version: $oldVersion" -ForegroundColor Yellow
    
    # Increment based on component
    switch ($Component) {
        "major" {
            $major++
            $minor = 0
            $build = 0
        }
        "minor" {
            $minor++
            $build = 0
        }
        "build" {
            $build++
        }
    }
    
    $newVersion = "$major.$minor.$build"
    Write-Host "New version: $newVersion" -ForegroundColor Green
    
    # Update all version tags
    $content = $content -replace '<Version>\d+\.\d+\.\d+</Version>', "<Version>$newVersion</Version>"
    $content = $content -replace '<AssemblyVersion>\d+\.\d+\.\d+\.\d+</AssemblyVersion>', "<AssemblyVersion>$newVersion.0</AssemblyVersion>"
    $content = $content -replace '<FileVersion>\d+\.\d+\.\d+\.\d+</FileVersion>', "<FileVersion>$newVersion.0</FileVersion>"
    
    # Write back to file
    Set-Content -Path $projectFile -Value $content -NoNewline
    
    Write-Host "[OK] Version updated successfully!" -ForegroundColor Green
    Write-Host "  From: $oldVersion" -ForegroundColor Gray
    Write-Host "  To:   $newVersion" -ForegroundColor Gray
    
    # Return the new version for use in CI/CD
    return $newVersion
} else {
    Write-Error "Could not find version in project file"
    exit 1
}
