using System.Text.Json.Serialization;

namespace YandexTracker.Models.Issues;

/// <summary>A new issue. The queue and the title are required; the other fields are those of Tracker's issue form.</summary>
public sealed record IssueDraft
{
    /// <summary>The key of the queue to create the issue in.</summary>
    [JsonPropertyName("queue")] public required string Queue { get; init; }

    /// <summary>The title.</summary>
    [JsonPropertyName("summary")] public required string Summary { get; init; }

    /// <summary>The description.</summary>
    [JsonPropertyName("description")] public string? Description { get; init; }

    /// <summary>The issue type key.</summary>
    [JsonPropertyName("type")] public string? Type { get; init; }

    /// <summary>The priority key.</summary>
    [JsonPropertyName("priority")] public string? Priority { get; init; }

    /// <summary>The assignee's login or uid.</summary>
    [JsonPropertyName("assignee")] public string? Assignee { get; init; }

    /// <summary>The parent issue key.</summary>
    [JsonPropertyName("parent")] public string? Parent { get; init; }

    /// <summary>The planned start date.</summary>
    [JsonPropertyName("start")] public DateOnly? Start { get; init; }

    /// <summary>The planned end date.</summary>
    [JsonPropertyName("end")] public DateOnly? End { get; init; }

    /// <summary>The deadline.</summary>
    [JsonPropertyName("deadline")] public DateOnly? Deadline { get; init; }

    /// <summary>The original estimate as an ISO 8601 duration; build one with <see cref="YandexTrackerDuration.FromHours"/>.</summary>
    [JsonPropertyName("originalEstimation")] public string? OriginalEstimation { get; init; }

    /// <summary>Tags.</summary>
    [JsonPropertyName("tags")] public IReadOnlyList<string>? Tags { get; init; }

    /// <summary>An idempotency key: repeating the request with the same key does not create a second issue.</summary>
    [JsonPropertyName("unique")] public string? Unique { get; init; }

    /// <summary>Queue-local fields, written as they are.</summary>
    [JsonExtensionData] public IDictionary<string, object?>? Fields { get; init; }
}
