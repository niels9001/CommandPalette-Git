// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace GitExtension.Models;

/// <summary>
/// Represents a discovered git repository on the local filesystem.
/// </summary>
public sealed class GitRepoInfo
{
    public GitRepoInfo(string name, string fullPath)
    {
        Name = name;
        FullPath = fullPath;
    }

    /// <summary>The display name of the repository (folder name).</summary>
    public string Name { get; }

    /// <summary>The full absolute path to the repository root directory.</summary>
    public string FullPath { get; }
}
