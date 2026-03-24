# Git Repos — Command Palette Extension

A [Command Palette](https://learn.microsoft.com/windows/powertoys/command-palette/) extension that discovers your local git repositories and gives you quick access to open them in Explorer, VS Code, or Terminal.

![Screenshot](https://img.shields.io/badge/Platform-Windows-blue)
![License](https://img.shields.io/badge/License-MIT-green)

## Features

- **Scan & discover** — Recursively finds git repositories under your configured root directories
- **Open in Explorer** — Default action opens the repo folder in Windows Explorer
- **Open in VS Code** — Launch VS Code directly at the repository path
- **Open in Terminal** — Opens Windows Terminal (or cmd fallback) at the repo directory
- **Terminal startup command** — Optionally run a command automatically when opening in terminal
- **Copy path** — Copy the full repository path to your clipboard
- **Search & filter** — Real-time filtering by repo name or path
- **Dock pinning** — Pin your favorite repos to the Command Palette Dock for instant access
- **Theme-aware icons** — Git icons that adapt to light and dark themes
- **Inline setup** — Configure scan paths directly from the extension, no need to dig into settings

## Getting started

### Install from the Microsoft Store

*(Coming soon)*

### Build from source

**Prerequisites:**
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PowerToys](https://github.com/microsoft/PowerToys) with Command Palette enabled
- Visual Studio 2022 (17.13+) or `dotnet` CLI

**Build & deploy:**

```powershell
git clone https://github.com/niels9001/CommandPalette-Git.git
cd CommandPalette-Git
dotnet build GitExtension.sln -c Debug
```

Then deploy the MSIX package from Visual Studio, or use `dotnet publish` with the appropriate profile.

## Configuration

When you first open the extension, you'll be prompted to configure your scan paths.

| Setting | Description | Default |
|---------|-------------|---------|
| **Scan paths** | Semicolon-separated root directories (e.g. `D:\Projects;C:\repos`) | *(empty)* |
| **Max scan depth** | How deep to recurse into directories (2–6) | 3 |
| **Terminal startup command** | Command to auto-run when opening a repo in terminal (e.g. `git status`) | *(empty)* |

You can change these anytime via the "Change scan paths" item at the bottom of the repo list, or through the extension's settings page.

## Actions per repository

| Action | Trigger | Description |
|--------|---------|-------------|
| Open folder | `Enter` (default) | Opens in Windows Explorer |
| Open in VS Code | Context menu | Runs `code <path>` |
| Open in terminal | Context menu | Runs `wt.exe -d <path>` |
| Copy path | Context menu | Copies full path to clipboard |

## Contributing

Contributions are welcome! Please open an issue or submit a pull request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

## Privacy

This extension operates entirely offline and does not collect any data. See [PRIVACY.md](PRIVACY.md) for details.
