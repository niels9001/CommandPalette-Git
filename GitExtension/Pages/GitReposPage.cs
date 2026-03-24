// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GitExtension.Commands;
using GitExtension.Models;
using GitExtension.Services;
using GitExtension.Settings;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GitExtension.Pages;

internal sealed partial class GitReposPage : DynamicListPage
{
    private static readonly IconInfo GitIcon =
        IconHelpers.FromRelativePaths("Assets\\git.light.svg", "Assets\\git.dark.svg");

    private readonly GitRepoSettingsManager _settingsManager;
    private readonly SetupPage _setupPage;
    private List<GitRepoInfo> _repos = [];
    private int _isScanning;

    // Cache of command items for dock pinning lookup
    private readonly Dictionary<string, ICommandItem> _commandItemCache = [];

    public GitReposPage(GitRepoSettingsManager settingsManager)
    {
        _settingsManager = settingsManager;
        _setupPage = new SetupPage(settingsManager);

        Icon = GitIcon;
        Title = "Browse git repositories";
        Name = "Open";
        PlaceholderText = "Search repositories...";

        _settingsManager.Settings.SettingsChanged += OnSettingsChanged;
        _settingsManager.ScanPathsUpdated += (s, e) => OnSettingsChanged(s, _settingsManager.Settings);
        RefreshRepos();
    }

    private void OnSettingsChanged(object? sender, Microsoft.CommandPalette.Extensions.Toolkit.Settings settingsArgs)
    {
        RefreshRepos();
        RaiseItemsChanged();
    }

    private void RefreshRepos()
    {
        if (string.IsNullOrWhiteSpace(_settingsManager.ScanPaths))
        {
            _repos = [];
            return;
        }

        _repos = GitRepoScanner.Scan(_settingsManager.ScanPaths, _settingsManager.MaxDepth);
    }

    private void RefreshReposInBackground()
    {
        if (string.IsNullOrWhiteSpace(_settingsManager.ScanPaths))
        {
            return;
        }

        // Skip if a scan is already running
        if (Interlocked.CompareExchange(ref _isScanning, 1, 0) != 0)
        {
            return;
        }

        IsLoading = true;
        Task.Run(() =>
        {
            try
            {
                var newRepos = GitRepoScanner.Scan(_settingsManager.ScanPaths, _settingsManager.MaxDepth);
                var changed = !_repos.Select(r => r.FullPath).SequenceEqual(newRepos.Select(r => r.FullPath));
                if (changed)
                {
                    _repos = newRepos;
                    RaiseItemsChanged();
                }
            }
            finally
            {
                IsLoading = false;
                Interlocked.Exchange(ref _isScanning, 0);
            }
        });
    }

    public override void UpdateSearchText(string oldSearch, string newSearch)
    {
        RaiseItemsChanged();
    }

    /// <summary>
    /// Resolves a command item by ID for dock pinning support.
    /// </summary>
    internal ICommandItem? GetCommandItemById(string id)
    {
        return _commandItemCache.TryGetValue(id, out var item) ? item : null;
    }

    private static string RepoCommandId(GitRepoInfo repo) => $"repo.{repo.FullPath}";

    public override IListItem[] GetItems()
    {
        // Trigger a background rescan each time the page is shown
        RefreshReposInBackground();

        if (_repos.Count == 0)
        {
            return
            [
                new ListItem(new NoOpCommand())
                {
                    Title = "No git repositories found",
                    Subtitle = "Check your scan paths and depth in settings",
                },
                new ListItem(_setupPage)
                {
                    Title = "Change scan paths",
                    Icon = new IconInfo("\uE713"),
                },
            ];
        }

        var filtered = FilterRepos();
        _commandItemCache.Clear();

        var items = filtered.Select(repo =>
        {
            var repoId = RepoCommandId(repo);
            var openFolder = new OpenFolderCommand(repo) { Id = repoId };
            var openTerminal = new OpenInTerminalCommand(repo, _settingsManager.TerminalCommand);
            var openVSCode = new OpenInVSCodeCommand(repo);
            var copyPath = new CopyRepoPathCommand(repo);

            var listItem = new ListItem(openFolder)
            {
                Title = repo.Name,
                Subtitle = repo.FullPath,
                Icon = GitIcon,
                MoreCommands =
                [
                    new CommandContextItem(openTerminal),
                    new CommandContextItem(openVSCode),
                    new CommandContextItem(copyPath),
                    new CommandContextItem(_settingsManager.Settings.SettingsPage) { Title = "Settings", Icon = new IconInfo("\uE713") },
                ],
            };

            // Register for dock pinning
            var commandItem = new CommandItem(openFolder)
            {
                Title = repo.Name,
                Subtitle = repo.FullPath,
                Icon = GitIcon,
            };
            _commandItemCache[repoId] = commandItem;

            return listItem;
        }).ToList();

        return items.ToArray();
    }

    private List<GitRepoInfo> FilterRepos()
    {
        var search = SearchText;
        if (string.IsNullOrWhiteSpace(search))
        {
            return _repos;
        }

        return _repos
            .Where(r =>
                r.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                r.FullPath.Contains(search, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
