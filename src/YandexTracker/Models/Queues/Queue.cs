using System.Text.Json.Serialization;

namespace YandexTracker.Models.Queues;

/// <summary>An issue queue: its key, name, lead and the defaults for new issues.</summary>
/// <param name="Id">The queue id.</param>
/// <param name="Key">The queue key, e.g. <c>TEST</c>.</param>
/// <param name="Name">The queue name.</param>
/// <param name="Description">The queue description.</param>
/// <param name="Lead">The queue lead.</param>
/// <param name="DefaultType">The issue type new issues get by default.</param>
/// <param name="DefaultPriority">The priority new issues get by default.</param>
public sealed record Queue(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("description")] string? Description = null,
    [property: JsonPropertyName("lead")] ObjectReference? Lead = null,
    [property: JsonPropertyName("defaultType")] ObjectReference? DefaultType = null,
    [property: JsonPropertyName("defaultPriority")] ObjectReference? DefaultPriority = null);
