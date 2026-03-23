// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Nodes;
using GitExtension.Settings;
using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;

namespace GitExtension.Pages;

internal sealed partial class ScanPathsSetupForm : FormContent
{
    private readonly GitRepoSettingsManager _settingsManager;

    public ScanPathsSetupForm(GitRepoSettingsManager settingsManager)
    {
        _settingsManager = settingsManager;

        TemplateJson = $$"""
{
    "$schema": "http://adaptivecards.io/schemas/adaptive-card.json",
    "type": "AdaptiveCard",
    "version": "1.6",
    "body": [
        {
            "type": "TextBlock",
            "size": "medium",
            "weight": "bolder",
            "text": "Configure scan paths",
            "wrap": true,
            "style": "heading"
        },
        {
            "type": "TextBlock",
            "text": "Enter the root directories to scan for git repositories. Separate multiple paths with a semicolon (;).",
            "wrap": true
        },
        {
            "type": "Input.Text",
            "label": "Scan paths",
            "style": "text",
            "id": "scanPaths",
            "isRequired": true,
            "errorMessage": "At least one scan path is required",
            "placeholder": "D:\\Projects;C:\\Users\\me\\repos"
        }
    ],
    "actions": [
        {
            "type": "Action.Submit",
            "title": "Save",
            "data": {}
        }
    ]
}
""";
    }

    public override CommandResult SubmitForm(string payload)
    {
        var formInput = JsonNode.Parse(payload)?.AsObject();
        if (formInput == null)
        {
            return CommandResult.GoBack();
        }

        var scanPaths = formInput["scanPaths"]?.ToString();
        if (!string.IsNullOrWhiteSpace(scanPaths))
        {
            // Update the setting value and trigger save via SettingsChanged
            _settingsManager.UpdateScanPaths(scanPaths);
        }

        return CommandResult.GoBack();
    }
}
