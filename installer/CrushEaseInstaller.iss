; CrushEase Ledger Installer Script
; Created with Inno Setup

#define MyAppName "CrushEase Ledger"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "CrushEase Development"
#define MyAppExeName "CrushEase.exe"
#define MyAppAssocName "CrushEase Database"
#define MyAppAssocExt ".db"

[Setup]
AppId={{8E9F5A2D-1B3C-4D5E-9F7A-2B4C6D8E0F1A}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\CrushEase Ledger
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
LicenseFile=..\LICENSE.txt
OutputDir=Output
OutputBaseFilename=CrushEaseLedger-Setup-v{#MyAppVersion}
SetupIconFile=..\CrushEase\assets\mainlogo.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin
ArchitecturesInstallIn64BitMode=x64

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\CrushEase\bin\Release\net8.0-windows\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{group}\{cm:UninstallProgram,{#MyAppName}}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
// No .NET check needed - self-contained deployment includes the runtime
