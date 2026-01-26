# GitHub Pages Setup Instructions

This directory contains the GitHub Pages site for hosting the CrushEase Ledger installer download page.

## Enabling GitHub Pages

To enable GitHub Pages for this repository:

1. Go to your repository on GitHub
2. Click on **Settings**
3. Scroll down to **Pages** in the left sidebar
4. Under **Source**, select:
   - **Source:** Deploy from a branch
   - **Branch:** `main` (or your default branch)
   - **Folder:** `/docs`
5. Click **Save**

GitHub will automatically build and deploy your site. It will be available at:
```
https://nandanamd.github.io/CrushEaseLedger/
```

## Updating the Download Link

The download link in `index.html` currently points to:
```
https://github.com/NandanaMD/CrushEaseLedger/releases/latest/download/CrushEaseLedger-Setup-v1.0.0.exe
```

### To Create a Release with the Installer:

1. Build the installer following the instructions in `BUILD_INSTALLER.md`
2. Go to your repository on GitHub
3. Click **Releases** → **Create a new release**
4. Choose a tag (e.g., `v1.0.0`)
5. Upload the installer exe file: `CrushEaseLedger-Setup-v1.0.0.exe`
6. Publish the release

The download link will automatically work once the release is published with the installer attached.

## Local Testing

To test the page locally, simply open `index.html` in a web browser:
```bash
# From the repository root
open docs/index.html
# or
start docs/index.html  # On Windows
```

## Customization

Edit `index.html` to customize:
- Version number
- Features list
- System requirements
- Styling and colors
- Download links
