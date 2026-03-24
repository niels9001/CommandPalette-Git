// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using GitExtension.Pages;
using GitExtension.Settings;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GitExtension;

public partial class GitExtensionCommandsProvider : CommandProvider
{
    private static readonly IconInfo ExtensionIcon =
        IconHelpers.FromRelativePath("Assets\\StoreLogo.scale-200.png");

    private readonly GitRepoSettingsManager _settingsManager = new();
    private readonly GitReposPage _reposPage;
    private readonly SetupPage _setupPage;
    private ICommandItem[] _commands;

    public GitExtensionCommandsProvider()
    {
        DisplayName = "Browse git repositories";
        Icon = ExtensionIcon;
        Id = "com.gitextension.repos";
        Settings = _settingsManager.Settings;

        _reposPage = new GitReposPage(_settingsManager);
        _setupPage = new SetupPage(_settingsManager);

        _commands = BuildCommands();
        _settingsManager.Settings.SettingsChanged += OnSettingsChanged;
        _settingsManager.ScanPathsUpdated += (s, e) => OnSettingsChanged(s, _settingsManager.Settings);

        // Signal readiness after construction so CmdPal re-resolves
        // any saved dock pins that may have been queried too early.
        _ = Task.Run(async () =>
        {
            await Task.Delay(500);
            RaiseItemsChanged();
        });
    }

    private void OnSettingsChanged(object? sender, Microsoft.CommandPalette.Extensions.Toolkit.Settings args)
    {
        _commands = BuildCommands();
        RaiseItemsChanged();
    }

    private ICommandItem[] BuildCommands()
    {
        if (string.IsNullOrWhiteSpace(_settingsManager.ScanPaths))
        {
            return
            [
                new CommandItem(_setupPage) { Title = DisplayName, Icon = ExtensionIcon },
            ];
        }

        return
        [
            new CommandItem(_reposPage) { Title = DisplayName, Icon = ExtensionIcon },
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
