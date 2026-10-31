using System.Text.Json.Serialization;

namespace YandexTracker.Models.Issues;

/// <summary>A comment on an issue.</summary>
/// <param name="Id">The comment id.</param>
/// <param name="LongId">The comment's long string id.</param>
/// <param name="Text">The text.</param>
/// <param name="CreatedBy">The author.</param>
/// <param name="CreatedAt">When the comment was written.</param>
/// <param name="UpdatedAt">When the comment was last edited.</param>
public sealed record Comment(
    [property: JsonPropertyName("id")] long Id,
    [property: JsonPropertyName("longId")] string? LongId,
    [property: JsonPropertyName("text")] string? Text,
    [property: JsonPropertyName("createdBy")] ObjectReference? CreatedBy,
    [property: JsonPropertyName("createdAt")] DateTimeOffset? CreatedAt,
    [property: JsonPropertyName("updatedAt")] DateTimeOffset? UpdatedAt = null);
