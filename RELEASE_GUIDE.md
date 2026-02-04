# Automated Release System - Quick Guide

## 🚀 How to Create a New Release

### Simple Method (Recommended)

Just run this command:

```powershell
.\create-release.ps1
```

This will automatically:
1. ✅ Increment the build version (e.g., 1.0.0 → 1.0.1)
2. ✅ Commit your changes
3. ✅ Create a git tag (e.g., v1.0.1)
4. ✅ Push to GitHub
5. ✅ Trigger automated build and release

### Advanced Options

**Increment minor version:**
```powershell
.\create-release.ps1 -VersionComponent minor
```
Result: 1.0.5 → 1.1.0

**Increment major version:**
```powershell
.\create-release.ps1 -VersionComponent major
```
Result: 1.0.5 → 2.0.0

**Add custom message:**
```powershell
.\create-release.ps1 -Message "Added new invoice features"
```

---

## 📦 What Happens Automatically

Once you push a tag, **GitHub Actions** automatically:

1. **Builds** the project with .NET 8
2. **Creates** installer with Inno Setup
3. **Publishes** GitHub release with:
   - Release notes (generated from commits)
   - Installer download link
   - Version details
4. **Notifies** users (they'll see update popup in the app)

⏱️ **Time:** ~5-10 minutes from tag push to release published

---

## 🎯 User Experience

When you publish a new version:

1. **App checks for updates on startup** (silently, after 3 seconds)
2. **Users see a calm notification popup** if update is available
3. **One-click download & install** - users can:
   - Click "Download & Install" to auto-download and install
   - Click "Later" to dismiss (they can check manually later)
4. **Progress tracking** during download
5. **Automatic installation** with admin elevation

---

## 📋 Release Workflow

```
1. Make your code changes
   ↓
2. Run: .\create-release.ps1
   ↓
3. Script increments version and creates tag
   ↓
4. GitHub Actions builds and publishes release
   ↓
5. Users get notified automatically in the app
```

---

## 🔍 Monitor Release Progress

**GitHub Actions:**
https://github.com/NandanaMD/CrushEaseLedger/actions

**View Releases:**
https://github.com/NandanaMD/CrushEaseLedger/releases

---

## 🛠️ Manual Release (If Needed)

If you prefer manual control:

```powershell
# 1. Increment version
.\increment-version.ps1 -Component build

# 2. Commit changes
git add .
git commit -m "Release v1.0.1"

# 3. Create tag
git tag -a v1.0.1 -m "Release v1.0.1"

# 4. Push tag
git push origin v1.0.1
```

GitHub Actions will still handle the build and release automatically.

---

## 📝 Version Numbering

Format: **MAJOR.MINOR.BUILD**

- **MAJOR** (1.x.x) - Breaking changes, major features
- **MINOR** (x.1.x) - New features, improvements
- **BUILD** (x.x.1) - Bug fixes, small updates

Examples:
- `1.0.0` → `1.0.1` - Bug fix
- `1.0.5` → `1.1.0` - New feature
- `1.5.0` → `2.0.0` - Major overhaul

---

## ⚙️ GitHub Actions Configuration

The workflow file is located at:
```
.github/workflows/release.yml
```

It automatically:
- Uses Windows runner (required for .NET WinForms)
- Installs Inno Setup
- Builds self-contained executable
- Creates installer
- Uploads to GitHub Releases

**No manual configuration needed!** Just push a tag.

---

## 🔐 Requirements

- Git repository connected to GitHub
- GitHub Actions enabled (free for public repos)
- Tags must follow format: `v1.0.0` (automatic with `create-release.ps1`)

---

## ❓ FAQ

**Q: Do I need to install Inno Setup locally?**
A: No! GitHub Actions installs it automatically in the cloud.

**Q: Can I test before releasing?**
A: Yes! Use `.\build-installer.ps1` to build locally first.

**Q: What if the build fails?**
A: Check GitHub Actions logs. Common issues:
   - Build errors in code
   - Missing files in .csproj
   - Inno Setup script errors

**Q: How do I delete a release?**
A: Go to GitHub Releases, click the release, then "Delete release"

---

## 🎉 Summary

**To create a release:**
```powershell
.\create-release.ps1
```

**That's it!** Everything else is automated. 🚀

Your users will automatically see the update notification in the app, can download with one click, and install seamlessly.
