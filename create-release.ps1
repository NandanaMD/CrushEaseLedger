# Create GitHub Release Tag
# This script increments the version, commits, and creates a git tag
# The tag triggers GitHub Actions to automatically build and publish the release

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("major", "minor", "build")]
    [string]$VersionComponent = "build",
    
    [Parameter(Mandatory=$false)]
    [string]$Message = ""
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Create GitHub Release Tag             " -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Step 1: Increment version
Write-Host "Step 1: Incrementing version ($VersionComponent)..." -ForegroundColor Yellow
$newVersion = & (Join-Path $PSScriptRoot "increment-version.ps1") -Component $VersionComponent

if (-not $newVersion) {
    Write-Host "Failed to increment version!" -ForegroundColor Red
    exit 1
}

Write-Host "New version: $newVersion" -ForegroundColor Green
Write-Host ""

# Step 2: Check git status
Write-Host "Step 2: Checking git status..." -ForegroundColor Yellow
$status = git status --porcelain

if ($status) {
    Write-Host "You have uncommitted changes:" -ForegroundColor Yellow
    git status --short
    Write-Host ""
    
    $response = Read-Host "Do you want to commit these changes? (y/n)"
    
    if ($response -eq 'y' -or $response -eq 'Y') {
        # Step 3: Commit changes
        Write-Host ""
        Write-Host "Step 3: Committing changes..." -ForegroundColor Yellow
        
        if ([string]::IsNullOrWhiteSpace($Message)) {
            $Message = "Release v$newVersion"
        }
        
        git add .
        git commit -m $Message
        
        if ($LASTEXITCODE -ne 0) {
            Write-Host "Commit failed!" -ForegroundColor Red
            exit 1
        }
        
        Write-Host "Changes committed successfully!" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "Please commit your changes manually, then run this script again." -ForegroundColor Yellow
        exit 0
    }
} else {
    Write-Host "Working directory is clean" -ForegroundColor Green
}

Write-Host ""

# Step 4: Create and push tag
Write-Host "Step 4: Creating and pushing git tag..." -ForegroundColor Yellow

$tagName = "v$newVersion"

# Check if tag already exists
$existingTag = git tag -l $tagName

if ($existingTag) {
    Write-Host "Tag $tagName already exists!" -ForegroundColor Red
    $response = Read-Host "Do you want to delete and recreate it? (y/n)"
    
    if ($response -eq 'y' -or $response -eq 'Y') {
        Write-Host "Deleting existing tag..." -ForegroundColor Yellow
        git tag -d $tagName
        git push origin --delete $tagName 2>$null
    } else {
        Write-Host "Aborted." -ForegroundColor Yellow
        exit 0
    }
}

# Create annotated tag
$tagMessage = "Release v$newVersion"
if (-not [string]::IsNullOrWhiteSpace($Message)) {
    $tagMessage = $Message
}

git tag -a $tagName -m $tagMessage

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to create tag!" -ForegroundColor Red
    exit 1
}

Write-Host "Tag $tagName created successfully!" -ForegroundColor Green
Write-Host ""

# Push tag to trigger GitHub Actions
Write-Host "Pushing tag to GitHub..." -ForegroundColor Yellow
git push origin $tagName

if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to push tag!" -ForegroundColor Red
    Write-Host "You can manually push the tag with: git push origin $tagName" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "  Release Tag Created Successfully!     " -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Tag: $tagName" -ForegroundColor Cyan
Write-Host ""
Write-Host "GitHub Actions will now automatically:" -ForegroundColor Yellow
Write-Host "  1. Build the project" -ForegroundColor White
Write-Host "  2. Create the installer" -ForegroundColor White
Write-Host "  3. Create a GitHub release" -ForegroundColor White
Write-Host "  4. Upload the installer" -ForegroundColor White
Write-Host ""
Write-Host "Monitor progress at:" -ForegroundColor Cyan
Write-Host "https://github.com/NandanaMD/CrushEaseLedger/actions" -ForegroundColor White
Write-Host ""
Write-Host "Once complete, the release will be available at:" -ForegroundColor Cyan
Write-Host "https://github.com/NandanaMD/CrushEaseLedger/releases/tag/$tagName" -ForegroundColor White
Write-Host ""
