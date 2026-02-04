# Version Management Guide

## Overview
CrushEase now includes automatic version management that increments the version number each time you build or create an installer.

## Current Version
The version is stored in `CrushEase/CrushEase.csproj` and is currently **1.0.4**.

## Scripts

### 1. `increment-version.ps1`
Manually increment the version number.

**Usage:**
```powershell
# Increment build number (1.0.4 → 1.0.5)
.\increment-version.ps1

# Increment minor version (1.0.4 → 1.1.0)
.\increment-version.ps1 -Component minor

# Increment major version (1.0.4 → 2.0.0)
.\increment-version.ps1 -Component major
```

### 2. `build-with-version.ps1`
Build the project with automatic version increment.

**Usage:**
```powershell
# Build Release with auto-increment (default: build number)
.\build-with-version.ps1

# Build Debug with auto-increment
.\build-with-version.ps1 -Configuration Debug

# Build without incrementing version
.\build-with-version.ps1 -SkipVersionIncrement

# Increment minor version and build
.\build-with-version.ps1 -VersionComponent minor
```

### 3. `build-installer.ps1` (Updated)
Build the installer with automatic version increment.

**Usage:**
```powershell
# Build installer with auto-increment (default: build number)
.\build-installer.ps1

# Build installer without incrementing version
.\build-installer.ps1 -SkipVersionIncrement

# Increment minor version and build installer
.\build-installer.ps1 -VersionComponent minor
```

## Version Format
- **Format**: `Major.Minor.Build`
- **Example**: `1.2.5`
- **Components**:
  - **Major**: Breaking changes or major features
  - **Minor**: New features, backward compatible
  - **Build**: Bug fixes, small improvements

## Recommended Workflow

### For Development Builds
```powershell
# Just build without version increment for testing
.\build-with-version.ps1 -SkipVersionIncrement
```

### For Release Builds
```powershell
# Increment build number and create installer
.\build-installer.ps1
```

### For Feature Releases
```powershell
# Increment minor version for new features
.\build-installer.ps1 -VersionComponent minor
```

### For Major Releases
```powershell
# Increment major version for breaking changes
.\build-installer.ps1 -VersionComponent major
```

## Version Updates

The scripts update three version fields in the `.csproj` file:
1. `<Version>` - Used for display and GitHub releases
2. `<AssemblyVersion>` - Used by .NET runtime
3. `<FileVersion>` - Shown in Windows file properties

## Version Check Service

The app's `VersionService` automatically checks GitHub releases for updates. Make sure to:
1. Tag releases on GitHub with format `v1.0.5`
2. Keep the version in `.csproj` synchronized
3. The version check compares using semantic versioning

## Tips

- **CI/CD Integration**: These scripts can be called from GitHub Actions or other CI/CD pipelines
- **Manual Control**: Use `-SkipVersionIncrement` when testing or when you don't want to bump the version
- **Version History**: Consider committing the `.csproj` file after version increments to track version history in Git
- **Rollback**: If you need to rollback a version, manually edit the `.csproj` file

## Example Scenarios

### Scenario 1: Bug Fix Release
```powershell
# Current: 1.0.4
.\build-installer.ps1
# Result: 1.0.5
```

### Scenario 2: New Feature Release
```powershell
# Current: 1.0.5
.\build-installer.ps1 -VersionComponent minor
# Result: 1.1.0
```

### Scenario 3: Testing Without Version Change
```powershell
# Current: 1.1.0
.\build-with-version.ps1 -SkipVersionIncrement
# Result: Still 1.1.0
```

### Scenario 4: Major Rewrite
```powershell
# Current: 1.5.3
.\build-installer.ps1 -VersionComponent major
# Result: 2.0.0
```
