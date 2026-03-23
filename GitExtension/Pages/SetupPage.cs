// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using GitExtension.Settings;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GitExtension.Pages;

internal sealed partial class SetupPage : ContentPage
{
    private readonly ScanPathsSetupForm _form;

    public SetupPage(GitRepoSettingsManager settingsManager)
    {
        _form = new ScanPathsSetupForm(settingsManager);
        Name = "Open";
        Title = "Configure scan paths";
        Icon = new IconInfo("\uE713"); // Settings icon
    }

    public override IContent[] GetContent() => [_form];
}
