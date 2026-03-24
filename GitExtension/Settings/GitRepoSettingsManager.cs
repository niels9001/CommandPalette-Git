// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GitExtension.Settings;

public sealed class GitRepoSettingsManager : JsonSettingsManager
{
    private static readonly string Namespace = "gitRepos";

    private static string Namespaced(string propertyName) => $"{Namespace}.{propertyName}";

    private readonly TextSetting _scanPaths = new(
        Namespaced("scanPaths"),
        "Scan Paths",
        "Semicolon-separated list of root directories to scan for git repositories (e.g. D:\\Projects;C:\\Users\\me\\repos)",
        string.Empty);

    private static readonly List<ChoiceSetSetting.Choice> DepthChoices =
    [
        new("2", "2"),
        new("3", "3"),
        new("4", "4"),
        new("5", "5"),
        new("6", "6"),
    ];

    private readonly ChoiceSetSetting _maxDepth = new(
        Namespaced("maxDepth"),
        "Max Scan Depth",
        "Maximum directory depth to search for .git folders",
        DepthChoices)
    {
        Value = "3",
    };

    private readonly TextSetting _terminalCommand = new(
        Namespaced("terminalCommand"),
        "Terminal Startup Command",
        "Command to run automatically when opening a repository in terminal (e.g. git status, npm install)",
        string.Empty);

    public string ScanPaths => _scanPaths.Value ?? string.Empty;

    public int MaxDepth => int.TryParse(_maxDepth.Value, out var depth) ? depth : 3;

    public string TerminalCommand => _terminalCommand.Value ?? string.Empty;

    /// <summary>
    /// Programmatically updates the scan paths setting and persists to disk.
    /// We can't use Settings.RaiseSettingsChanged() (it's internal to the toolkit),
    /// so we save directly and fire our own event for listeners.
    /// </summary>
    public event EventHandler? ScanPathsUpdated;

    public void UpdateScanPaths(string paths)
    {
        var key = Namespaced("scanPaths");
        var json = $"{{\"{key}\":\"{paths.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"}}";
        Settings.Update(json);
        SaveSettings();
        ScanPathsUpdated?.Invoke(this, EventArgs.Empty);
    }

    internal static string SettingsJsonPath()
    {
        var directory = Utilities.BaseSettingsPath("Microsoft.CmdPal");
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, "GitExtension.settings.json");
    }

    public GitRepoSettingsManager()
    {
        FilePath = SettingsJsonPath();

        Settings.Add(_scanPaths);
        Settings.Add(_maxDepth);
        Settings.Add(_terminalCommand);

        LoadSettings();

        Settings.SettingsChanged += (s, a) => SaveSettings();
    }
}
