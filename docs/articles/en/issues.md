# Issues and search

[Русский](../ru/issues.md)

Choose exactly one IssueSearch condition: Query, Filter, Keys or Queue. Order is supported with Filter only.
PageSize is 1-100; Limit bounds the total number of returned issues. Enumeration requests pages lazily.
This is ordinary page-based iteration, not a snapshot or an export/scroll API; changes during a scan
can change the result set.

~~~csharp
using YandexTracker.Models.Issues;

var search = new IssueSearch
{
    Filter = new Dictionary<string, object?> { ["queue"] = "TEST", ["status"] = "open" },
    Order = "+updatedAt",
    PageSize = 50,
    Limit = 200
};
await foreach (var issue in client.Issues.FindAsync(search, cancellationToken))
{
    Console.WriteLine($"{issue.Key}: {issue.Summary}");
    if (issue.Fields?.TryGetValue("customField", out var value) == true)
        Console.WriteLine(value);
}
~~~

~~~csharp
var created = await client.Issues.CreateAsync(new IssueDraft
{
    Queue = "TEST",
    Summary = "Review import",
    Unique = Guid.NewGuid().ToString(),
    OriginalEstimation = YandexTrackerDuration.FromHours(10)
});
var updated = await client.Issues.UpdateAsync(created.Key,
    new Dictionary<string, object?> { ["summary"] = "Review SDK import" }, created.Version);
var transitions = await client.Issues.GetTransitionsAsync(updated.Key);
var transition = transitions.FirstOrDefault(t => t.To?.Key == "inProgress");
if (transition is not null)
    await client.Issues.ExecuteTransitionAsync(updated.Key, transition.Id, "Started");
await client.Issues.AddCommentAsync(updated.Key, "SDK integration checked");
var links = await client.Issues.GetLinksAsync(updated.Key);
~~~

Reuse a draft's Unique value when explicitly retrying creation. Version checks reject stale updates.
Get available transitions rather than writing a status field directly. Issue.Fields preserves unmodeled
JSON properties; inspect JsonElement values according to the queue's field schema.

Dates without time use DateOnly; timestamp fields use DateTimeOffset. Duration conversion assumes an
8-hour day and a 5-day week; it is a conversion helper, not a work calendar or a scheduling engine.
