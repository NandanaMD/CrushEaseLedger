# Version Information System

## Overview
CrushEase now includes a comprehensive version management system that:
- Displays the current version in the status bar
- Checks for updates automatically from GitHub releases
- Shows update availability status (Latest / Update Available)
- Provides an About dialog with detailed version information

## Features

### 1. Status Bar Version Display
- **Location**: Bottom-right corner of the main window
- **Display Format**:
  - `Version: 1.0.0 (Latest)` - When running the latest version (Green)
  - `Version: 1.0.0 (Update Available: v1.1.0)` - When an update is available (Orange)
  - `Version: 1.0.0` - When update check fails (Default color)

### 2. About Dialog
- **Access**: Help menu → About CrushEase
- **Features**:
  - Shows app logo and name
  - Displays current version
  - Shows update status with visual indicators
  - "Check for Updates" button for manual checking
  - "Download Update" button (appears when update is available)
  - Release notes preview for the latest version

### 3. Automatic Update Checking
- Checks for updates automatically on application startup
- Caches results for 1 hour to avoid excessive API calls
- Compares current version with the latest GitHub release
- Non-intrusive - won't block the UI or interrupt work

## How It Works

### Version Detection
- Current version is read from the assembly metadata (`AssemblyVersion`)
- Configured in `CrushEase.csproj`:
  ```xml
  <Version>1.0.0</Version>
  <AssemblyVersion>1.0.0.0</AssemblyVersion>
  <FileVersion>1.0.0.0</FileVersion>
  ```

### Update Checking
- Queries GitHub API: `https://api.github.com/repos/NandanaMD/CrushEaseLedger/releases/latest`
- Parses the latest release version from the `tag_name` field
- Compares versions using semantic versioning
- Falls back gracefully if internet is unavailable

### Version Comparison
- Uses .NET `Version` class for accurate comparison
- Handles formats like "v1.2.0" or "1.2.0"
- Determines if update is available based on version numbers

## Files Added/Modified

### New Files
1. **Services/VersionService.cs**
   - Core service for version management
   - Handles GitHub API communication
   - Provides version comparison logic
   - Caches results to minimize API calls

2. **Forms/AboutForm.cs**
   - Complete About dialog with version information
   - Visual update status indicators
   - Integrated update checking UI

### Modified Files
1. **CrushEase.csproj**
   - Added `AssemblyVersion` and `FileVersion` properties

2. **Forms/MainForm.cs**
   - Added `MenuAbout_Click` handler
   - Added `LoadVersionInfoAsync` method
   - Loads version info on form startup

3. **Forms/MainForm.Designer.cs**
   - Added "About CrushEase" menu item in Help menu
   - Added `lblVersionStatus` to status bar (right side)
   - Added separator in Help menu

## Usage for Users

### Check Current Version
1. Look at the bottom-right corner of the main window
2. The version is always displayed in the status bar

### Check for Updates
1. Click **Help** → **About CrushEase**
2. The dialog will automatically check for updates
3. Click "Check for Updates" to manually refresh
4. If an update is available, click "Download Update" to go to the download page

### Understanding Version Status
- **✓ You have the latest version** (Green) - No action needed
- **✓ Update Available: vX.X.X** (Orange) - New version available for download
- **Unable to check for updates** (Orange) - Internet connection issue

## For Developers

### Releasing a New Version

1. **Update version in CrushEase.csproj**:
   ```xml
   <Version>1.1.0</Version>
   <AssemblyVersion>1.1.0.0</AssemblyVersion>
   <FileVersion>1.1.0.0</FileVersion>
   ```

2. **Build the installer** using the existing build script

3. **Create a GitHub Release**:
   - Go to GitHub repository → Releases → Create new release
   - Tag version: `v1.1.0` (must start with 'v')
   - Release title: `CrushEase Ledger v1.1.0`
   - Description: Add release notes
   - Attach the installer executable
   - Publish release

4. **The app will automatically detect the new version**:
   - Users will see "Update Available" in the status bar
   - About dialog will show the update information
   - Users can click to download the new version

### Testing Update Detection

To test without publishing a real release:
1. Temporarily change `VERSION_CHECK_URL` in `VersionService.cs`
2. Point to a test API endpoint or mock response
3. Test various scenarios (latest, outdated, offline)

### Version Number Guidelines
- Follow semantic versioning: `MAJOR.MINOR.PATCH`
- **MAJOR**: Breaking changes
- **MINOR**: New features (backward compatible)
- **PATCH**: Bug fixes

## Technical Details

### API Rate Limiting
- GitHub API has rate limits (60 requests/hour unauthenticated)
- The app caches results for 1 hour to minimize requests
- Only checks on startup and when manually requested

### Error Handling
- Gracefully handles network failures
- Falls back to showing current version only
- Logs errors for debugging
- Never crashes due to update check failures

### Privacy
- No user data is sent to GitHub
- Only checks public release information
- All communication is over HTTPS
- No tracking or analytics

## Future Enhancements

Potential improvements:
- [ ] Auto-download and install updates
- [ ] Update notification toast on startup
- [ ] Release notes viewer in the app
- [ ] Beta/Stable channel selection
- [ ] Update history log
