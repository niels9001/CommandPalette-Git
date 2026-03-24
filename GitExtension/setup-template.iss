; Inno Setup Script for Git Repositories Command Palette Extension

#define AppVersion "0.0.1.0"

[Setup]
AppId={{80839ccf-b666-4d90-97c6-c8e2a2325332}}
AppName=Git Repositories
AppVersion={#AppVersion}
AppPublisher=Niels Laute
DefaultDirName={autopf}\GitExtension
OutputDir=bin\Release\installer
OutputBaseFilename=GitExtension-Setup-{#AppVersion}
Compression=lzma
SolidCompression=yes
MinVersion=10.0.19041

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "bin\Release\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\Git Repositories"; Filename: "{app}\GitExtension.exe"

[Registry]
Root: HKCU; Subkey: "SOFTWARE\Classes\CLSID\{{80839ccf-b666-4d90-97c6-c8e2a2325332}}"; ValueData: "GitExtension"
Root: HKCU; Subkey: "SOFTWARE\Classes\CLSID\{{80839ccf-b666-4d90-97c6-c8e2a2325332}}\LocalServer32"; ValueData: "{app}\GitExtension.exe -RegisterProcessAsComServer"
