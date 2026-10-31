using System.Text.Json.Serialization;

namespace YandexTracker.Models.Issues;

/// <summary>A transition available to an issue: the only way to change its status.</summary>
/// <param name="Id">The transition id to execute.</param>
/// <param name="Display">The transition's name.</param>
/// <param name="To">The status the transition leads to.</param>
public sealed record Transition(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("display")] string? Display,
    [property: JsonPropertyName("to")] ObjectReference? To);
