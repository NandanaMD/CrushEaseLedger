# Quick Start - Version Management

## To Build with Auto-Version

**Normal Build** (increments build number: 1.0.4 → 1.0.5):
```powershell
.\build-installer.ps1
```

**New Feature** (increments minor version: 1.0.4 → 1.1.0):
```powershell
.\build-installer.ps1 -VersionComponent minor
```

**Major Release** (increments major version: 1.0.4 → 2.0.0):
```powershell
.\build-installer.ps1 -VersionComponent major
```

**Testing** (no version change):
```powershell
.\build-installer.ps1 -SkipVersionIncrement
```

## What Happens?

1. **Version is incremented** in `CrushEase.csproj`
2. **Project is built** in Release mode
3. **Installer is created** with the new version number
4. **Output**: `installer\Output\CrushEaseLedger-Setup-v1.0.5.exe`

## Files Created

- ✅ `increment-version.ps1` - Standalone version incrementer
- ✅ `build-with-version.ps1` - Build project with version increment
- ✅ `build-installer.ps1` - Updated with auto-version (existing file enhanced)
- ✅ `VERSION_MANAGEMENT.md` - Complete documentation

## Current Status

Current version: **1.0.4**

Next build will be: **1.0.5** (if you use default)

---

**See VERSION_MANAGEMENT.md for complete documentation**
