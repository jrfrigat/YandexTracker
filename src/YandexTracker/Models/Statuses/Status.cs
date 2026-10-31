using System.Text.Json.Serialization;

namespace YandexTracker.Models.Statuses;

/// <summary>An issue status of the organization.</summary>
/// <param name="Id">The status id.</param>
/// <param name="Key">The status key, e.g. <c>open</c> or <c>inProgress</c>.</param>
/// <param name="Name">The status name.</param>
/// <param name="Description">The status description.</param>
/// <param name="Order">The position of the status in the list.</param>
public sealed record Status(
    [property: JsonPropertyName("id")] long? Id,
    [property: JsonPropertyName("key")] string Key,
    [property: JsonPropertyName("name")] string? Name,
    [property: JsonPropertyName("description")] string? Description = null,
    [property: JsonPropertyName("order")] int? Order = null);
