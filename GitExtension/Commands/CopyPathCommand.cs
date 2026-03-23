// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using GitExtension.Models;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GitExtension.Commands;

/// <summary>
/// Copies the repository path to the clipboard.
/// </summary>
internal sealed partial class CopyRepoPathCommand : InvokableCommand
{
    private readonly GitRepoInfo _repo;

    public CopyRepoPathCommand(GitRepoInfo repo)
    {
        _repo = repo;
        Name = "Copy path";
        Icon = new IconInfo("\uE8C8"); // Copy icon
    }

    public override CommandResult Invoke()
    {
        ClipboardHelper.SetText(_repo.FullPath);
        return CommandResult.ShowToast($"Copied: {_repo.FullPath}");
    }
}
