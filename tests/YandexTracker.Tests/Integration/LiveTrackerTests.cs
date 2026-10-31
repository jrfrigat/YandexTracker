using Xunit;
using YandexTracker.Models.Issues;

namespace YandexTracker.Tests.Integration;

/// <summary>
/// Read-only checks against the live Yandex Tracker API: they never create or change anything in the organization.
/// Skipped unless <see cref="IntegrationFactAttribute.TokenVariable"/> and
/// <see cref="IntegrationFactAttribute.OrganizationVariable"/> are set.
/// </summary>
public sealed class LiveTrackerTests
{
    private static YandexTrackerClient Client() => new(new YandexTrackerClientOptions
    {
        Token = IntegrationFactAttribute.Setting(IntegrationFactAttribute.TokenVariable)!,
        OrganizationId = IntegrationFactAttribute.Setting(IntegrationFactAttribute.OrganizationVariable)!,
        OrganizationKind = string.Equals(IntegrationFactAttribute.Setting(IntegrationFactAttribute.OrganizationKindVariable), "cloud", StringComparison.OrdinalIgnoreCase)
            ? YandexTrackerOrganizationKind.Cloud
            : YandexTrackerOrganizationKind.Yandex360,
    });

    [IntegrationFact]
    public async Task Myself_names_the_token_owner()
    {
        using var client = Client();

        var me = await client.MyselfAsync();

        Assert.False(string.IsNullOrWhiteSpace(me.Login ?? me.Display));
    }

    [IntegrationFact]
    public async Task Statuses_are_listed()
    {
        using var client = Client();

        var statuses = await client.Statuses.GetAllAsync();

        Assert.NotEmpty(statuses);
        Assert.All(statuses, s => Assert.False(string.IsNullOrWhiteSpace(s.Key)));
    }

    [IntegrationFact]
    public async Task A_queue_search_reads_issues_with_their_dates()
    {
        using var client = Client();
        var queue = IntegrationFactAttribute.Setting(IntegrationFactAttribute.QueueVariable)
            ?? (await client.Queues.GetAllAsync().ToListAsync()).Select(q => q.Key).FirstOrDefault();
        if (queue is null) return; // an organization without queues has nothing to search

        var issues = await client.Issues.FindAsync(new IssueSearch { Queue = queue, PageSize = 5, Limit = 5 }).ToListAsync();

        Assert.All(issues, issue =>
        {
            Assert.StartsWith(queue + "-", issue.Key, StringComparison.Ordinal);
            Assert.NotNull(issue.UpdatedAt);
        });
    }

    [IntegrationFact]
    public async Task A_missing_issue_is_a_typed_not_found()
    {
        using var client = Client();

        await Assert.ThrowsAsync<YandexTrackerNotFoundException>(() => client.Issues.GetAsync("NOSUCHQUEUE-999999"));
    }
}
