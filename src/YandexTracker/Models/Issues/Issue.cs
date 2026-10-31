using System.Text.Json;
using System.Text.Json.Serialization;

namespace YandexTracker.Models.Issues;

/// <summary>
/// A Tracker issue. Standard fields are typed; queue-local fields and anything the API adds later stay in
/// <see cref="Fields"/>, like the attributes of an issue object in the official Python SDK.
/// </summary>
public sealed record Issue
{
    /// <summary>The issue's internal id.</summary>
    [JsonPropertyName("id")] public string? Id { get; init; }

    /// <summary>The issue key, e.g. <c>TEST-1</c>.</summary>
    [JsonPropertyName("key")] public string Key { get; init; } = "";

    /// <summary>The issue version; pass it to an update to reject edits of a stale copy.</summary>
    [JsonPropertyName("version")] public long? Version { get; init; }

    /// <summary>The title.</summary>
    [JsonPropertyName("summary")] public string? Summary { get; init; }

    /// <summary>The description, in Yandex Flavored Markdown.</summary>
    [JsonPropertyName("description")] public string? Description { get; init; }

    /// <summary>The current status.</summary>
    [JsonPropertyName("status")] public ObjectReference? Status { get; init; }

    /// <summary>The issue type (task, bug, epic, …).</summary>
    [JsonPropertyName("type")] public ObjectReference? Type { get; init; }

    /// <summary>The priority (<c>blocker</c>, <c>critical</c>, <c>normal</c>, <c>minor</c>, <c>trivial</c>).</summary>
    [JsonPropertyName("priority")] public ObjectReference? Priority { get; init; }

    /// <summary>The queue the issue belongs to.</summary>
    [JsonPropertyName("queue")] public ObjectReference? Queue { get; init; }

    /// <summary>The resolution of a closed issue; <see langword="null"/> while it is open.</summary>
    [JsonPropertyName("resolution")] public ObjectReference? Resolution { get; init; }

    /// <summary>The assignee; its <see cref="ObjectReference.Id"/> is the user's uid.</summary>
    [JsonPropertyName("assignee")] public ObjectReference? Assignee { get; init; }

    /// <summary>Who created the issue.</summary>
    [JsonPropertyName("createdBy")] public ObjectReference? CreatedBy { get; init; }

    /// <summary>The parent issue, when this one is a subtask.</summary>
    [JsonPropertyName("parent")] public ObjectReference? Parent { get; init; }

    /// <summary>When the issue was created.</summary>
    [JsonPropertyName("createdAt")] public DateTimeOffset? CreatedAt { get; init; }

    /// <summary>When the issue last changed; incremental sync searches by it.</summary>
    [JsonPropertyName("updatedAt")] public DateTimeOffset? UpdatedAt { get; init; }

    /// <summary>When the issue was resolved.</summary>
    [JsonPropertyName("resolvedAt")] public DateTimeOffset? ResolvedAt { get; init; }

    /// <summary>The planned start date.</summary>
    [JsonPropertyName("start")] public DateOnly? Start { get; init; }

    /// <summary>The planned end date.</summary>
    [JsonPropertyName("end")] public DateOnly? End { get; init; }

    /// <summary>The deadline.</summary>
    [JsonPropertyName("deadline")] public DateOnly? Deadline { get; init; }

    /// <summary>The original estimate as an ISO 8601 duration; convert with <see cref="YandexTrackerDuration.ToHours"/>.</summary>
    [JsonPropertyName("originalEstimation")] public string? OriginalEstimation { get; init; }

    /// <summary>The current estimate of the remaining work, as an ISO 8601 duration.</summary>
    [JsonPropertyName("estimation")] public string? Estimation { get; init; }

    /// <summary>The time spent, as an ISO 8601 duration.</summary>
    [JsonPropertyName("spent")] public string? Spent { get; init; }

    /// <summary>Story points.</summary>
    [JsonPropertyName("storyPoints")] public decimal? StoryPoints { get; init; }

    /// <summary>Tags.</summary>
    [JsonPropertyName("tags")] public IReadOnlyList<string>? Tags { get; init; }

    /// <summary>Fields with no typed property: queue-local fields and fields the API adds later.</summary>
    [JsonExtensionData] public IDictionary<string, JsonElement>? Fields { get; init; }
}
