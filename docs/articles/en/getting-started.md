# Getting started

[Русский](../ru/getting-started.md)

Use the YandexTracker package with .NET 8, 9 or 10. Choose a published version on
[NuGet](https://www.nuget.org/packages/YandexTracker).

~~~sh
dotnet add package YandexTracker
~~~

~~~csharp
using YandexTracker;
using YandexTracker.Models.Issues;

using var client = new YandexTrackerClient(new YandexTrackerClientOptions
{
    Token = Environment.GetEnvironmentVariable("YANDEX_TRACKER_TOKEN")!,
    OrganizationId = Environment.GetEnvironmentVariable("YANDEX_TRACKER_ORG_ID")!
});
using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
var me = await client.MyselfAsync(cancellation.Token);
Console.WriteLine(me.Display);
await foreach (var issue in client.Issues.FindAsync(
    new IssueSearch { Queue = "TEST", Limit = 20 }, cancellation.Token))
    Console.WriteLine($"{issue.Key}: {issue.Summary}");
~~~

Without an HttpClient argument the SDK creates and disposes one. You can instead pass a long-lived
or factory-managed HttpClient as the first constructor argument; disposing the SDK then leaves it alive.
Reuse clients rather than creating one per issue. The SDK has no dependency injection package.
[Authentication](authentication.md) explains token and organization choices.

The repository also includes `samples/YandexTracker.QuickStart`, a complete read-only program with
environment validation, Ctrl+C cancellation and error handling. Run
`dotnet run --project samples/YandexTracker.QuickStart -- --help` from the repository root.

## Compatibility and limits

The package has no runtime NuGet dependencies. Models use System.Text.Json; unknown issue properties
are preserved in Issue.Fields (see [Issues and search](issues.md)). Native AOT and trimming have not
been validated; serialization uses reflection.

Attachments, boards, bulk operations, exports and token acquisition are not implemented. The supported
endpoints are listed in the [README](https://github.com/jrfrigat/YandexTracker/blob/main/README.md#features). This is an independent SDK; service behavior
is described in the [official API reference](https://yandex.ru/support/tracker/ru/api/about-api).
