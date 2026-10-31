# YandexTracker

Unofficial asynchronous .NET client for the [Yandex Tracker API](https://yandex.ru/support/tracker/ru/api/about-api)
(v2). Runs on **.NET 8, .NET 9 and .NET 10** and depends on nothing but the BCL.

> Unofficial project, not affiliated with Yandex.

```csharp
using YandexTracker;
using YandexTracker.Models.Issues;

using var client = new YandexTrackerClient(new YandexTrackerClientOptions
{
    Token = Environment.GetEnvironmentVariable("YANDEX_TRACKER_TOKEN")!,
    OrganizationId = Environment.GetEnvironmentVariable("YANDEX_TRACKER_ORG_ID")!,
});

var me = await client.MyselfAsync();
var issue = await client.Issues.GetAsync("TEST-1");
var hours = YandexTrackerDuration.ToHours(issue.OriginalEstimation);   // P1W2D → 56 working hours

await foreach (var found in client.Issues.FindAsync(new IssueSearch { Query = "Queue: TEST Resolution: empty()" }))
    Console.WriteLine($"{found.Key}: {found.Summary}");
```

- OAuth or IAM tokens; Yandex 360 (`X-Org-ID`) or Yandex Cloud (`X-Cloud-Org-ID`) organizations.
- Issues: get, paged search, create (idempotent), update with version check, links, transitions, comments;
  queue-local fields in `Issue.Fields`.
- Queues, users, statuses, `myself`.
- Retries on 429 (any request) and on 5xx for reads; `Retry-After` is honoured.
- Typed exceptions under `YandexTrackerException`.

Documentation: <https://jrfrigat.github.io/YandexTracker/> · Source: <https://github.com/jrfrigat/YandexTracker>
