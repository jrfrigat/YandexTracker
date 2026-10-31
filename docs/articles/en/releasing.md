# Releasing

[Русский](../ru/releasing.md)

## One-time GitHub and NuGet setup

1. Create the jrfrigat/YandexTracker repository on GitHub without generated initial files.
   Add its URL as origin in the local repository. Review and push the working branch yourself.
2. Set the repository Actions variable NUGET_USER to your nuget.org username.
3. Create a GitHub environment named nuget. On nuget.org configure a Trusted Publisher for the
   package, owner jrfrigat, repository YandexTracker, workflow release.yml and environment nuget.
   Follow the current [NuGet Trusted Publishing guide](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing).
   There is no long-lived NuGet API key in the repository.
4. Set Pages -> Build and deployment -> Source to GitHub Actions; allow the github-pages environment
   to deploy main. Enable private vulnerability reporting if desired.
5. Enable required CI checks/branch protection. CodeQL availability depends on repository/account settings.

## Release

Run the checks in CONTRIBUTING.md. Update both changelogs for the version.
MinVer reads full git history and tags with prefix v; an untagged commit is a preview.
After the intended commit is on main, create and push a v0.1.0 tag yourself.
The release workflow builds/tests every TFM, packs, obtains a short-lived NuGet token through OIDC,
pushes the package (and its symbol package), and creates a GitHub release with both artifacts.
Tags with a prerelease suffix create a GitHub prerelease without making it latest.
Publication and GitHub workflow execution have not been performed by preparing these files.

## Local package before publication

PowerShell, from this repository:

~~~powershell
$feed = Join-Path $env:TEMP 'yandextracker-local-feed'
dotnet pack src/YandexTracker/YandexTracker.csproj -c Release -p:MinVerVersionOverride=0.1.0 -o $feed
~~~

Use a fresh temporary packages directory with --packages during restore when validating a changed
local package with the same version, then pass the same directory as RestorePackagesPath to builds/tests.
Otherwise NuGet may reuse a cached 0.1.0. Never replace an already published stable version.

To use the package in another project, add a PackageReference to YandexTracker version 0.1.0.
Restore that project with the local feed; replace the example path with your project file:

~~~powershell
dotnet restore ../Consumer/Consumer.csproj --source $feed --source https://api.nuget.org/v3/index.json
~~~

A clean machine or CI needs the published package or access to the local feed.
Do not commit machine-specific feed paths or package binaries.
