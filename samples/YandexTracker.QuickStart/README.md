# Read-only quick start

[Русский](README.ru.md)

Requires the SDK selected by `global.json` to build, and .NET 8 to run. This sample uses the local project,
reads the current user and optionally up to ten issues. It never creates or changes Tracker data.

From the repository root, using Bash:

```bash
dotnet run --project samples/YandexTracker.QuickStart -- --help
read -rsp 'Tracker access token: ' YANDEX_TRACKER_TOKEN
export YANDEX_TRACKER_TOKEN
export YANDEX_TRACKER_ORG_ID='<organization-id>'
export YANDEX_TRACKER_QUEUE='TEST' # optional; omit to read only myself
dotnet run --project samples/YandexTracker.QuickStart
unset YANDEX_TRACKER_TOKEN
```

On Windows, set the same variables in your shell (for example `$env:YANDEX_TRACKER_ORG_ID` in PowerShell)
and run the same `dotnet` commands.

Defaults are OAuth and Yandex 360. For IAM/Cloud, also set `YANDEX_TRACKER_TOKEN_TYPE=iam` and
`YANDEX_TRACKER_ORG_KIND=cloud`. The sample reads environment variables only, not `.env`.
Ctrl+C or a 30-second deadline cancels requests. Exit codes: 0 success/help, 1 request failure or
cancellation, 2 configuration/usage error. Output includes user and issue data; handle it accordingly.

`dotnet build -c Release` at the repository root compiles this sample without connecting it to Tracker. See the
[getting started guide](../../docs/articles/en/getting-started.md) for package-based consumption.
