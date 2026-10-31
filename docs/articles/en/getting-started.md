# Getting started

[Русский](../ru/getting-started.md)

Use the YandexTracker package with .NET 8, 9 or 10. The first planned release is 0.1.0;
before it is published, use the [local package instructions](releasing.md).

~~~sh
dotnet add package YandexTracker --version 0.1.0
~~~

~~~csharp
using YandexTracker;
using YandexTracker.Models.Issues;

using var client = new YandexTrackerClient(new YandexTrackerClientOptions
{
    Token = Environment.GetEnvironmentVariable("YANDEX_TRACKER_TOKEN")!,
    OrganizationId = Environment.GetEnvironmentVariable("YANDEX_TRACKER_ORG_ID")!
});
var me = await client.MyselfAsync();
Console.WriteLine(me.Display);
using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
await foreach (var issue in client.Issues.FindAsync(
    new IssueSearch { Queue = "TEST", Limit = 20 }, cancellation.Token))
    Console.WriteLine($"{issue.Key}: {issue.Summary}");
~~~

Without an HttpClient argument the SDK creates and disposes one. You can instead pass a long-lived
or factory-managed HttpClient as the first constructor argument; disposing the SDK then leaves it alive.
Reuse clients rather than creating one per issue. The SDK has no dependency injection package.
[Authentication](authentication.md) explains token and organization choices.
