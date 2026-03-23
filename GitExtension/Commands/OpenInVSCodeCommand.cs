// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using GitExtension.Models;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GitExtension.Commands;

/// <summary>
/// Opens the repository in Visual Studio Code.
/// </summary>
internal sealed partial class OpenInVSCodeCommand : InvokableCommand
{
    private readonly GitRepoInfo _repo;

    public OpenInVSCodeCommand(GitRepoInfo repo)
    {
        _repo = repo;
        Name = "Open in VS Code";
        Icon = new IconInfo("\uE943"); // Code icon
    }

    public override CommandResult Invoke()
    {
        ShellHelpers.OpenInShell("code", $"\"{_repo.FullPath}\"", _repo.FullPath);
        return CommandResult.Dismiss();
    }
}
