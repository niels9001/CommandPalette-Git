// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GitExtension.Models;

namespace GitExtension.Services;

/// <summary>
/// Scans configured directories for git repositories.
/// </summary>
public static class GitRepoScanner
{
    /// <summary>
    /// Scans the given semicolon-separated root paths for git repositories
    /// up to the specified maximum depth.
    /// </summary>
    public static List<GitRepoInfo> Scan(string scanPaths, int maxDepth)
    {
        var roots = ParseScanPaths(scanPaths);
        var repos = new List<GitRepoInfo>();

        foreach (var root in roots)
        {
            ScanDirectory(root, maxDepth, 0, repos);
        }

        repos.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
        return repos;
    }

    private static string[] ParseScanPaths(string scanPaths)
    {
        return scanPaths
            .Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(ResolvePath)
            .Where(Directory.Exists)
            .ToArray();
    }

    private static string ResolvePath(string path)
    {
        // Expand ~ to user profile directory
        if (path.StartsWith('~'))
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            path = Path.Combine(home, path[1..].TrimStart('\\', '/'));
        }

        // Expand environment variables like %USERPROFILE%
        path = Environment.ExpandEnvironmentVariables(path);

        return Path.GetFullPath(path);
    }

    private static void ScanDirectory(string directory, int maxDepth, int currentDepth, List<GitRepoInfo> results)
    {
        if (currentDepth > maxDepth)
        {
            return;
        }

        try
        {
            var gitDir = Path.Combine(directory, ".git");
            if (Directory.Exists(gitDir) || File.Exists(gitDir))
            {
                var name = Path.GetFileName(directory) ?? directory;
                results.Add(new GitRepoInfo(name, directory));
                // Don't recurse into a repo's subdirectories (avoids submodules/nested repos)
                return;
            }

            foreach (var subDir in Directory.EnumerateDirectories(directory))
            {
                // Skip hidden, system, and special directories for performance
                var dirName = Path.GetFileName(subDir);
                if (dirName.StartsWith('.') ||
                    dirName.StartsWith('$') ||
                    dirName.Equals("node_modules", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                ScanDirectory(subDir, maxDepth, currentDepth + 1, results);
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Skip directories we can't access
        }
        catch (IOException)
        {
            // Skip directories with I/O errors (e.g., broken symlinks)
        }
    }
}
