# 🚀 Quick Start: Auto-Update & GitHub Releases

## For You (Developer)

### Create a New Release

```powershell
.\create-release.ps1
```

Done! This automatically:
1. Increments version (1.0.0 → 1.0.1)
2. Commits changes
3. Creates & pushes git tag
4. Triggers GitHub Actions
5. Builds and publishes release

**Monitor:** https://github.com/NandanaMD/CrushEaseLedger/actions

---

## For Your Users

### What They See

1. **App starts** → Silent update check (after 3 seconds)
2. **If update available** → Calm popup appears:
   - Shows new version
   - Shows release notes
   - Two buttons: "Download & Install" or "Later"
3. **Click Download** → Progress bar shows download
4. **Download complete** → Prompt to install
5. **Click OK** → App closes, installer runs
6. **Done!** → New version installed

### What They Experience

- ✅ Non-intrusive notification
- ✅ One-click download
- ✅ Automatic installation
- ✅ Can postpone if busy
- ✅ No manual steps needed

---

## Version Options

```powershell
# Bug fix: 1.0.0 → 1.0.1
.\create-release.ps1

# New feature: 1.0.5 → 1.1.0
.\create-release.ps1 -VersionComponent minor

# Major release: 1.5.0 → 2.0.0
.\create-release.ps1 -VersionComponent major
```

---

## Files You Need to Know

- **create-release.ps1** - Creates releases (use this!)
- **.github/workflows/release.yml** - GitHub Actions config
- **RELEASE_GUIDE.md** - Detailed documentation
- **AUTO_UPDATE_IMPLEMENTATION.md** - Technical details

---

## First Time Setup

1. Push workflow to GitHub:
   ```bash
   git add .github/workflows/release.yml
   git commit -m "Add auto-release workflow"
   git push
   ```

2. Create first release:
   ```powershell
   .\create-release.ps1
   ```

3. Wait 5-10 minutes for GitHub Actions

4. Check releases:
   https://github.com/NandanaMD/CrushEaseLedger/releases

---

## That's It! 🎉

From now on, just run `.\create-release.ps1` whenever you want to release a new version.

Everything else is automatic:
- ✅ Build
- ✅ Installer creation
- ✅ GitHub release
- ✅ User notifications

No manual work required!
