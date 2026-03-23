// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using GitExtension.Models;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GitExtension.Commands;

/// <summary>
/// Opens the repository folder in Windows Explorer.
/// </summary>
internal sealed partial class OpenFolderCommand : InvokableCommand
{
    private readonly GitRepoInfo _repo;

    public OpenFolderCommand(GitRepoInfo repo)
    {
        _repo = repo;
        Name = "Open folder";
        Icon = new IconInfo("\uE838"); // FolderOpen icon
    }

    public override CommandResult Invoke()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "explorer.exe",
            Arguments = _repo.FullPath,
            UseShellExecute = true,
        });

        return CommandResult.Dismiss();
    }
}
