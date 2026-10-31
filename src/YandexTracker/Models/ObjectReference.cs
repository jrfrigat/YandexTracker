using System.Text.Json.Serialization;

namespace YandexTracker.Models;

/// <summary>
/// A reference to another Tracker object inside a response: a status, queue, type, user or parent issue. Different
/// objects fill different fields - an issue has a <see cref="Key"/>, a user has an <see cref="Id"/> and a
/// <see cref="Display"/> name.
/// </summary>
/// <param name="Self">The API address of the referenced object.</param>
/// <param name="Id">The object's id (for a user, the uid).</param>
/// <param name="Key">The object's key, e.g. <c>TEST-1</c> for an issue or <c>inProgress</c> for a status.</param>
/// <param name="Display">The object's human-readable name.</param>
public sealed record ObjectReference(
    [property: JsonPropertyName("self")] string? Self = null,
    [property: JsonPropertyName("id")] string? Id = null,
    [property: JsonPropertyName("key")] string? Key = null,
    [property: JsonPropertyName("display")] string? Display = null);
