// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using GitExtension.Pages;
using GitExtension.Settings;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GitExtension;

public partial class GitExtensionCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;
    private readonly GitRepoSettingsManager _settingsManager = new();
    private readonly GitReposPage _reposPage;

    public GitExtensionCommandsProvider()
    {
        DisplayName = "Git repositories";
        Icon = IconHelpers.FromRelativePaths("Assets\\git.dark.svg", "Assets\\git.light.svg");
        Id = "com.gitextension.repos";
        Settings = _settingsManager.Settings;

        _reposPage = new GitReposPage(_settingsManager);
        _commands =
        [
            new CommandItem(_reposPage) { Title = DisplayName },
        ];
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

    public override ICommandItem? GetCommandItem(string id)
    {
        return _reposPage.GetCommandItemById(id);
    }
}
