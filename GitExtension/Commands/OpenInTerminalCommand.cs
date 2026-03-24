// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using GitExtension.Models;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GitExtension.Commands;

/// <summary>
/// Opens Windows Terminal (or cmd.exe as fallback) at the repository path.
/// </summary>
internal sealed partial class OpenInTerminalCommand : InvokableCommand
{
    private readonly GitRepoInfo _repo;
    private readonly string _startupCommand;

    public OpenInTerminalCommand(GitRepoInfo repo, string startupCommand = "")
    {
        _repo = repo;
        _startupCommand = startupCommand;
        Name = "Open in terminal";
        Icon = new IconInfo("\uE756"); // CommandPrompt icon
    }

    public override CommandResult Invoke()
    {
        if (!string.IsNullOrWhiteSpace(_startupCommand))
        {
            // Open terminal in the repo directory and run the startup command
            ShellHelpers.OpenInShell("wt.exe", $"-d \"{_repo.FullPath}\" cmd /k \"{_startupCommand}\"", _repo.FullPath);
        }
        else
        {
            ShellHelpers.OpenInShell("wt.exe", $"-d \"{_repo.FullPath}\"", _repo.FullPath);
        }

        return CommandResult.Dismiss();
    }
}
