# CrushEase Business Manager

A simple, offline business management software for small transport/material businesses.

## Features

- **Master Management**: Vehicles, Vendors, Buyers, Materials
- **Transactions**: Sales, Purchases, Maintenance
- **Dashboard**: Daily totals and monthly net profit
- **Excel Reports**: Sales, Purchase, Maintenance, Vehicle Profit Summary
- **Automatic Backups**: Daily and weekly backups with restore functionality

## System Requirements

- Windows 10 or later
- .NET 8.0 Runtime (included in installer)
- 100MB free disk space

## Technical Stack

- **Framework**: .NET 8.0 Windows Forms
- **Database**: SQLite with WAL mode
- **Excel Generation**: EPPlus
- **Logging**: Serilog

## Build Instructions

1. Open `CrushEase.sln` in Visual Studio 2022
2. Restore NuGet packages
3. Build solution (F6)
4. Run (F5)

## Project Structure

```
CrushEase/
├── Data/              # Database and data access layer
├── Models/            # Entity models
├── Forms/             # UI forms
├── Services/          # Business logic and report generation
├── Utils/             # Helpers and utilities
└── Program.cs         # Application entry point
```

## License

Proprietary - All rights reserved
