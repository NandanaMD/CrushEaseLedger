# Building the CrushEase Ledger Installer

## Prerequisites

1. **Inno Setup** - Download and install from: https://jrsoftware.org/isdl.php
   - Get the latest version (currently 6.x)
   - Free and open source

## Steps to Create the Installer

### Step 1: Build Release Version

Open PowerShell in the project root and run:

```powershell
dotnet publish CrushEase\CrushEase.csproj -c Release -r win-x64 --self-contained false -o CrushEase\bin\Release\net8.0-windows
```

This creates an optimized release build of your application.

### Step 2: Generate the Installer

1. Open **Inno Setup Compiler**
2. Click **File → Open** and select: `installer\CrushEaseInstaller.iss`
3. Click **Build → Compile** (or press Ctrl+F9)

The installer will be created in: `installer\Output\CrushEaseLedger-Setup-v1.0.0.exe`

### Step 3: Test the Installer

1. Run the generated `.exe` file
2. Follow the installation wizard
3. Test the installed application

## Alternative: Self-Contained Deployment (No .NET Required)

If you want to create a version that doesn't require .NET to be installed:

```powershell
dotnet publish CrushEase\CrushEase.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o CrushEase\bin\Release\net8.0-windows
```

**Note**: This creates a larger installer (~100-150MB) but includes the .NET runtime.

To use this with the installer, update the `CrushEaseInstaller.iss` file:
- Change the Files section Source path if needed
- Remove the .NET check code from the `[Code]` section

## Quick Build Script

Use the provided script to automate the process:

```powershell
.\build-installer.ps1
```

## Distribution

Share the generated installer file with your clients:
- `CrushEaseLedger-Setup-v1.0.0.exe`

The installer will:
- Install the application to Program Files
- Create desktop shortcut (optional)
- Create Start Menu entries
- Check for .NET 8 Desktop Runtime (or include it if self-contained)
- Provide an uninstaller
